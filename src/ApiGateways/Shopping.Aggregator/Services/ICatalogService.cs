using Shopping.Aggregator.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<CatalogModel>> GetCatalogs();
        Task<IEnumerable<CatalogModel>> GetCatalogsByCategory(string category);
        Task<CatalogModel> GetCatalog(string id);
    }
}
