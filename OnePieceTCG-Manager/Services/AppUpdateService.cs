using Newtonsoft.Json;
using OnePieceTCG_Manager.Models;
using OnePieceTCG_Manager.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnePieceTCG_Manager.Services
{
    internal sealed class AppUpdateService
    {
        private static readonly HttpClient HttpClient = CreateHttpClient();

        public async Task<UpdateCheckResult> CheckForUpdatesAsync()
        {
            Version currentVersion = GetCurrentVersion();
            string manifestUrl = Settings.Default.update_manifest_url;

            if (string.IsNullOrWhiteSpace(manifestUrl))
                return UpdateCheckResult.NoUpdate(currentVersion);

            try
            {
                string json = await HttpClient.GetStringAsync(manifestUrl).ConfigureAwait(false);
                UpdateManifest manifest = JsonConvert.DeserializeObject<UpdateManifest>(json);

                if (manifest == null || string.IsNullOrWhiteSpace(manifest.Version))
                {
                    return new UpdateCheckResult
                    {
                        CurrentVersion = currentVersion,
                        IsUpdateAvailable = false,
                        ErrorMessage = "No se pudo interpretar el manifiesto de actualización."
                    };
                }

                Version latestVersion;
                if (!Version.TryParse(manifest.Version, out latestVersion))
                {
                    return new UpdateCheckResult
                    {
                        CurrentVersion = currentVersion,
                        IsUpdateAvailable = false,
                        ErrorMessage = "La versión remota no tiene un formato válido."
                    };
                }

                return new UpdateCheckResult
                {
                    CurrentVersion = currentVersion,
                    LatestVersion = latestVersion,
                    Manifest = manifest,
                    IsUpdateAvailable = latestVersion > currentVersion
                };
            }
            catch (Exception ex)
            {
                return new UpdateCheckResult
                {
                    CurrentVersion = currentVersion,
                    IsUpdateAvailable = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<DownloadedUpdatePackage> DownloadUpdateAsync(UpdateManifest manifest)
        {
            if (manifest == null)
                throw new ArgumentNullException("manifest");

            if (string.IsNullOrWhiteSpace(manifest.DownloadUrl))
                throw new InvalidOperationException("La actualización no incluye una URL de descarga.");

            string updateRoot = Path.Combine(Path.GetTempPath(), "OnePieceTCG-Manager", "updates", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(updateRoot);

            string packagePath = Path.Combine(updateRoot, "OnePieceTCG-Manager.zip");
            string scriptPath = Path.Combine(updateRoot, "apply-update.ps1");

            using (HttpResponseMessage response = await HttpClient.GetAsync(manifest.DownloadUrl).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();

                using (Stream remoteStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                using (FileStream localStream = new FileStream(packagePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await remoteStream.CopyToAsync(localStream).ConfigureAwait(false);
                }
            }

            if (!string.IsNullOrWhiteSpace(manifest.Sha256))
                ValidateChecksum(packagePath, manifest.Sha256);

            File.WriteAllText(scriptPath, BuildUpdaterScript(), Encoding.UTF8);

            return new DownloadedUpdatePackage
            {
                PackagePath = packagePath,
                ScriptPath = scriptPath
            };
        }

        public string GetUpdaterLogPath()
        {
            return Path.Combine(GetLogDirectoryPath(), "updater.log");
        }

        public string GetLauncherLogPath()
        {
            return Path.Combine(GetLogDirectoryPath(), "launcher.log");
        }

        public void LaunchUpdaterAndExit(DownloadedUpdatePackage package)
        {
            if (package == null)
                throw new ArgumentNullException("package");

            string executablePath = Application.ExecutablePath;
            string installDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string executableName = Path.GetFileName(executablePath);
            string launcherLogPath = GetLauncherLogPath();

            Directory.CreateDirectory(GetLogDirectoryPath());
            File.AppendAllText(launcherLogPath, string.Format("{0:s} Lanzando actualizador. InstallDir={1}; Script={2}; Zip={3}{4}", DateTime.Now, installDirectory, package.ScriptPath, package.PackagePath, Environment.NewLine));

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = string.Format(
                    "-NoLogo -ExecutionPolicy Bypass -File \"{0}\" -ParentProcessId {1} -ZipPath \"{2}\" -InstallDir \"{3}\" -ExeName \"{4}\"",
                    package.ScriptPath,
                    Process.GetCurrentProcess().Id,
                    package.PackagePath,
                    installDirectory,
                    executableName),
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Normal,
                WorkingDirectory = installDirectory
            };

            Process process = Process.Start(startInfo);
            File.AppendAllText(launcherLogPath, string.Format("{0:s} Actualizador lanzado. PID={1}{2}", DateTime.Now, process != null ? process.Id.ToString() : "null", Environment.NewLine));
        }

        private static HttpClient CreateHttpClient()
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(20);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("OnePieceTCG-Manager-Updater/1.0");
            return client;
        }

        private static Version GetCurrentVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version ?? new Version(1, 0, 0, 0);
        }

        private static void ValidateChecksum(string filePath, string expectedHash)
        {
            string normalizedExpected = expectedHash.Replace("-", string.Empty).Trim().ToUpperInvariant();
            using (SHA256 sha256 = SHA256.Create())
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byte[] hash = sha256.ComputeHash(stream);
                string actualHash = BitConverter.ToString(hash).Replace("-", string.Empty).ToUpperInvariant();
                if (!string.Equals(actualHash, normalizedExpected, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("La firma SHA256 del paquete no coincide con el manifiesto publicado.");
            }
        }

        private static string GetLogDirectoryPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OnePieceTCG-Manager", "Logs");
        }

        private static string BuildUpdaterScript()
        {
            return @"param(
    [int]$ParentProcessId,
    [string]$ZipPath,
    [string]$InstallDir,
    [string]$ExeName
)

$ErrorActionPreference = 'Stop'
$logDir = Join-Path $env:LOCALAPPDATA 'OnePieceTCG-Manager\Logs'
$logPath = Join-Path $logDir 'updater.log'
New-Item -ItemType Directory -Path $logDir -Force | Out-Null

function Write-Log {
    param([string]$Message)
    $line = (Get-Date).ToString('s') + ' ' + $Message
    Add-Content -Path $logPath -Value $line
    Write-Host $line
}

try {
    Write-Log ('Inicio de actualización. Destino=' + $InstallDir + '; Zip=' + $ZipPath)
    Write-Host 'Esperando a que se cierre la aplicación...'

    while (Get-Process -Id $ParentProcessId -ErrorAction SilentlyContinue) {
        Start-Sleep -Milliseconds 500
    }

    $stagingDir = Join-Path ([System.IO.Path]::GetTempPath()) ('OPTCG-Manager-' + [guid]::NewGuid().ToString('N'))
    New-Item -ItemType Directory -Path $stagingDir -Force | Out-Null
    Expand-Archive -Path $ZipPath -DestinationPath $stagingDir -Force
    Write-Log ('ZIP expandido en ' + $stagingDir)
    Write-Host 'Copiando nueva versión...'

    $exe = Get-ChildItem -Path $stagingDir -Filter $ExeName -Recurse | Select-Object -First 1
    if (-not $exe) {
        throw 'No se encontró el ejecutable dentro del paquete descargado.'
    }

    $packageRoot = Split-Path -Path $exe.FullName -Parent
    New-Item -ItemType Directory -Path $InstallDir -Force | Out-Null

    $robocopyArgs = @($packageRoot, $InstallDir, '/E', '/R:3', '/W:1', '/NFL', '/NDL', '/NJH', '/NJS', '/NP')
    $robocopy = Start-Process -FilePath 'robocopy.exe' -ArgumentList $robocopyArgs -Wait -PassThru -NoNewWindow
    Write-Log ('Robocopy exit code: ' + $robocopy.ExitCode)

    if ($robocopy.ExitCode -ge 8) {
        throw ('Robocopy devolvió un código de error: ' + $robocopy.ExitCode)
    }

    $targetExe = Join-Path $InstallDir $ExeName
    if (-not (Test-Path $targetExe)) {
        throw ('No se encontró el ejecutable actualizado en ' + $targetExe)
    }

    Write-Log ('Reinicio de aplicación: ' + $targetExe)
    Write-Host 'Abriendo la aplicación actualizada...'
    Start-Process -FilePath $targetExe
    Start-Sleep -Seconds 2

    Remove-Item -Path $stagingDir -Recurse -Force -ErrorAction SilentlyContinue
    Remove-Item -Path $ZipPath -Force -ErrorAction SilentlyContinue
    Remove-Item -Path $PSCommandPath -Force -ErrorAction SilentlyContinue
    Write-Log 'Actualización completada.'
    Write-Host 'Actualización completada.'
    Start-Sleep -Seconds 3
}
catch {
    Write-Log ('ERROR: ' + $_.Exception.Message)
    Write-Host ''
    Write-Host 'La actualización ha fallado.' -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    Write-Host ''
    Write-Host ('Revisa el log en: ' + $logPath)
    Read-Host 'Pulsa Enter para cerrar'
    throw
}
";
        }
    }
}
