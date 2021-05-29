using Framework.Security.Authorization;
using System.Collections.Generic;

namespace Framework.Security.Permissoes
{
    public class ProdutoPermissoes : RegistradorAuthPermissao
    {
        public const string Gerenciar = "{55BDC6F8-5EF1-4C69-9DF6-45D1993A0D7D}";

        public static KeyValuePair<string, AuthPermissaoTipoAcao> Permitir => new KeyValuePair<string, AuthPermissaoTipoAcao>("{1B95D655-7B2E-4C48-A84D-BD5F1470CEF6}", AuthPermissaoTipoAcao.Permitir);

        protected override string Grupo => NomeGrupo;
        public static string NomeGrupo => "Produtos/Serviços";
        public static string Descricao => "Gerenciar Produtos/Serviços";
        public override void Init()
        {
            Registrar(Gerenciar, Descricao, true)
                .AddAcao(Permitir.Key, Permitir.Value);
        }
    }
}
