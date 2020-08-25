using Estoque.Models.AccountModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.Models.ClientModels
{
    public class EditViewModel : CreateViewModel
    {
        public int Id { get; set; }

        [Required]
        public Status Status { get; set; }
    }
}
