using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TallerDos.ProductsService.src.DTOs;
using TallerDos.ProductsService.src.models;
using TallerDos.ProductsService.src.Repositories;
using TallerDos.ProductsService.src.Services;

namespace TallerDos.ProductsService.src.Controllers
{    
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;
        private readonly CloudinaryService _cloudinary;

        public ProductsController(IProductRepository repo, CloudinaryService cloudinary)
        {
            _repo = repo;
            _cloudinary = cloudinary;
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _repo.GetAllAsync();
            return Ok(products);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var prod = await _repo.GetByIdAsync(id);
            if (prod == null) return NotFound();
            return Ok(prod);
        }

        // POST: api/products
        // [Authorize(Roles = "Admin")] -- habilita cuando tengas auth
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductDto dto)
        {
            // validaciones básicas
            if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest("Name is required.");

            if (await _repo.ExistsByNameAsync(dto.Name))
                return Conflict("Product name already exists.");

            var product = new Product
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                Category = dto.Category,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl ?? "",
                ImagePublicId = "",  
                DateCreated = DateTime.UtcNow,
                IsActive = true
            };

            await _repo.CreateAsync(product);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // PUT: api/products/{id}
        // [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateProductDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return NotFound();

            if (!existing.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase))
            {
                if (await _repo.ExistsByNameAsync(dto.Name, excludeId: id))
                    return Conflict("Product name already exists.");
            }

            existing.Name = dto.Name;
            existing.Category = dto.Category;
            existing.Price = dto.Price;

            if (!string.IsNullOrEmpty(dto.ImageUrl))
            {
                existing.ImageUrl = dto.ImageUrl;
            }

            await _repo.UpdateAsync(existing);

            return Ok(existing);
        }

        // DELETE (soft delete)
        // [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return NotFound();

            await _repo.SoftDeleteAsync(id);
            return Ok();
        }
    }
}
