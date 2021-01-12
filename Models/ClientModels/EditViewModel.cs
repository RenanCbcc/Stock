using System.ComponentModel.DataAnnotations;

namespace Stock_Back_End.Models.ClientModels
{
    public class EditViewModel : CreateViewModel
    {
        public int Id { get; set; }

        [Required]
        public Status Status { get; set; }
    }
}
