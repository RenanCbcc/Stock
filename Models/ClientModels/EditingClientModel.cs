using System.ComponentModel.DataAnnotations;

namespace Stock_Back_End.Models.ClientModels
{
    public class EditingClientModel : CreatingClientModel
    {
        public int Id { get; set; }

        [Required]
        public Status Status { get; set; }
    }
}
