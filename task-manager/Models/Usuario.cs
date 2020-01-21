using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace task_manager.Models
{
    [DataContract]
    public class Usuario : BaseModel
    {
        [Required]
        [DataMember]
        public int Id { get; set; }
        [Required]
        [DataMember]
        public string Senha { get; set; }

        [Required]
        [DataMember]
        public string Email { get; set; }

        [Required]
        [DataMember]
        public string Nome { get; set; }

        [Required]
        [DataMember]
        public Boolean Adm { get; set; }

        public Usuario()
        {
            Adm = false;
        }
    }
}
