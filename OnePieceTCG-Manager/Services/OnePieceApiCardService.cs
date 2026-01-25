using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using OnePieceTCG_Manager.Models;

namespace OnePieceTCG_Manager.Services
{
    public class OnePieceApiCardService
    {
        private readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://optcgapi.aquero.es/")
        };

        public OnePieceApiCardService() { }

        // Carta completa
        public async Task<CardApi> GetCardAsync(string cardId)
        {
            return await GetAsync<CardApi>($"card/{cardId}");
        }

        // Todas las imágenes (básica + alters)
        public async Task<List<string>> GetCardImagesAsync(string cardId)
        {
            return await GetAsync<List<string>>($"card/{cardId}/images");
        }

        // Solo alters
        public async Task<List<string>> GetAlterImagesAsync(string cardId)
        {
            return await GetAsync<List<string>>($"card/{cardId}/images/alter");
        }

        // Método genérico privado
        private async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
                return default;

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
