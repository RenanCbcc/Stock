﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Stock_Back_End.Models.ProductModels
{
    public class CreatingProductModel
    {
        [Range(minimum: 0, maximum: 100, ErrorMessage = "O desconto do produto deve estar entre 0 e 100.")]
        public float Discount { get; set; }

        [Required(ErrorMessage = "O produto precisa ter um valor de preço de compra.")]
        [DataType(DataType.Currency)]
        [Range(minimum: 1, maximum: 1000, ErrorMessage = "O valor da compra deve estar entre R$ 1,0 e R$ 1000,0.")]
        public float PurchasePrice { get; set; }

        [Required(ErrorMessage = "O produto precisa ter um valor de preço de venda.")]
        [DataType(DataType.Currency)]
        [Range(minimum: 1, maximum: 1000, ErrorMessage = "O valor da venda deve estar entre R$ 1,0 e R$ 1000,0.")]
        public float SalePrice { get; set; }


        [Required(ErrorMessage = "O produto precisa ter uma descrição.")]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 50, MinimumLength = 4,
        ErrorMessage = "A descrição do produto deve ter no mímino 4 caracteres e no máximo 50.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "O produto precisa ter um código.")]
        [StringLength(maximumLength: 13, MinimumLength = 9,
        ErrorMessage = "O código do produto deve ter no mímino 9 caracteres e no máximo 13.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "O produto precisa ter uma quantidade.")]
        [Range(minimum: 1, maximum: 100, ErrorMessage = "A quantidade deve estar entre 1 e 100.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Produto precisa ter uma quantidade mínima. ")]
        [Range(minimum: 1, maximum: 100, ErrorMessage = "A quantidade deve estar entre 1 e 100.")]
        public int MinimumQuantity { get; set; }

        public int CategoryId { get; set; }

        public int SupplierId { get; set; }
    }
}
