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
        /// <summary>
        /// Identificador unico del producto
        /// </summary>
        /// <value></value>
        public string Id { get; set; } = null!;

        [BsonElement("name")]
        /// <summary>
        /// Nombre del producto
        /// </summary>
        /// <value></value>
        public string Name { get; set; } = null!;

        [BsonElement("category")]
        /// <summary>
        /// Categoría del producto
        /// </summary>
        /// <value></value>
        public string Category { get; set; } = null!;

        [BsonElement("price")]
        /// <summary>
        /// Precio a pagar del producto
        /// </summary>
        /// <value></value>
        public decimal Price { get; set; }

        [BsonElement("imageUrl")]
        /// <summary>
        /// URL de la imagen del producto
        /// </summary>
        /// <value></value>
        public string ImageUrl { get; set; } = null!;

        [BsonElement("imagePublicId")]
        /// <summary>
        /// Id publica de la imagen del producto
        /// </summary>
        /// <value></value>
        public string ImagePublicId { get; set; } = null!;

        [BsonElement("isActive")]
        /// <summary>
        /// Indicador de si el produco esta activado
        /// </summary>
        /// <value></value>
        public bool IsActive { get; set; } = true;
        
        [BsonElement("dateCreated")]
        /// <summary>
        /// Fecha y hora de la creación del producto
        /// </summary>
        /// <value></value>
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}