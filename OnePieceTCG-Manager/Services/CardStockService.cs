using Newtonsoft.Json;
using OnePieceTCG_Manager.Models;
using OnePieceTCG_Manager.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OnePieceTCG_Manager.Services
{
    public class CardStockService
    {
        private readonly HttpClient _http;

        public CardStockService()
        {
            _http = new HttpClient { BaseAddress = new Uri(Settings.Default.db_api) };
        }

        public async Task<List<CardStock>> GetAllAsync()
        {
            var response = await _http.GetAsync("/CardStock");
            if (!response.IsSuccessStatusCode)
                return new List<CardStock>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<CardStock>>(json);
        }

        public async Task<CardStock> GetByIdAsync(Guid id)
        {
            var response = await _http.GetAsync($"/CardStock/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CardStock>(json);
        }

        public async Task<Guid> CreateAsync(CardStock card)
        {
            var json = JsonConvert.SerializeObject(card);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("/CardStock", content);
            response.EnsureSuccessStatusCode();

            var idStr = await response.Content.ReadAsStringAsync();
            return Guid.Parse(idStr.Trim('"')); // API devuelve el GUID como string
        }

        public async Task UpdateAsync(Guid id, CardStock card)
        {
            var json = JsonConvert.SerializeObject(card);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PutAsync($"/CardStock/{id}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateUnitsAsync(Guid id, int units)
        {
            var json = new StringContent(units.ToString(), Encoding.UTF8, "application/json");
            var response = await _http.PutAsync($"/CardStock/{id}/Units?units={units}", null);
            response.EnsureSuccessStatusCode();
        }
    }
}
