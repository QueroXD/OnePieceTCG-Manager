# OnePieceTCG-Manager

## Sistema de autoactualización

He dejado preparado un sistema para que la aplicación haga esto:

- Comprueba al arrancar si la versión local coincide con la última publicada desde `main`.
- Consulta un manifiesto remoto en `update/version.json` publicado desde GitHub.
- Si detecta una versión más nueva, pregunta al usuario si quiere actualizar.
- Descarga el paquete publicado en GitHub Releases.
- Verifica el `sha256` del paquete.
- Cierra la app, reemplaza los binarios y la vuelve a abrir.

## Cómo funciona la arquitectura

### 1. Versión local de la app
La versión que usa la app sale de:

- [AssemblyInfo.cs](/C:/Users/Usuario/Documents/GitHub/OnePieceTCG-Manager/OnePieceTCG-Manager/Properties/AssemblyInfo.cs)

Ahora mismo está en:

```csharp
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
```

La comparación se hace contra esa versión. Si queréis que una build nueva se considere actualización, hay que subir ese número antes de publicar cambios que deban instalarse en clientes.

### 2. Manifiesto remoto
La app consulta este archivo:

- [update/version.json](/C:/Users/Usuario/Documents/GitHub/OnePieceTCG-Manager/update/version.json)

Ese manifiesto contiene:

- `version`: versión más reciente disponible.
- `downloadUrl`: ZIP descargable de la app.
- `sha256`: hash del ZIP para validar integridad.
- `publishedAt`: fecha de publicación.
- `releaseNotesUrl`: URL de la release.
- `channel`: canal, en este caso `main`.
- `mandatory`: reservado por si queréis forzar una actualización futura.

### 3. Publicación automática desde GitHub
He añadido este workflow:

- [.github/workflows/publish-main-update.yml](/C:/Users/Usuario/Documents/GitHub/OnePieceTCG-Manager/.github/workflows/publish-main-update.yml)

Cuando entra un push en `main`, el workflow:

1. Restaura paquetes NuGet.
2. Compila en `Release`.
3. Empaqueta `bin/Release` en un ZIP.
4. Calcula el `sha256`.
5. Regenera `update/version.json`.
6. Hace commit de ese manifiesto a `main`.
7. Publica o actualiza una release rotativa con tag `main-latest`.

La app descarga siempre desde esta URL estable:

- [main-latest ZIP](https://github.com/QueroXD/OnePieceTCG-Manager/releases/download/main-latest/OnePieceTCG-Manager.zip)

## Qué necesita GitHub para que funcione

### Requisitos del repositorio
- El repositorio debe estar accesible públicamente, o el cliente no podrá leer `raw.githubusercontent.com` ni descargar la release sin autenticación.
- GitHub Actions debe estar habilitado en el repositorio.
- En `Settings > Actions > General`, el workflow necesita permiso de escritura sobre `contents`.

### Ajustes recomendados en GitHub
- Activar `Read and write permissions` para `GITHUB_TOKEN` en Actions.
- Mantener `main` como rama de publicación.
- Si usáis protección de rama, permitid que GitHub Actions pueda empujar el archivo `update/version.json` o moved ese manifiesto a otra rama/hosting si preferís un flujo más estricto.

## Qué necesita el usuario final

### Instalación recomendada
Para que la actualización automática pueda sobrescribir archivos sin pedir permisos de administrador, conviene instalar la app en una carpeta de usuario, por ejemplo:

```text
%LocalAppData%\Programs\OnePieceTCG-Manager
```

No es buena idea instalarla en `Program Files` si queréis actualizaciones silenciosas, porque normalmente requerirá elevación.

### Requisitos del equipo cliente
- Windows con PowerShell disponible.
- .NET Framework 4.8.
- Acceso HTTPS a GitHub Releases y a `raw.githubusercontent.com`.

## Archivos añadidos o modificados

### Lógica de actualización en la app
- [Program.cs](/C:/Users/Usuario/Documents/GitHub/OnePieceTCG-Manager/OnePieceTCG-Manager/Program.cs)
- [FrmMain.cs](/C:/Users/Usuario/Documents/GitHub/OnePieceTCG-Manager/OnePieceTCG-Manager/FrmMain.cs)
- [AppUpdateService.cs](/C:/Users/Usuario/Documents/GitHub/OnePieceTCG-Manager/OnePieceTCG-Manager/Services/AppUpdateService.cs)
- [UpdateManifest.cs](/C:/Users/Usuario/Documents/GitHub/OnePieceTCG-Manager/OnePieceTCG-Manager/Models/UpdateManifest.cs)
- [UpdateCheckResult.cs](/C:/Users/Usuario/Documents/GitHub/OnePieceTCG-Manager/OnePieceTCG-Manager/Models/UpdateCheckResult.cs)
- [DownloadedUpdatePackage.cs](/C:/Users/Usuario/Documents/GitHub/OnePieceTCG-Manager/OnePieceTCG-Manager/Models/DownloadedUpdatePackage.cs)

### Configuración
- [App.config](/C:/Users/Usuario/Documents/GitHub/OnePieceTCG-Manager/OnePieceTCG-Manager/App.config)
- [Settings.settings](/C:/Users/Usuario/Documents/GitHub/OnePieceTCG-Manager/OnePieceTCG-Manager/Properties/Settings.settings)
- [Settings.Designer.cs](/C:/Users/Usuario/Documents/GitHub/OnePieceTCG-Manager/OnePieceTCG-Manager/Properties/Settings.Designer.cs)
- [OnePieceTCG-Manager.csproj](/C:/Users/Usuario/Documents/GitHub/OnePieceTCG-Manager/OnePieceTCG-Manager/OnePieceTCG-Manager.csproj)
- [update/version.json](/C:/Users/Usuario/Documents/GitHub/OnePieceTCG-Manager/update/version.json)
- [.github/workflows/publish-main-update.yml](/C:/Users/Usuario/Documents/GitHub/OnePieceTCG-Manager/.github/workflows/publish-main-update.yml)

## Flujo de trabajo a partir de ahora

1. Hacéis cambios en la app.
2. Si queréis que los clientes se actualicen, subís `AssemblyVersion` y `AssemblyFileVersion`.
3. Hacéis merge o push a `main`.
4. GitHub Actions compila, empaqueta, recalcula `update/version.json` y actualiza la release `main-latest`.
5. Los clientes, al abrir la app, detectan si su versión es menor y pueden actualizarse automáticamente.

## Limitaciones actuales

- La detección se basa en `AssemblyFileVersion`, así que hay que versionar cada release instalable.
- El sistema actual está pensado para despliegue tipo carpeta/ZIP, no para ClickOnce ni MSI.
- Si el usuario no tiene permisos de escritura en la carpeta de instalación, la actualización fallará.
- Si el repositorio pasa a privado, este sistema necesitará un backend intermedio o autenticación adicional.

## Siguiente mejora recomendada

La siguiente mejora natural sería añadir un pequeño ejecutable `Updater` dedicado, en vez de usar un script de PowerShell temporal. Funcionaría mejor en entornos bloqueados y os daría más control sobre rollback, logs y actualizaciones obligatorias.
