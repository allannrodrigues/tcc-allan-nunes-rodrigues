using Admin.Filters;
using Admin.Resources;
using Admin.Services;
using Admin.ViewModels.Produto;
using ApplicationCore.Entities;
using ApplicationCore.Services;
using Framework.Exceptions;
using Framework.Extenders;
using Framework.Security.Authorization;
using Framework.Security.Permissoes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Admin.Controllers
{
    public class ProdutoController : BaseController
    {
        private ProdutoServiceWeb ProdutoServiceWeb => GetService<ProdutoServiceWeb>();
        private ProdutoService ProdutoService => GetService<ProdutoService>();

        [AuthUsuarioFilter(ProdutoPermissoes.Gerenciar, AuthPermissaoTipoAcao.Permitir)]
        public IActionResult Index(ProdutoFiltroViewModel filtro) => View(ProdutoServiceWeb.Listar(filtro));

        [AuthUsuarioFilter(ProdutoPermissoes.Gerenciar, AuthPermissaoTipoAcao.Permitir)]
        public IActionResult Form(int id) => View(ProdutoServiceWeb.Recuperar(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthUsuarioFilter(ProdutoPermissoes.Gerenciar, AuthPermissaoTipoAcao.Permitir)]
        public async Task<IActionResult> Form(ProdutoViewModel produto)
        {
            try
            {
                if (produto == null || !ModelState.IsValid)
                {
                    ExibirMensagemErro(MensagemResource.ModelStateInvalido);
                    return View(produto);
                }

                if (string.IsNullOrWhiteSpace(produto.ImagemUrl) && produto.Imagem == null)
                {
                    ExibirMensagemErro(MensagemResource.ImagemNaoInformada);
                    return View(produto);
                }

                var model = new ProdutoEntity
                {
                    Id = produto.Id,
                    IdConteudo = produto.IdConteudo ?? throw new InvalidOperationException("IdConteudo NULL"),
                    Titulo = produto.Titulo,
                    Codigo = produto.Codigo,
                    DescricaoLonga = produto.DescricaoLonga,
                    Status = produto.Status ?? throw new InvalidOperationException("Status NULL"),
                    CategoriaProduto = produto.CategoriaProduto ?? ApplicationCore.Enuns.ECategoriaProduto.ProdutoFisico,
                };

                if (produto.Imagem != null)
                {
                    model.Imagem = $"{Guid.NewGuid()}{RecuperarExtensaoDoArquivo(produto.Imagem)}";
                    await ProdutoService.SalvarImagemAsync(produto.Imagem.OpenReadStream(), model.Imagem);
                }

                ProdutoService.Salvar(model);

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

            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthUsuarioFilter(ProdutoPermissoes.Gerenciar, AuthPermissaoTipoAcao.Permitir)]
        public IActionResult Excluir(int id)
        {
            try
            {
                ProdutoService.Excluir(id);

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


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthUsuarioFilter(ProdutoPermissoes.Gerenciar, AuthPermissaoTipoAcao.Permitir)]
        public async Task<IActionResult> EnviarNps(ProdutoNpsViewModel vm)
        {
            try
            {

                if (vm == null || vm?.IdProduto <= 0)
                    return NotFound();

                if (vm.Email == null)
                {
                    ExibirMensagemErro(MensagemResource.EmailNaoInformado);
                    return RedirectToAction(nameof(Form), new { id = vm.IdProduto });
                }

                if (vm.DataLimite.HasValue && vm.DataLimite.Value.Date < DateTime.Now.Date)
                {
                    ExibirMensagemErro(MensagemResource.DataLimiteNpsInvalida);
                    return RedirectToAction(nameof(Form), new { id = vm.IdProduto });
                }

                await ProdutoService.EnviarNps(vm.IdProduto.Value, vm.Email, vm.DataLimite);

                ExibirMensagemSucesso(MensagemResource.Sucesso);
            }
            catch (MensagemException ex)
            {
                ExibirMensagemErro(ex.GetMessages());
            }
            catch (Exception)
            {
                ExibirMensagemErro(MensagemResource.Erro);
            }

            return RedirectToAction(nameof(Form), new { id = vm.IdProduto });
        }
    }
}