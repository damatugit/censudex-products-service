using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using TallerDos.ProductsService.src.Data;
using TallerDos.ProductsService.src.models;

namespace TallerDos.ProductsService.src.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _collection;

        public ProductRepository(MongoDBContext context)
        {
            _collection = context.Products;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var filter = Builders<Product>.Filter.Empty;
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            return await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Product product)
        {
            await _collection.InsertOneAsync(product);
        }

        public async Task UpdateAsync(Product product)
        {
            await _collection.ReplaceOneAsync(p => p.Id == product.Id, product);
        }

        public async Task SoftDeleteAsync(string id)
        {
            var update = Builders<Product>.Update.Set(p => p.IsActive, false);
            await _collection.UpdateOneAsync(p => p.Id == id, update);
        }

        public async Task<bool> ExistsByNameAsync(string name, string? excludeId = null)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            if (!string.IsNullOrEmpty(excludeId))
                filter = Builders<Product>.Filter.And(filter, Builders<Product>.Filter.Ne(p => p.Id, excludeId));
            return await _collection.Find(filter).AnyAsync();
        }
            
    }
}