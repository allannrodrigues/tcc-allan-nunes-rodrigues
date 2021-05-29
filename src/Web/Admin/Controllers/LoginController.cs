using Admin.Filters;
using Admin.Resources;
using Admin.ViewModels.Login;
using Admin.ViewModels.Usuario;
using ApplicationCore.Services;
using Framework.Exceptions;
using Framework.Extenders;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Admin.Controllers
{
    public class LoginController : BaseController
    {
        private UsuarioService UsuarioService => GetService<UsuarioService>();

        [AuthUsuarioFilter(false)]
        public IActionResult Index() => View();

        [HttpPost]
        [AuthUsuarioFilter(false)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel login)
        {
            try
            {
                ModelState.Clear();

                var model = UsuarioService.Autenticar(login?.Email, login?.Senha);

                await AutenticarUsuarioAsync(new UsuarioAuthViewModel(model));

                return RedirectToAction(nameof(Index), "Home");
            }
            catch (MensagemException ex)
            {
                ExibirMensagemErro(ex);
            }
            catch (Exception)
            {
                ExibirMensagemErro(MensagemResource.Erro);
            }

            return View(login);
        }

        [AuthUsuarioFilter(false)]
        public IActionResult Sair()
        {
            DesautenticarUsuarioAsync();
            return RedirectToAction(nameof(Index), "Login");
        }


        [HttpPost]
        [AuthUsuarioFilter]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlterarSenha(AlterarSenhaViewModel vm)
        {
            try
            {
                ModelState.Clear();

                await UsuarioService.AlterarSenha(Usuario.Identificao.Id, vm?.SenhaAtual, vm?.NovaSenha, vm?.ConfirmarNovaSenha, ConteudoResource.EmailRecuperacao)
                            .ConfigureAwait(true);

                ExibirMensagemSucesso(MensagemResource.SenhaAlteradaSucesso);
            }
            catch (MensagemException ex)
            {
                ExibirMensagemErro(ex);
            }
            catch (Exception)
            {
                ExibirMensagemErro(MensagemResource.Erro);
            }

            if (!string.IsNullOrWhiteSpace(vm?.ReturnUrl))
                return Redirect(vm.ReturnUrl);

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}