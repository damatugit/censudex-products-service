using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        [RequestSizeLimit(10_000_000)] // ejemplo
        public async Task<IActionResult> Create([FromForm] Product product, IFormFile image)
        {
            // validaciones básicas
            if (string.IsNullOrWhiteSpace(product.Name)) return BadRequest("Name required.");

            if (await _repo.ExistsByNameAsync(product.Name))
                return Conflict("Product name already exists.");

            product.Id = Guid.NewGuid().ToString();
            if (image != null)
            {
                var url = await _cloudinary.UploadImageAsync(image, publicId: $"products/{product.Id}");
                product.ImageUrl = url!; //nunca la url sera null
            }

            product.DateCreated = DateTime.UtcNow;
            product.IsActive = true;

            await _repo.CreateAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // PUT: api/products/{id}
        // [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromForm] Product updated, IFormFile image)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return NotFound();

            if (!string.Equals(existing.Name, updated.Name, StringComparison.OrdinalIgnoreCase))
            {
                if (await _repo.ExistsByNameAsync(updated.Name, excludeId: id))
                    return Conflict("Product name already exists.");
            }

            // campos editables
            existing.Name = updated.Name;
            existing.Price = updated.Price;
            existing.Category = updated.Category;

            if (image != null)
            {
                var url = await _cloudinary.UploadImageAsync(image, publicId: $"products/{existing.Id}");
                if (!string.IsNullOrEmpty(url)) existing.ImageUrl = url;
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
