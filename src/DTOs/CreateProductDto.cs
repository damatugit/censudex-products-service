using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TallerDos.ProductsService.src.DTOs
{
    public class CreateProductDto
    {
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string ImagePublicId { get; set; } = null!;
    }
}