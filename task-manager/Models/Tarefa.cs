using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace task_manager.Models
{
    [DataContract]
    public class Tarefa : BaseModel
    {
        [Required]
        [DataMember]
        public int Id { get; set; }

        [Required]
        [DataMember]
        public string Titulo { get; set; }

        [DataMember]
        public string Descricao { get; set; }

        [Required]
        [DataMember]
        public Usuario Responsavel { get; set; }

        [DataMember]
        public IList<Atividade> HistoricoAtividades { get; set; }

        [DataMember]
        public DateTime Prazo { get; set; }

        [DataMember]
        public DateTime DataFinalizacao { get; set; }

        [DataMember]
        public DateTime DataCancelamento { get; set; }

        [Required]
        [DataMember]
        public int NivelUrgencia { get; set; }

        [Required]
        [DataMember]
        public int Status { get; set; } // 0 = Ativa ~ 1 = Concluida ~ 2 = Cancelada

        public Tarefa()
        {
            NivelUrgencia = 0;
        }

        public Tarefa(string titulo, string descricao, Usuario responsavel, DateTime prazo, int nivelUrgencia)
        {
            Titulo = titulo;
            Descricao = descricao;
            Responsavel = responsavel;
            Prazo = prazo;
            NivelUrgencia = nivelUrgencia;
            Status = 0;
            this.HistoricoAtividades = new List<Atividade>();
        }
    }
}
