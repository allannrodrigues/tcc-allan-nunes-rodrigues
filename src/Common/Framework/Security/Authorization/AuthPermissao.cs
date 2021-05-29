using System;
using System.Collections.Generic;
using System.Linq;


namespace Framework.Security.Authorization
{
    public class AuthPermissao
    {
        public static List<AuthPermissao> Todas;

        public AuthPermissao(Guid id, string grupo, string descricao, bool permiteAlterar)
        {
            Id = id;
            Grupo = grupo;
            Descricao = descricao;
            PermiteAlterar = permiteAlterar;
            Acoes = new List<AuthPermissaoAcao>();

            if (Todas == null)
                Todas = new List<AuthPermissao>();

            Todas.Add(this);
        }

        public Guid Id { get; private set; }
        public string Grupo { get; }
        public string Descricao { get; private set; }
        public bool PermiteAlterar { get; private set; }

        public IEnumerable<AuthPermissaoAcao> Acoes { get; }


        public AuthPermissao AddAcao(string idFuncionalidadeAcao, AuthPermissaoTipoAcao acao)
        {
            var idGuidFuncionalidadeAcao = new Guid(idFuncionalidadeAcao);

            if (Acoes.Any(a => a.Acao == acao))
                throw new Exception("Ação já registrada para essa funcionalidade");

            if (Todas.Any(f => f.Acoes.Any(a => a.Id == idGuidFuncionalidadeAcao)))
                throw new Exception("Já existe uma Funcionalidade-Ação com o ID: " + idFuncionalidadeAcao);

            var funcionalidadeAcao = new AuthPermissaoAcao(idGuidFuncionalidadeAcao, this, acao);

            ((List<AuthPermissaoAcao>)Acoes).Add(funcionalidadeAcao);

            return this;
        }

        public static List<AuthPermissao> Listar() => Todas.ToList();
    }
}