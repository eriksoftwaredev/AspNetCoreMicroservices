using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AspnetRunBasics.Extensions;
using AspnetRunBasics.Models;

namespace AspnetRunBasics.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _client;

        public CatalogService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogs()
        {
            var response = await _client.GetAsync("/Catalog");
            return await response.ReadContentAs<List<CatalogModel>>();
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category)
        {
            var response = await _client.GetAsync($"/Catalog/GetProductsByCategory/{category}");
            return await response.ReadContentAs<List<CatalogModel>>();
        }

        public async Task<CatalogModel> GetCatalog(string id)
        {
            var response = await _client.GetAsync($"/Catalog/{id}");
            return await response.ReadContentAs<CatalogModel>();
        }

        public async Task<CatalogModel> CreateCatalog(CatalogModel model)
        {
            var response = await _client.PostAsJsonAsync($"/Catalog", model);
            
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<CatalogModel>();
            else
            {
                throw new Exception("Something went wrong when calling CreateCatalog api.");
            }
        }
    }
}
