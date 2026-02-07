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

        private List<CardStock> _cache;
        private bool _isLoaded;

        public CardStockService()
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri(Settings.Default.db_api)
            };
        }

        // --------------------------
        // Carga cache si no existe
        // --------------------------
        public async Task<List<CardStock>> GetAllAsync(bool forceReload = false)
        {
            if (_isLoaded && !forceReload)
                return _cache;

            var response = await _http.GetAsync("/CardStock");
            if (!response.IsSuccessStatusCode)
                return new List<CardStock>();

            var json = await response.Content.ReadAsStringAsync();
            _cache = JsonConvert.DeserializeObject<List<CardStock>>(json) ?? new List<CardStock>();
            _isLoaded = true;

            return _cache;
        }

        // --------------------------
        // Obtener una carta del cache
        // --------------------------
        public async Task<CardStock> GetByKeyAsync(string cardId, bool isAlter, string cardImage)
        {
            var all = await GetAllAsync();

            return all.FirstOrDefault(c =>
                c.cardId == cardId &&
                c.isAlter == isAlter &&
                c.cardImage == cardImage);
        }

        // --------------------------
        // Obtener varias cartas por ids
        // --------------------------
        public async Task<List<CardStock>> GetByIdsAsync(List<Guid> ids)
        {
            if (ids == null || ids.Count == 0) return new List<CardStock>();

            // Convertimos a query string
            string query = string.Join(",", ids);
            var response = await _http.GetAsync($"CardStock/by-ids?ids={query}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<CardStock>>(json) ?? new List<CardStock>();
        }

        // --------------------------
        // Crear carta
        // --------------------------
        public async Task CreateAsync(CardStock card)
        {
            var json = JsonConvert.SerializeObject(card);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("/CardStock", content);
            response.EnsureSuccessStatusCode();

            // ❌ cache inválido
            InvalidateCache();
        }

        // --------------------------
        // Update completo
        // --------------------------
        public async Task UpdateAsync(Guid id, CardStock card)
        {
            var json = JsonConvert.SerializeObject(card);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PutAsync($"/CardStock/{id}", content);
            response.EnsureSuccessStatusCode();

            InvalidateCache();
        }

        // --------------------------
        // Update solo unidades
        // --------------------------
        public async Task UpdateUnitsAsync(Guid id, int units)
        {
            var response = await _http.PutAsync(
                $"/CardStock/{id}/Units?units={units}", null);

            response.EnsureSuccessStatusCode();

            InvalidateCache();
        }

        // --------------------------
        // Invalidar cache
        // --------------------------
        private void InvalidateCache()
        {
            _isLoaded = false;
            _cache = null;
        }
    }
}
