using System;
using System.ComponentModel.DataAnnotations;

namespace Stock_Back_End.Models.CategoryModels
{
    public class Category : Base
    {
        public Category()
        {
            Discount = 0;           
        }

        [Required]
        [StringLength(maximumLength: 25, MinimumLength = 5,
        ErrorMessage = "O título da categoria deve ter no mímino 5 caracteres e no máximo 25.")]
        public string Title { get; set; }

        [Range(minimum: 0, maximum: 100, ErrorMessage = "O desconto da categoria deve estar entre 0 e 100.")]
        public float Discount { get; set; }

        
    }
}
