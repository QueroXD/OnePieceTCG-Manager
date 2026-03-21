using System;

namespace OnePieceTCG_Manager.Models
{
    internal sealed class UpdateCheckResult
    {
        public static UpdateCheckResult NoUpdate(Version currentVersion)
        {
            return new UpdateCheckResult
            {
                CurrentVersion = currentVersion,
                IsUpdateAvailable = false
            };
        }

        public Version CurrentVersion { get; set; }

        public Version LatestVersion { get; set; }

        public bool IsUpdateAvailable { get; set; }

        public UpdateManifest Manifest { get; set; }

        public string ErrorMessage { get; set; }
    }
}
