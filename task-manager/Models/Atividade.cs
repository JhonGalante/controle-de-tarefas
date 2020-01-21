using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace task_manager.Models
{
    [DataContract]
    public class Atividade : BaseModel
    {
        [Required]
        [DataMember]
        public int Id { get; set; }

        [Required]
        [DataMember]
        public string Descricao { get; set; }

        [Required]
        [DataMember]
        public DateTime dataHoraPublicacao { get; set; }

        [Required]
        [DataMember]
        public Tarefa Tarefa { get; set; }

        public Atividade(string Descricao)
        {
            this.Descricao = Descricao;
            dataHoraPublicacao = DateTime.Now;
        }
    }
}
