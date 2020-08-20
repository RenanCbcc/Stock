using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Estoque.Models
{
    [DataContract]
    public abstract class Base
    {
        [DataMember]
        public int Id { get; set; }
    }
}
