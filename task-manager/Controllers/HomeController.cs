using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using task_manager.Models;
using task_manager.Repositories;
using Microsoft.AspNetCore.Http;
using task_manager.Models.ViewModels;
using System.Net.Mail;

namespace task_manager.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor httpContext;
        private readonly ITarefaRepository tarefaRepository;
        private readonly IUsuarioRepository usuarioRepository;
        private readonly IAtividadeRepository atividadeRepository;
        private readonly Usuario usuarioLogado;

        public HomeController(IHttpContextAccessor httpContext, ITarefaRepository tarefaRepository, IUsuarioRepository usuarioRepository, IAtividadeRepository atividadeRepository)
        {
            this.httpContext = httpContext;
            this.tarefaRepository = tarefaRepository;
            this.usuarioRepository = usuarioRepository;
            this.atividadeRepository = atividadeRepository;
            usuarioLogado = usuarioRepository.GetUsuarioId(httpContext.HttpContext.Session.GetInt32("UsuarioLogadoId"));
        }

        public IActionResult Index()
        {
            if (usuarioLogado == null) return RedirectToAction("Login", "Secure");
            if (usuarioLogado.Adm)
            {
                return View("../Home/Index", new IndexViewModel(tarefaRepository.GetTodasTarefas(), usuarioRepository.GetUsuarios()));
            }
            return View("../Home/Usuario/Index", new IndexViewModel(tarefaRepository.GetTarefasPorUsuario(usuarioLogado), usuarioRepository.GetUsuarios()));
        }

        public IActionResult Historico()
        {
            if (usuarioLogado == null) return RedirectToAction("Login", "Secure");
            if (usuarioLogado.Adm)
            {
                return View(new IndexViewModel(tarefaRepository.GetTodasTarefasHistorico(), usuarioRepository.GetUsuarios()));
            }
            return View(new IndexViewModel(tarefaRepository.GetTarefasPorUsuarioHistorico(usuarioLogado), usuarioRepository.GetUsuarios()));
        }

        public IActionResult Historico2()
        {
            if (usuarioLogado == null) return RedirectToAction("Login", "Secure");
            if (usuarioLogado.Adm)
            {
                return View("../Home/Usuario/Historico", new IndexViewModel(tarefaRepository.GetTodasTarefasHistorico(), usuarioRepository.GetUsuarios()));
            }
            return View("../Home/Usuario/Historico", new IndexViewModel(tarefaRepository.GetTarefasPorUsuarioHistorico(usuarioLogado), usuarioRepository.GetUsuarios()));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SalvarTarefa(string Titulo, string Descricao, int ResponsavelId, string Prazo, int NivelUrgencia)
        {
            var tarefaSalva = new Tarefa(Titulo, Descricao, usuarioRepository.GetUsuarioId(ResponsavelId), DateTime.Parse(Prazo), NivelUrgencia);
            tarefaSalva.HistoricoAtividades = new List<Atividade>();
            tarefaRepository.SetNovaTarefa(tarefaSalva);
            sendMailCriacaoTarefa(tarefaSalva);
            if (usuarioLogado.Adm)
            {
                return PartialView("_TabelaTarefas", tarefaRepository.GetTodasTarefas());
            }
            return PartialView("_TabelaTarefas", tarefaRepository.GetTarefasPorUsuario(usuarioLogado));
        }

        [HttpPost]
        public IActionResult AtualizarTarefa(int Id, string Titulo, string Descricao, int ResponsavelId, string Prazo, int NivelUrgencia)
        {
            var tarefaSalva = tarefaRepository.GetTarefaPorId(Id);
            tarefaSalva.Titulo = Titulo;
            tarefaSalva.Descricao = Descricao;
            tarefaSalva.Responsavel = usuarioRepository.GetUsuarioId(ResponsavelId);
            tarefaSalva.Prazo = DateTime.Parse(Prazo);
            tarefaSalva.NivelUrgencia = NivelUrgencia;
            tarefaRepository.UpdateTarefa(tarefaSalva);

            if (usuarioLogado.Adm)
            {
                return PartialView("_TabelaTarefas", tarefaRepository.GetTodasTarefas());
            }
            return PartialView("_TabelaTarefas", tarefaRepository.GetTarefasPorUsuario(usuarioLogado));
        }

        [HttpPost]
        public IActionResult FinalizarTarefa(int Id)
        {
            var tarefaFinalizada = tarefaRepository.GetTarefaPorId(Id);
            tarefaFinalizada.Status = 1;
            tarefaFinalizada.DataFinalizacao = DateTime.Now;
            tarefaRepository.UpdateTarefa(tarefaFinalizada);
            sendMailFinalizacaoTarefa(tarefaFinalizada);

            if (usuarioLogado.Adm)
            {
                return PartialView("_TabelaTarefas", tarefaRepository.GetTodasTarefas());
            }
            return PartialView("_TabelaTarefas", tarefaRepository.GetTarefasPorUsuario(usuarioLogado));
        }

        [HttpPost]
        public IActionResult CancelarTarefa(int Id)
        {
            var tarefaCancelada = tarefaRepository.GetTarefaPorId(Id);
            tarefaCancelada.Status = 2;
            tarefaCancelada.DataCancelamento = DateTime.Now;
            tarefaRepository.UpdateTarefa(tarefaCancelada);
            sendMailCancelamentoTarefa(tarefaCancelada);
            if (usuarioLogado.Adm)
            {
                return PartialView("_TabelaTarefas", tarefaRepository.GetTodasTarefas());
            }
            return PartialView("_TabelaTarefas", tarefaRepository.GetTarefasPorUsuario(usuarioLogado));
        }

        [HttpPost]
        public IActionResult DeletarTarefa(int Id)
        {
            var tarefaDeletada = tarefaRepository.GetTarefaPorId(Id);

            tarefaRepository.DeleteTarefa(tarefaDeletada);

            if (usuarioLogado.Adm)
            {
                return PartialView("_TabelaTarefas", tarefaRepository.GetTodasTarefas());
            }
            return PartialView("_TabelaTarefas", tarefaRepository.GetTarefasPorUsuario(usuarioLogado));
        }

        [HttpPost]
        public IActionResult EnviarAtividade(int IdTarefa, string Descricao)
        {
            var atividadeEnviada = new Atividade(Descricao);
            var tarefaSelecionada = tarefaRepository.GetTarefaPorId(IdTarefa);
            tarefaSelecionada.HistoricoAtividades.Add(atividadeEnviada);
            tarefaRepository.UpdateTarefa(tarefaSelecionada);

            return PartialView("_ListaAtividades", tarefaSelecionada.HistoricoAtividades);

        }

        [HttpPost]
        public IActionResult CarregarAtividades(int IdTarefa)
        {
            var tarefaSelecionada = tarefaRepository.GetTarefaPorId(IdTarefa);
            return PartialView("_ListaAtividades", tarefaSelecionada.HistoricoAtividades);

        }

        [HttpPost]
        public IActionResult CarregarAtividadesHistorico(int IdTarefa)
        {
            var tarefaSelecionada = tarefaRepository.GetTarefaPorId(IdTarefa);
            return PartialView("_ListaAtividadesHistorico", tarefaSelecionada.HistoricoAtividades);

        }

        [HttpPost]
        public IActionResult DeletarAtividade(int IdTarefa, int IdAtividade)
        {
            var atividadeDeletada = atividadeRepository.GetAtividadePorId(IdAtividade);
            atividadeRepository.DeleteAtividade(atividadeDeletada);
            return PartialView("_ListaAtividades", tarefaRepository.GetTarefaPorId(IdTarefa).HistoricoAtividades);

        }


        protected void sendMailCriacaoTarefa(Tarefa tarefa)
        {
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Host = "email-ssl.com.br";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("ti@globalcorps.com.br", "GCSti22.");
            MailMessage mail = new MailMessage();
            mail.Sender = new MailAddress("ti@globalcorps.com.br", "GlobalCorps TI");
            mail.From = new MailAddress("ti@globalcorps.com.br", "GlobalCorps TI");
            mail.To.Add(new MailAddress(tarefa.Responsavel.Email, tarefa.Responsavel.Nome));
            mail.Subject = "NÃO RESPONDA - CONTROLE DE TAREFAS GLOBAL CORPORATE SOLUTIONS";
            mail.Body =
                "<h3>" + tarefa.Responsavel.Nome + ", uma nova tarefa foi atribuida a você!</h3>" +
                "<b>Titulo:</b> " + tarefa.Titulo +
                "<br/>" +
                "<b>Descricao:</b> " + tarefa.Descricao + "" +
                "<br/> " +
                "<b>Nivel Urgencia:</b> " + retornarNivelUrgencia(tarefa.NivelUrgencia) + "" +
                "<br/> " +
                "<b>Prazo:</b> " + tarefa.Prazo.Day + "/" + tarefa.Prazo.Month + "/" + tarefa.Prazo.Year +
                "<br/><br/>" +
                "<h4>Acesse o sistema de controle de tarefas para mais informações!</h4>";
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            try
            {
                client.Send(mail);
            }
            catch (System.Exception erro)
            {
                Console.WriteLine(erro);
            }
            finally
            {
                mail = null;
            }
        }


        protected void sendMailFinalizacaoTarefa(Tarefa tarefa)
        {
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Host = "email-ssl.com.br";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("ti@globalcorps.com.br", "GCSti22.");
            MailMessage mail = new MailMessage();
            mail.Sender = new MailAddress("ti@globalcorps.com.br", "GlobalCorps TI");
            mail.From = new MailAddress("ti@globalcorps.com.br", "GlobalCorps TI");
            mail.To.Add(new MailAddress("jhonata.galante@gmail.com", "Jhonata TI"));
            mail.To.Add(new MailAddress(tarefa.Responsavel.Email, tarefa.Responsavel.Nome));
            mail.Subject = "NÃO RESPONDA - CONTROLE DE TAREFAS GLOBAL CORPORATE SOLUTIONS";
            mail.Body =
                "<h3>A tarefa foi finalizada com sucesso!</h3>" +
                "<b>Titulo:</b> " + tarefa.Titulo +
                "<br/>" +
                "<b>Descricao:</b> " + tarefa.Descricao + "" +
                "<br/> " +
                "<b>Nivel Urgencia:</b> " + retornarNivelUrgencia(tarefa.NivelUrgencia) + "" +
                "<br/> " +
                "<b>Prazo:</b> " + tarefa.Prazo.Day + "/" + tarefa.Prazo.Month + "/" + tarefa.Prazo.Year +
                "<br/>" +
                "<b>Finalizado em:</b> " + tarefa.DataFinalizacao.Day + "/" + tarefa.DataFinalizacao.Month + "/" + tarefa.DataFinalizacao.Year +
                "<br/><br/>" +
                "<h4>Acesse o sistema de controle de tarefas para mais informações!</h4>";
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            try
            {
                client.Send(mail);
            }
            catch (System.Exception erro)
            {
                Console.WriteLine(erro);
            }
            finally
            {
                mail = null;
            }
        }

        protected void sendMailCancelamentoTarefa(Tarefa tarefa)
        {
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Host = "email-ssl.com.br";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("ti@globalcorps.com.br", "GCSti22.");
            MailMessage mail = new MailMessage();
            mail.Sender = new MailAddress("ti@globalcorps.com.br", "GlobalCorps TI");
            mail.From = new MailAddress("ti@globalcorps.com.br", "GlobalCorps TI");
            mail.To.Add(new MailAddress(tarefa.Responsavel.Email, tarefa.Responsavel.Nome));
            mail.Subject = "NÃO RESPONDA - CONTROLE DE TAREFAS GLOBAL CORPORATE SOLUTIONS";
            mail.Body =
                "<h3>A tarefa foi cancelada com sucesso!</h3>" +
                "<b>Titulo:</b> " + tarefa.Titulo +
                "<br/>" +
                "<b>Descricao:</b> " + tarefa.Descricao + "" +
                "<br/> " +
                "<b>Nivel Urgencia:</b> " + retornarNivelUrgencia(tarefa.NivelUrgencia) + "" +
                "<br/> " +
                "<b>Prazo:</b> " + tarefa.Prazo.Day + "/" + tarefa.Prazo.Month + "/" + tarefa.Prazo.Year +
                "<br/>" +
                "<b>Cancelado em:</b> " + tarefa.DataCancelamento.Day + "/" + tarefa.DataCancelamento.Month + "/" + tarefa.DataCancelamento.Year +
                "<br/><br/>" +
                "<h4>Acesse o sistema de controle de tarefas para mais informações!</h4>";
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            try
            {
                client.Send(mail);
            }
            catch (System.Exception erro)
            {
                Console.WriteLine(erro);
            }
            finally
            {
                mail = null;
            }
        }

        private string retornarNivelUrgencia(int nivelUrgencia)
        {
            switch (nivelUrgencia)
            {
                case 0:
                    return "Baixo";
                case 1:
                    return "Medio";
                case 2:
                    return "Alto";
                default:
                    return "Desconhecido";
            }
        }
    }
}
