using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.DataAccess.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string Description { get; set; }
        public List<CartItem> CartItems { get; set; }
        public Product()
        {
            CartItems = new List<CartItem>();
        }
        public Image Image { get; set; }
    }
}