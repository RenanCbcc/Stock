using System.ComponentModel.DataAnnotations;

namespace Stock_Back_End.Models.SupplierModels
{
    public class CreatingSupplierModel
    {
        [Required(ErrorMessage = "Fornecedor precisa ter um nome.")]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 50, MinimumLength = 5,
       ErrorMessage = "O nome do fornecedor deve ter no mímino 5 caracteres e no máximo 50.")]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(maximumLength: 50, MinimumLength = 5,
        ErrorMessage = "O nome do fornecedor deve ter no mímino 5 caracteres e no máximo 50.")]
        public string Email { get; set; }


        [Required]
        [StringLength(maximumLength: 11, MinimumLength = 11,
        ErrorMessage = "O número de telefone deve ter exatamente 11 caracteres.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}
