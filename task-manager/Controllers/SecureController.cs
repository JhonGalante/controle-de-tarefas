using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using task_manager.Models.ViewModels;
using task_manager.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace controle_de_tarefa_global.Controllers
{
    public class SecureController : Controller
    {
        private readonly IUsuarioRepository usuarioRepository;
        private readonly ITarefaRepository tarefaRepository;
        private readonly IHttpContextAccessor httpContext;

        public SecureController(IUsuarioRepository usuarioRepository, IHttpContextAccessor httpContext, ITarefaRepository tarefaRepository)
        {
            this.usuarioRepository = usuarioRepository;
            this.tarefaRepository = tarefaRepository;
            this.httpContext = httpContext;
        }

        [HttpPost]
        public IActionResult Index(string email, string senha)
        {
            var usuario = usuarioRepository.GetUsuario(email, senha);
            if (usuario != null)
            {
                httpContext.HttpContext.Session.SetInt32("UsuarioLogadoId", usuario.Id);
                httpContext.HttpContext.Session.SetString("UsuarioLogadoNome", usuario.Nome);
                if (usuario.Adm)
                {
                    return View("../Home/Index", new IndexViewModel(tarefaRepository.GetTodasTarefas(), usuarioRepository.GetUsuarios()));
                }
                return View("../Home/Usuario/Index", new IndexViewModel(tarefaRepository.GetTarefasPorUsuario(usuario), usuarioRepository.GetUsuarios()));
            }
            return View("Error");
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}