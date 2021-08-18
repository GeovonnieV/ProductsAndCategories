using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace ProductsAndCategories.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // im a catergory and these are my products i have
        public List<Association> CategoriesProducts {get; set;}

    }
}