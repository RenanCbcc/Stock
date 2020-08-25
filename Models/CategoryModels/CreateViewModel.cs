using System;
using System.ComponentModel.DataAnnotations;

namespace Estoque.Models.CategoryModels
{
    public class CreateViewModel
    {
        public CreateViewModel()
        {
            Discount = 0;
        }

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 5,
        ErrorMessage = "O título da categoria deve ter no mímino 5 caracteres e no máximo 50.")]
        public string Title { get; set; }

        [Range(minimum: 0, maximum: 100, ErrorMessage = "O desconto da categoria deve estar entre 0 e 100.")]
        public float Discount { get; set; }
    }
}
