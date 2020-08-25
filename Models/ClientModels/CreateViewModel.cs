using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.Models.ClientModels
{
    public class CreateViewModel
    {
        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 10,
        ErrorMessage = "O nome do cliente deve ter no mímino 9 caracteres e no máximo 50.")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required]

        [StringLength(maximumLength: 100, MinimumLength = 10,
        ErrorMessage = "O endereço do cliente deve ter no mímino 10 caracteres e no máximo 100.")]
        [DataType(DataType.Text)]
        public string Address { get; set; }

        [Required]
        [StringLength(maximumLength: 11, MinimumLength = 11,
        ErrorMessage = "O número de telefone deve ter exatamente 11 caracteres.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

    }
}
