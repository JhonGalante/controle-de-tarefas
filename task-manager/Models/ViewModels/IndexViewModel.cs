using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace task_manager.Models.ViewModels
{
    public class IndexViewModel
    {
        public IList<Tarefa> TarefasUsuario { get; set; }
        public IList<Usuario> Responsaveis { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public int ResponsavelId { get; set; }
        public IList<Atividade> HistoricoAtividades { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime Prazo { get; set; }
        public int NivelUrgencia { get; set; }

        public IndexViewModel(IList<Tarefa> tarefas, IList<Usuario> responsaveis, IList<Atividade> atividades)
        {
            this.TarefasUsuario = tarefas;
            this.Responsaveis = responsaveis;
            this.HistoricoAtividades = atividades;
        }

        public IndexViewModel(IList<Tarefa> tarefas, IList<Usuario> responsaveis)
        {
            this.TarefasUsuario = tarefas;
            this.Responsaveis = responsaveis;
            this.HistoricoAtividades = new List<Atividade>();
        }

        public IndexViewModel() {
            this.HistoricoAtividades = new List<Atividade>();
        }
    }
}
