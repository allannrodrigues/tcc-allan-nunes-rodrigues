using Framework.Security.Authorization;
using System.Collections.Generic;

namespace Framework.Security.Permissoes
{
    public class UsuarioPermissoes : RegistradorAuthPermissao
    {
        public const string Gerenciar = "{BC158B92-D6BD-4FBB-BAFF-4B4303AF7F19}";

        public static KeyValuePair<string, AuthPermissaoTipoAcao> Permitir => new KeyValuePair<string, AuthPermissaoTipoAcao>("{33B05B6E-3992-4B52-A036-AF90D719DDBC}", AuthPermissaoTipoAcao.Permitir);

        protected override string Grupo => NomeGrupo;
        public static string NomeGrupo => "Usuários";
        public static string Descricao => "Gerenciar Usuários";
        public override void Init()
        {
            Registrar(Gerenciar, Descricao, false)
                .AddAcao(Permitir.Key, Permitir.Value);
        }
    }
}
