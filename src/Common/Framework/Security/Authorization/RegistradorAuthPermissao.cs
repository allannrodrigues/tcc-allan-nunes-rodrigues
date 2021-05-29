using System;
using System.Linq;

namespace Framework.Security.Authorization
{
    public abstract class RegistradorAuthPermissao
    {
        protected abstract string Grupo { get; }

        protected AuthPermissao Registrar(string id, string descricao, bool permiteAlterar)
        {
            var idFuncionalidade = new Guid(id);

            if (AuthPermissao.Todas != null)
            {
                var funcionalidade = AuthPermissao.Todas.FirstOrDefault(f => f.Id == idFuncionalidade);
                if (funcionalidade != null)
                    return funcionalidade;
            }

            return new AuthPermissao(idFuncionalidade, Grupo, descricao, permiteAlterar);
        }

        public abstract void Init();
    }
}