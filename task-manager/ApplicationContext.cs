using task_manager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace task_manager
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>().HasKey(u => u.Id);

            modelBuilder.Entity<Tarefa>().HasKey(t => t.Id);
            modelBuilder.Entity<Tarefa>().HasOne(t => t.Responsavel);
            modelBuilder.Entity<Tarefa>().HasMany(t => t.HistoricoAtividades).WithOne(a => a.Tarefa);

            modelBuilder.Entity<Atividade>().HasKey(a => a.Id);



        }
    }
}
