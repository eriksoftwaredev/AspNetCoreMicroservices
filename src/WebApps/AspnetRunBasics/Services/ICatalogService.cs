using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspnetRunBasics.Models;

namespace AspnetRunBasics.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<CatalogModel>> GetCatalogs();
        Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category);
        Task<CatalogModel> GetCatalog(string id);
        Task<CatalogModel> CreateCatalog(CatalogModel model);
    }
}
