using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.Models.SupplierModels
{
    public class Supplier : Base
    {
        [Required(ErrorMessage = "Fornecedor precisa ter um nome.")]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 4, MinimumLength = 50,
        ErrorMessage = "O nome do fornecedor deve ter no mímino 4 caracteres e no máximo 50.")]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 11, MinimumLength = 11,
        ErrorMessage = "O número de telefone deve ter exatamente 11 caracteres.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}
