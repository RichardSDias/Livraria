using ProvaTecnica.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProvaTecnica.Controllers
{
    public class LivroController : Controller
    {
        // GET: Livro
        public ActionResult Index(string nome = null,
                                  string autor = null,
                                  string isbn = null,
                                  string localizacao = null,
                                  int? quantidadeMinima = null,
                                  int? quantidadeMaxima = null)
        {
            var livros = LivroModel.Filtrar(nome: nome,
                                            autor: autor,
                                            isbn: isbn,
                                            quantidadeMinima: quantidadeMinima,
                                            quantidadeMaxima: quantidadeMaxima,
                                            localizacao: localizacao);

            ViewBag.Nome = nome;
            ViewBag.Autor = autor;
            ViewBag.ISBN = isbn;
            ViewBag.Localizacao = localizacao;
            ViewBag.QuantidadeMinima = quantidadeMinima;
            ViewBag.QuantidadeMaxima = quantidadeMaxima;

            return View("Index", livros);
        }

        public ActionResult Incluir()
        {
            return View("Incluir");
        }

        [HttpPost]
        public ActionResult Incluir(LivroModel livro)
        {
            if (ModelState.IsValid)
            {
                var sucesso = livro.Incluir();

                if (sucesso)
                {
                    TempData["Mensagem"] = "O livro foi incluído com sucesso!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Mensagem"] = "Não foi possível realizar a inclusão do livro solicitado. Tente novamente em alguns instantes!";
                }
            }
            else
            {
                ModelState.AddModelError("", "Verifique os campos obrigatórios e preencha-os para continuar");
            }

            return View("Incluir", livro);
        }

        public ActionResult Editar(string id)
        {
            var livro = LivroModel.Obter(id);

            if (livro == null)
            {
                TempData["Mensagem"] = "Não foi possível encontrar o livro solicitado";

                return RedirectToAction("Index");
            }

            return View("Editar", livro);
        }

        [HttpPost]
        public ActionResult Editar(LivroModel livro)
        {
            if (ModelState.IsValid)
            {
                var sucesso = livro.Editar();

                if (sucesso)
                {
                    TempData["Mensagem"] = "O livro foi editado com sucesso!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Mensagem"] = "Não foi possível realizar a edição do livro solicitado. Tente novamente em alguns instantes!";
                }
            }
            else
            {
                ModelState.AddModelError("", "Verifique os campos obrigatórios e preencha-os para continuar");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Excluir(string id)
        {
            var sucesso = LivroModel.Remover(id);

            if (sucesso)
            {
                TempData["Mensagem"] = "Livro excluído com sucesso!";
            }
            else
            {
                TempData["Mensagem"] = "Não foi possível excluir o livro solicitado. por favor tente novamente mais tarde.";
            }

            return RedirectToAction("Index");
        }
    }
}