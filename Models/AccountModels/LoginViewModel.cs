using System.ComponentModel.DataAnnotations;

namespace Stock_Back_End.Models.AccountModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        
    }
}
