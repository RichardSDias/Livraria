using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
//using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvaTecnica.Model
{
    public class Repositorio
    {
        private string _StringConexao;

        public Repositorio(string strConexao)
        {
            this._StringConexao = strConexao;
        }

        public static Repositorio NovaInstancia(string chaveStringConexao = null)
        {
            ///Caso não tenha sido informada nenhuma chave para obter a string de conexão do arquivo
            ///de configuração da aplicação que consome este método, será definido uma chave padrão
            if (String.IsNullOrWhiteSpace(chaveStringConexao))
            {
                chaveStringConexao = "dapper";
            }

            var strConexao = ConfigurationManager.ConnectionStrings[chaveStringConexao]?.ConnectionString;

            if (String.IsNullOrWhiteSpace(strConexao))
            {
                throw new ArgumentNullException($"A string de conexão, identificada pela chave '{chaveStringConexao}', não foi definida do arquivo de configuração da aplicação.");
            }

            return new Repositorio(strConexao);
        }

        public SqlConnection ObterConexao()
        {
            var conexao = new SqlConnection(this._StringConexao);

            conexao.Open();

            return conexao;
        }

        public T Obter<T>(object id) where T : class
        {
            using (var conexao = this.ObterConexao())
            {
                return conexao.Get<T>(id);
            }
        }

        /// <summary>
        /// Método utilzado para Incluir, Atualizar e Excluir itens de forma genérica
        /// </summary>
        /// <param name="nomeProcedure">O nome da procedure utilizada para realizar o procedimento</param>
        /// <param name="parametros">Os parâmetros utilizados</param>
        /// <returns>Retorna true quando houver mais de um registro modificado/inserido</returns>
        public bool ExecutarProcedure(string nomeProcedure, DynamicParameters parametros)
        {
            using (var conexao = this.ObterConexao())
            {
                return conexao.Execute(sql: nomeProcedure,
                                       param: parametros,
                                       commandType: System.Data.CommandType.StoredProcedure) > 0;
            }
        }

        public DynamicParameters NovosParametros()
        {
            return new DynamicParameters();
        }
    }
}