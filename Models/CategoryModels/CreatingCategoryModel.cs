using System;
using System.ComponentModel.DataAnnotations;

namespace Stock_Back_End.Models.CategoryModels
{
    public class CreatingCategoryModel
    {
        public CreatingCategoryModel()
        {
            Discount = 0;
        }

        [Required(ErrorMessage ="Categoria deve ter um título.")]
        [StringLength(maximumLength: 50, MinimumLength = 5,
        ErrorMessage = "O título da categoria deve ter no mímino 5 caracteres e no máximo 50.")]
        public string Title { get; set; }

        [Range(minimum: 0, maximum: 1, ErrorMessage = "O desconto da categoria deve estar entre 0 e 1.")]
        public float Discount { get; set; }
    }
}
