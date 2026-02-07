using Newtonsoft.Json;
using OnePieceTCG_Manager.Models;
using OnePieceTCG_Manager.Properties;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnePieceTCG_Manager.Services
{
    public class DecksService
    {
        private readonly HttpClient _http;

        public DecksService()
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri(Settings.Default.db_api)
            };
        }

        // Get Decks by CodUsu
        public async Task<List<DeckRow>> GetDecksByUserAsync(string codUsu)
        {
            var response = await _http.GetAsync($"Decks/{codUsu}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<DeckRow>>(json)
                   ?? new List<DeckRow>();
        }

        // Delete Deck by Id
        public async Task DeleteDeckAsync(Guid deckId)
        {
            var response = await _http.DeleteAsync($"Decks/{deckId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<DeckEditDto> GetDeckForEditAsync(Guid deckId)
        {
            var response = await _http.GetAsync($"Decks/edit/{deckId}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DeckEditDto>(json);
        }

        public async Task SaveDeckAsync(DeckSaveDto dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("Decks", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
