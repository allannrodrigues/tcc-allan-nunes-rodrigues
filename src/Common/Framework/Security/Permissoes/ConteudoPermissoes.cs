using Framework.Security.Authorization;
using System.Collections.Generic;

namespace Framework.Security.Permissoes
{
    public class ConteudoPermissoes : RegistradorAuthPermissao
    {
        public const string Gerenciar = "{7F5D3EFD-2C3D-44B3-B371-A442F8E5595C}";

        public static KeyValuePair<string, AuthPermissaoTipoAcao> Permitir => new KeyValuePair<string, AuthPermissaoTipoAcao>("{16505584-AC70-441B-9317-B8555AC8CF62}", AuthPermissaoTipoAcao.Permitir);

        protected override string Grupo => NomeGrupo;
        public static string NomeGrupo => "Conteúdos";
        public static string Descricao => "Gerenciar Conteúdos";
        public override void Init()
        {
            Registrar(Gerenciar, Descricao, true)
                .AddAcao(Permitir.Key, Permitir.Value);
        }
    }
}
