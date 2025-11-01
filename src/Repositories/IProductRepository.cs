using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TallerDos.ProductsService.src.models;

namespace TallerDos.ProductsService.src.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(string id);
        Task CreateAsync(Product product);
        Task UpdateAsync(Product product);
        Task SoftDeleteAsync(string id);
        Task<bool> ExistsByNameAsync(string name, string excludeId);        
    }
}