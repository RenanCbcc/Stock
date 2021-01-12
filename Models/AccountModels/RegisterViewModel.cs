using System.ComponentModel.DataAnnotations;

namespace Stock_Back_End.Models.AccountModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 10,
        ErrorMessage = "Nome deve ter entre 50 e 10 caracteres.")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password",
            ErrorMessage = "Senha e confirmação da senha precisam coincidir.")]
        public string ConfirmPassword { get; set; }
    }
}
