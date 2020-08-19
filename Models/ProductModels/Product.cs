using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.Models.ProductModels
{
    public class Product : Base
    {
        [Required(ErrorMessage = "Produto precisa ter um valor.")]
        [DataType(DataType.Currency)]
        public float Price { get; set; }

        [Required(ErrorMessage = "Produto precisa ter uma descrição.")]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 4, MinimumLength = 50,
        ErrorMessage = "A descrição do produto deve ter no mímino 4 caracteres e no máximo 50.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Produto precisa ter um código.")]
        [StringLength(maximumLength: 9, MinimumLength = 13,
        ErrorMessage = "O código do produto deve ter no mímino 9 caracteres e no máximo 13.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Produto precisa ter uma quantidade.")]
        [Range(minimum: 1, maximum: 100, ErrorMessage = "A quantidade deve estar entre 1 e 100.")]
        public int Quantity { get; set; }

        public int CatgoryId { get; set; }
    }
}
