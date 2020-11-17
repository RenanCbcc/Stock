using System.ComponentModel.DataAnnotations;

namespace Estoque.Models.ClientModels
{
    public class EditViewModel : CreateViewModel
    {
        public int Id { get; set; }

        [Required]
        public Status Status { get; set; }
    }
}
