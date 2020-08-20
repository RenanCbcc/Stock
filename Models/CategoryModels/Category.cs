using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.Models.CategoryModels
{
    public class Category : Base
    {
        [Required]
        [StringLength(maximumLength: 4, MinimumLength = 50,
        ErrorMessage = "O título da categoria deve ter no mímino 4 caracteres e no máximo 50.")]
        public string Title { get; set; }

        public float Discount { get; set; }
    }
}
