using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TallerDos.ProductsService.src.DTOs
{
    /// <summary>
    /// UpdateProductoDto
    /// </summary> <summary>
    /// Dto/modelo para actualizar un producto
    /// </summary>
    public class UpdateProductDto
    {
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; } 
        public string? ImagePublicId { get; set; }
    }
}