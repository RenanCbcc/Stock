using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.Models.ClientModels
{
    public class Client : Base
    {
        [Required]
        [StringLength(maximumLength: 9, MinimumLength = 50,
        ErrorMessage = "O nome do cliente deve ter no mímino 9 caracteres e no máximo 50.")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(maximumLength: 11, MinimumLength = 11,
        ErrorMessage = "Número de telefone deve ter exatamente 11 caracteres.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        
    }
}
