using Newtonsoft.Json;

namespace OnePieceTCG_Manager.Models
{
    internal sealed class UpdateManifest
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("downloadUrl")]
        public string DownloadUrl { get; set; }

        [JsonProperty("sha256")]
        public string Sha256 { get; set; }

        [JsonProperty("publishedAt")]
        public string PublishedAt { get; set; }

        [JsonProperty("releaseNotesUrl")]
        public string ReleaseNotesUrl { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("mandatory")]
        public bool Mandatory { get; set; }
    }
}
