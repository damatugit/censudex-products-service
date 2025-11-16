using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Products;
using TallerDos.ProductsService.src.Repositories;

namespace TallerDos.ProductsService.src.Services
{
    /// <summary>
    /// Servicio Grpc para configurar puerto del servicio de productos
    /// </summary>
    public class ProductsGrpcService : ProductService.ProductServiceBase
    {
        private readonly IProductRepository _repo;

        public ProductsGrpcService(IProductRepository repo)
        {
            _repo = repo;
        }

        // GET ALL PRODUCTS
        public override async Task<ProductListResponse> GetAllProducts(
            GetAllProductsRequest request, 
            ServerCallContext context)
        {
            var products = await _repo.GetAllAsync();
            
            var response = new ProductListResponse
            {
                Success = true,
                Message = "Products retrieved successfully",
                Count = products.Count()
            };

            foreach (var p in products)
            {
                response.Products.Add(new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category,
                    Price = (double)p.Price,
                    ImageUrl = p.ImageUrl ?? "",
                    ImagePublicId = p.ImagePublicId ?? "",
                    IsActive = p.IsActive,
                    DateCreated = p.DateCreated.ToString("o")
                });
            }

            return response;
        }

        // GET PRODUCT BY ID
        public override async Task<ProductResponse> GetProductById(
            GetProductByIdRequest request, 
            ServerCallContext context)
        {
            var product = await _repo.GetByIdAsync(request.Id);

            if (product == null)
            {
                return new ProductResponse
                {
                    Success = false,
                    Message = "Product not found"
                };
            }

            return new ProductResponse
            {
                Success = true,
                Message = "Product retrieved successfully",
                Product = new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Category = product.Category,
                    Price = (double)product.Price,
                    ImageUrl = product.ImageUrl ?? "",
                    ImagePublicId = product.ImagePublicId ?? "",
                    IsActive = product.IsActive,
                    DateCreated = product.DateCreated.ToString("o")
                }
            };
        }

        // CREATE PRODUCT
        public override async Task<ProductResponse> CreateProduct(
            CreateProductRequest request, 
            ServerCallContext context)
        {
            try
            {
                // Validar que el nombre no exista
                if (await _repo.ExistsByNameAsync(request.Name))
                {
                    return new ProductResponse
                    {
                        Success = false,
                        Message = "Product name already exists"
                    };
                }

                var product = new TallerDos.ProductsService.src.models.Product
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = request.Name,
                    Category = request.Category,
                    Price = (decimal)request.Price,
                    ImageUrl = request.ImageUrl ?? "",
                    ImagePublicId = "",
                    IsActive = true,
                    DateCreated = DateTime.UtcNow
                };

                await _repo.CreateAsync(product);

                return new ProductResponse
                {
                    Success = true,
                    Message = "Product created successfully",
                    Product = new Products.Product
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Category = product.Category,
                        Price = (double)product.Price,
                        ImageUrl = product.ImageUrl,
                        ImagePublicId = product.ImagePublicId,
                        IsActive = product.IsActive,
                        DateCreated = product.DateCreated.ToString("o")
                    }
                };
            }
            catch (Exception ex)
            {
                return new ProductResponse
                {
                    Success = false,
                    Message = $"Error creating product: {ex.Message}"
                };
            }
        }

        // UPDATE PRODUCT
        public override async Task<ProductResponse> UpdateProduct(
            UpdateProductRequest request, 
            ServerCallContext context)
        {
            try
            {
                var existing = await _repo.GetByIdAsync(request.Id);

                if (existing == null)
                {
                    return new ProductResponse
                    {
                        Success = false,
                        Message = "Product not found"
                    };
                }

                // Validar nombre duplicado (excluyendo el producto actual)
                if (!existing.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase))
                {
                    if (await _repo.ExistsByNameAsync(request.Name, excludeId: request.Id))
                    {
                        return new ProductResponse
                        {
                            Success = false,
                            Message = "Product name already exists"
                        };
                    }
                }

                existing.Name = request.Name;
                existing.Category = request.Category;
                existing.Price = (decimal)request.Price;
                
                if (!string.IsNullOrEmpty(request.ImageUrl))
                {
                    existing.ImageUrl = request.ImageUrl;
                }

                await _repo.UpdateAsync(existing);

                return new ProductResponse
                {
                    Success = true,
                    Message = "Product updated successfully",
                    Product = new Products.Product
                    {
                        Id = existing.Id,
                        Name = existing.Name,
                        Category = existing.Category,
                        Price = (double)existing.Price,
                        ImageUrl = existing.ImageUrl,
                        ImagePublicId = existing.ImagePublicId,
                        IsActive = existing.IsActive,
                        DateCreated = existing.DateCreated.ToString("o")
                    }
                };
            }
            catch (Exception ex)
            {
                return new ProductResponse
                {
                    Success = false,
                    Message = $"Error updating product: {ex.Message}"
                };
            }
        }

        // DELETE PRODUCT
        public override async Task<DeleteProductResponse> DeleteProduct(
            DeleteProductRequest request, 
            ServerCallContext context)
        {
            try
            {
                var existing = await _repo.GetByIdAsync(request.Id);

                if (existing == null)
                {
                    return new DeleteProductResponse
                    {
                        Success = false,
                        Message = "Product not found"
                    };
                }

                await _repo.SoftDeleteAsync(request.Id);

                return new DeleteProductResponse
                {
                    Success = true,
                    Message = "Product deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new DeleteProductResponse
                {
                    Success = false,
                    Message = $"Error deleting product: {ex.Message}"
                };
            }
        }
    }
}