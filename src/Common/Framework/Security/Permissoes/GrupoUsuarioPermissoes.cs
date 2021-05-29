using Framework.Security.Authorization;
using System.Collections.Generic;

namespace Framework.Security.Permissoes
{
    public class GrupoUsuarioPermissoes : RegistradorAuthPermissao
    {
        public const string Gerenciar = "{8D047FA4-3F15-43D8-9B53-B2B5F9464232}";

        public static KeyValuePair<string, AuthPermissaoTipoAcao> Permitir => new KeyValuePair<string, AuthPermissaoTipoAcao>("{DC830BDA-9991-4B44-8BE5-A60F99D1E90B}", AuthPermissaoTipoAcao.Permitir);

        protected override string Grupo => NomeGrupo;
        public static string NomeGrupo => "Perfis de usuários";
        public static string Descricao => "Gerenciar Perfis de usuários";
        public override void Init()
        {
            Registrar(Gerenciar, Descricao, false)
                .AddAcao(Permitir.Key, Permitir.Value);
        }
    }
}
