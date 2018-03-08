using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Dapper;
using System.ComponentModel.DataAnnotations;

namespace ProvaTecnica.Model
{
    [Table("dbo.livro")]
    public class LivroModel
    {
        [ExplicitKey]
        public string Id { get; set; }
        [Required(ErrorMessage = "O nome do livro deve ser informado")]
        public string Nome { get; set; }
        public string NomeAutor { get; set; }
        public string ISBN { get; set; }
        [Required(ErrorMessage = "Você deve informar a quantidade disponível para este livro")]
        public int Quantidade { get; set; }
        public string Localizacao { get; set; }

        public bool Incluir()
        {
            var repositorio = Repositorio.NovaInstancia();
            var parametros = repositorio.NovosParametros();

            parametros.Add("@Id", this.Id, System.Data.DbType.String, System.Data.ParameterDirection.Output, size: 36);
            parametros.Add("@Nome", this.Nome, System.Data.DbType.String, size: 100);
            parametros.Add("@Autor", this.NomeAutor, System.Data.DbType.String, size: 100);
            parametros.Add("@ISBN", this.ISBN, System.Data.DbType.String, size: 50);
            parametros.Add("@Quantidade", this.Quantidade, System.Data.DbType.Int32);
            parametros.Add("@Localizacao", this.Localizacao, System.Data.DbType.String, size: 50);

            return repositorio.ExecutarProcedure("[dbo].[PRC_INCLUIR_LIVRO]", parametros);
        }

        public bool Editar()
        {
            var repositorio = Repositorio.NovaInstancia();
            var parametros = repositorio.NovosParametros();

            parametros.Add("@Id", this.Id, System.Data.DbType.String, size: 36);
            parametros.Add("@Nome", this.Nome, System.Data.DbType.String, size: 100);
            parametros.Add("@Autor", this.NomeAutor, System.Data.DbType.String, size: 100);
            parametros.Add("@ISBN", this.ISBN, System.Data.DbType.String, size: 50);
            parametros.Add("@Quantidade", this.Quantidade, System.Data.DbType.Int32);
            parametros.Add("@Localizacao", this.Localizacao, System.Data.DbType.String, size: 50);

            return repositorio.ExecutarProcedure("[dbo].[PRC_ATUALIZAR_LIVRO]", parametros);
        }

        public static bool Remover(string id)
        {
            var repositorio = Repositorio.NovaInstancia();
            var parametros = repositorio.NovosParametros();

            parametros.Add("@Id", id, System.Data.DbType.String);

            return repositorio.ExecutarProcedure("[dbo].[PRC_EXCLUIR_LIVRO]", parametros);
        }

        public static LivroModel Obter(string id)
        {
            var repositorio = Repositorio.NovaInstancia();

            return repositorio.Obter<LivroModel>(id);
        }

        public static List<LivroModel> Filtrar(string nome = null,
                                               string autor = null,
                                               string isbn = null,
                                               int? quantidadeMinima = null,
                                               int? quantidadeMaxima = null,
                                               string localizacao = null)
        {
            //Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            var repositorio = Repositorio.NovaInstancia();
            var parametros = repositorio.NovosParametros();

            parametros.Add("@Nome", nome, System.Data.DbType.String);
            parametros.Add("@Autor", autor, System.Data.DbType.String);
            parametros.Add("@ISBN", isbn, System.Data.DbType.String);
            parametros.Add("@Localizacao", localizacao, System.Data.DbType.String);
            parametros.Add("@QuantidadeMinima", quantidadeMinima, System.Data.DbType.Int32);
            parametros.Add("@QuantidadeMaxima", quantidadeMaxima, System.Data.DbType.Int32);

            using (var conexao = repositorio.ObterConexao())
            {
                return conexao.Query<LivroModel>(sql: "[dbo].[PRC_FILTRAR_LIVROS]",
                                                 param: parametros,
                                                 commandType: System.Data.CommandType.StoredProcedure)?.ToList();
            }
        }
    }
}