using task_manager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace task_manager.Repositories
{
    public interface IAtividadeRepository
    {
        public Atividade GetAtividadePorId(int Id);
        public void DeleteAtividade(Atividade atividade);
    }

    public class AtividadeRepository : BaseRepository<Atividade> , IAtividadeRepository
    {
        public AtividadeRepository(ApplicationContext contexto) : base(contexto)
        {
        }

        public Atividade GetAtividadePorId(int Id)
        {
            return dbSet
                .Include(a => a.Tarefa)
                .Where(a => a.Id == Id)
                .SingleOrDefault();
        }

        public void DeleteAtividade(Atividade Atividade)
        {
            contexto.Remove<Atividade>(Atividade);
            contexto.SaveChanges();
        }


    }
}
