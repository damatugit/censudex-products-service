using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TallerDos.ProductsService.src.models
{
    /// <summary>
    /// Producto
    /// </summary> <summary>
    /// Modelado del producto para el servicio a usar
    /// </summary>
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; } = null!;

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("category")]
        public string Category { get; set; } = null!;

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("imageUrl")]
        public string ImageUrl { get; set; } = null!;

        [BsonElement("imagePublicId")]
        public string ImagePublicId { get; set; } = null!;

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;
        
        [BsonElement("dateCreated")]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}