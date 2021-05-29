using Admin.Filters;
using Admin.Resources;
using Admin.Services;
using Admin.ViewModels.GrupoUsuario;
using ApplicationCore.Entities;
using ApplicationCore.Services;
using Framework.Exceptions;
using Framework.Security.Authorization;
using Framework.Security.Permissoes;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Admin.Controllers
{
    public class GrupoUsuarioController : BaseController
    {
        private GrupoUsuarioServiceWeb GrupoAcessoServiceWeb => GetService<GrupoUsuarioServiceWeb>();
        private GrupoUsuarioService GrupoUsuarioServico => GetService<GrupoUsuarioService>();

        [AuthUsuarioFilter(GrupoUsuarioPermissoes.Gerenciar, AuthPermissaoTipoAcao.Permitir)]
        public IActionResult Index(GrupoUsuarioFiltroViewModel filtro) => View(GrupoAcessoServiceWeb.Listar(filtro));

        [AuthUsuarioFilter(GrupoUsuarioPermissoes.Gerenciar, AuthPermissaoTipoAcao.Permitir)]
        public IActionResult Form(int id) => View(GrupoAcessoServiceWeb.Recuperar(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthUsuarioFilter(GrupoUsuarioPermissoes.Gerenciar, AuthPermissaoTipoAcao.Permitir)]
        public IActionResult Form(GrupoUsuarioViewModel grupoUsuario)
        {
            try
            {
                if (grupoUsuario == null || !ModelState.IsValid)
                {
                    ExibirMensagemErro(MensagemResource.ModelStateInvalido);
                    return View(grupoUsuario);
                }

                var model = new GrupoUsuarioEntity
                {
                    Id = grupoUsuario.Id,
                    Nome = grupoUsuario.Nome,
                    Status = grupoUsuario.Status
                };

                GrupoUsuarioServico.Salvar(model, grupoUsuario.PermissoesDoGrupo);

                ExibirMensagemSucesso(MensagemResource.Sucesso);

                return RedirectToAction(nameof(Form), new { model.Id });
            }
            catch (MensagemException ex)
            {
                ExibirMensagemErro(ex);
            }
            catch (Exception)
            {
                ExibirMensagemErro(MensagemResource.Erro);
            }

            return RedirectToAction(nameof(Form), new { grupoUsuario?.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthUsuarioFilter(GrupoUsuarioPermissoes.Gerenciar, AuthPermissaoTipoAcao.Permitir)]
        public IActionResult Excluir(int id)
        {
            try
            {
                GrupoUsuarioServico.Excluir(id);

                ExibirMensagemSucesso(MensagemResource.ExcluirSucesso);
            }
            catch (MensagemException ex)
            {
                ExibirMensagemErro(ex);
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(
                    ErroRelacionamento(ex)
                    ? MensagemResource.ErroRelacionamento
                    : MensagemResource.Erro);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}