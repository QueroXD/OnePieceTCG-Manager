using Newtonsoft.Json;
using OnePieceTCG_Manager.Properties;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnePieceTCG_Manager.Services
{
    public class UsuariosService
    {
        private readonly HttpClient _http;

        public UsuariosService()
        {
            _http = new HttpClient { BaseAddress = new Uri(Settings.Default.db_api) };
        }

        public async Task<UsuarioResponse> LoginAsync(string userName, string password)
        {
            var response = await _http.PostAsync($"/Usuarios/Login?userName={Uri.EscapeDataString(userName)}&password={Uri.EscapeDataString(password)}", null);

            if (!response.IsSuccessStatusCode)
                return null;

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UsuarioResponse>(json);
        }

        public async Task<UsuarioResponse> AutoLoginAsync(string hostname)
        {
            var response = await _http.GetAsync($"/Usuarios/AutoLogin/{Uri.EscapeDataString(hostname)}");

            if (!response.IsSuccessStatusCode)
                return null;

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UsuarioResponse>(json);
        }
    }

    public class UsuarioResponse
    {
        public string CodUsu { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Passwd { get; set; } = "";
    }
}
