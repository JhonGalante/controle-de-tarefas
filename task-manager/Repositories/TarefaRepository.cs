using task_manager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace task_manager.Repositories
{
    public interface ITarefaRepository
    {
        IList<Tarefa> GetTodasTarefas();
        IList<Tarefa> GetTarefasPorUsuario(Usuario usuario);
        IList<Tarefa> GetTodasTarefasHistorico();
        IList<Tarefa> GetTarefasPorUsuarioHistorico(Usuario usuario);
        Tarefa GetTarefaPorId(int Id);
        void SetNovaTarefa(Tarefa tarefa);
        void UpdateTarefa(Tarefa tarefa);
        void DeleteTarefa(Tarefa tarefa);
    }

    public class TarefaRepository : BaseRepository<Tarefa> , ITarefaRepository
    {
        public TarefaRepository(ApplicationContext contexto) : base(contexto)
        {
        }

        public Tarefa GetTarefaPorId(int Id)
        {
            return dbSet
                .Include(t => t.Responsavel)
                .Include(t => t.HistoricoAtividades)
                .Where(t => t.Id == Id)
                .SingleOrDefault();
        }

        public IList<Tarefa> GetTarefasPorUsuario(Usuario usuario)
        {
            return dbSet
                .Where(t => t.Responsavel == usuario)
                .Where(t => t.Status != 1)
                .Where(t => t.Status != 2)
                .ToList<Tarefa>();
        }

        public IList<Tarefa> GetTodasTarefas()
        {
            return dbSet
                .Include(t => t.Responsavel)
                .Where(t => t.Status != 1)
                .Where(t => t.Status != 2)
                .ToList<Tarefa>();
        }

        public void SetNovaTarefa(Tarefa tarefa)
        {
            contexto.Add<Tarefa>(tarefa);
            contexto.SaveChanges();
        }

        public void UpdateTarefa(Tarefa tarefa)
        {
            contexto.Update<Tarefa>(tarefa);
            contexto.SaveChanges();
        }

        public void DeleteTarefa(Tarefa tarefa)
        {
            contexto.Remove<Tarefa>(tarefa);
            contexto.SaveChanges();
        }

        public IList<Tarefa> GetTodasTarefasHistorico()
        {
            return dbSet
                .Include(t => t.Responsavel)
                .Where(t => t.Status == 1 || t.Status == 2)
                .ToList<Tarefa>();
        }

        public IList<Tarefa> GetTarefasPorUsuarioHistorico(Usuario usuario)
        {
            return dbSet
                .Where(t => t.Responsavel == usuario)
                .Where(t => t.Status == 1 || t.Status == 2)
                .ToList<Tarefa>();
        }
    }
}
