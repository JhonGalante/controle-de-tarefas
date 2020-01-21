using task_manager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace task_manager.Repositories
{
    public interface IUsuarioRepository
    {
        Usuario GetUsuario(string login, string senha);
        Usuario GetUsuarioId(int? id);
        IList<Usuario> GetUsuarios();
    }

    public class UsuarioRepository : BaseRepository<Usuario> , IUsuarioRepository
    {
        public UsuarioRepository(ApplicationContext contexto) : base(contexto)
        {
        }

        public Usuario GetUsuario(string email, string senha)
        {
            return dbSet
                .Where(u => u.Email == email)
                .Where(u => u.Senha == senha)
                .SingleOrDefault();
        }

        public Usuario GetUsuarioId(int? id)
        {
            return dbSet
                .Where(u => u.Id == id)
                .SingleOrDefault();
        }

        public IList<Usuario> GetUsuarios()
        {
            return dbSet.ToList<Usuario>();
        }
    }
}
