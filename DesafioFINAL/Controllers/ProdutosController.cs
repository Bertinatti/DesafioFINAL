using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DesafioFINAL;
using DesafioFINAL.Data;
using Microsoft.AspNetCore.Authorization;
using DesafioFINAL.Models;
using System.Security;
using System.Security.Permissions;

namespace DesafioFINAL.Controllers
{
    /// <summary>
    /// Acesso restrito aos usuários logados.
    /// </summary>
    [Authorize]
    public class ProdutosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProdutosController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Método da controller de Produtos que captura os dados do banco e monta a view com os dados.
        /// </summary>
        /// <returns>Retorna a view Inicial dos Produtos com os dados capturados do banco.</returns>
        public async Task<IActionResult> Index()
        {
            return _context.Produto != null ? 
                        View(await _context.Produto.ToListAsync()) :
                        Problem("A entidade 'ApplicationDbContext.Produto'  está vazia.");
        }

        /// <summary>
        /// Método da controller de Produtos que exibe a view Details.
        /// </summary>
        /// <param name="id">Id do Produto, de tipo INT, capturado pelo clique do botão e enviado através de asp-route.</param>
        /// <returns>Retorna a view com os Detalhes desse Produto.</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Produto == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .FirstOrDefaultAsync(m => m.IdProduto == id);
            if (produto == null)
            {
                return NotFound();
            }

            // Registro do log quando um usuário exibir os detalhes de determinado produto
            _context.LogProdutos.Add(new LogProdutos
            {
                EmailUsuario = User.Identity.Name,
                IdProduto = produto.IdProduto,
                AcaoLog = String.Concat("Usuário exibiu o produto: ", produto.Nome.ToUpper()),
                DataLog = DateTime.Now,

            });
            _context.SaveChanges();

            return View(produto);
        }

        /// <summary>
        /// Método da controller de Produtos que exibe a view Create.
        /// </summary>
        /// <returns>Retorna a view de Criação de um novo Produto.</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Método da controller de Produtos para efetuar o Post com os dados do novo Produto que será salvo no banco.
        /// </summary>
        /// <param name="produto">Objeto da Classe Produto que será salvo no banco.</param>
        /// <returns>Retorna para a Index, em caso de sucesso, ou exibe quais campos estão errados, permanecendo na tela de Criação.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProduto,CodigoEAN,Nome,Preco,Estoque,Fornecedor,CNPJFornecedor")] Produto produto)
        {
            // Captura um valor booleano para caso exista um Produto que possua código de barras igual ao valor digitado e com o id diferente do produto que se está criando
            bool CodigoEANExistente = _context.Produto.Any(x => x.CodigoEAN == produto.CodigoEAN && x.IdProduto != produto.IdProduto);

            // Verifica se o código de barras digitado é um valor repetido
            if(CodigoEANExistente == true)
            {
                ModelState.AddModelError("CodigoEAN", "Código de barras já existente.");
            }

            if (ModelState.IsValid)
            {               
                _context.Add(produto);
                await _context.SaveChangesAsync();

                // Registro do log quando um usuário criar/gravar um novo produto
                _context.LogProdutos.Add(new LogProdutos
                {
                    EmailUsuario = User.Identity.Name,
                    IdProduto = produto.IdProduto,
                    AcaoLog = String.Concat("Usuário cadastrou o produto: ", produto.Nome.ToUpper()),
                    DataLog = DateTime.Now,

                });
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        /// <summary>
        /// Método da controller de Produtos que exibe a view Edit.
        /// </summary>
        /// <param name="id">Id do Produto, de tipo INT, capturado pelo clique do botão e enviado através de asp-route.</param>
        /// <returns>Retorna a view de Edição com os dados salvos do Produto.</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Produto == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            if (produto.Nome == "PRODUTO DELETADO")
            {
                return RedirectToAction(nameof(Index));
            }

            return View(produto);
        }

        /// <summary>
        /// Método da controller de Produtos para efetuar o Post com os dados editados do Produto que será atualizado.
        /// </summary>
        /// <param name="id">Id do Produto, de tipo INT, que foi capturado no método acima durante o redirecionamento para essa view, através do clique de um botão e por asp-route.</param>
        /// <param name="produto">Objeto da Classe Produto que terá seus dados atualizados no banco.</param>
        /// <returns>Retorna para a Index, em caso de sucesso, ou exibe quais campos estão errados, permanecendo na tela de Edição.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProduto,CodigoEAN,Nome,Preco,Estoque,Fornecedor,CNPJFornecedor")] Produto produto)
        {
            if (id != produto.IdProduto)
            {
                return NotFound();
            }
            // Captura um valor booleano para caso exista um Produto que possua código de barras igual ao valor digitado e com o id diferente do produto que se está editando
            bool CodigoEANExistente = _context.Produto.Any(x => x.CodigoEAN == produto.CodigoEAN && x.IdProduto != produto.IdProduto);

            // Verifica se o código de barras digitado é um valor repetido
            if (CodigoEANExistente == true)
            {
                ModelState.AddModelError("CodigoEAN", "Código de barras já existente.");
            }
        
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();

                    // Registro do log quando o usuário editar um produto
                    _context.LogProdutos.Add(new LogProdutos
                    {
                        EmailUsuario = User.Identity.Name,
                        IdProduto = produto.IdProduto,
                        AcaoLog = String.Concat("Usuário editou o produto: ", produto.Nome.ToUpper()),
                        DataLog = DateTime.Now,

                    });
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.IdProduto))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        /// <summary>
        /// Método da controller de Produtos que exibe a view Delete
        /// </summary>
        /// <param name="id">Id do Produto, de tipo INT, capturado pelo clique do botão e enviado através de asp-route.</param>
        /// <returns>Retorna a view de Deleção com os dados salvos do Produto.</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Produto == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .FirstOrDefaultAsync(m => m.IdProduto == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        /// <summary>
        /// Método da controller de Produtos para efetuar o Post com os dados do Produto que será deletado.
        /// </summary>
        /// <param name="id">Id do Produto, de tipo INT, que foi capturado no método acima durante o redirecionamento para essa view, através do clique de um botão e por asp-route.</param>
        /// <returns>Retorna para a Index, em caso de sucesso, ou exibe os erros da Deleção.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Produto == null)
            {
                return Problem("A entidade 'ApplicationDbContext.Produto'  está vazia.");
            }

            var produto = await _context.Produto.FindAsync(id);
            var produtoTMP = produto;
            var produtoNULO = _context.Produto.FirstOrDefault(x => x.Nome == "PRODUTO DELETADO");

            // Checagem de segurança, para o usuário não deletar o produto entidade PRODUTO DELETADO
            if(produto.Nome == "PRODUTO DELETADO")
            {
                return RedirectToAction(nameof(Index));
            }

            if (produto != null)
            {
                var listalogProdutos = await _context.LogProdutos.ToListAsync();

                // Realiza a substituição do id produto na tabela de log, substituindo o valor da chave estrangeira para o da entidade PRODUTO DELETADO
                foreach (var log in listalogProdutos)
                {
                    // Todos os logs que tiverem id de produtos deletados serão atualizados para o id de um produto entidade, para que não fiquem nulos 
                    if(log.IdProduto == produto.IdProduto)
                    {
                        log.IdProduto = produtoNULO.IdProduto;
                        _context.SaveChanges();
                    }
                }
         
                _context.Produto.Remove(produto);
            }

            await _context.SaveChangesAsync();

            // Registro do log quando o usuário deletar o produto
            _context.LogProdutos.Add(new LogProdutos
            {
                EmailUsuario = User.Identity.Name,
                IdProduto = produtoNULO.IdProduto,
                AcaoLog = String.Concat("Usuário deletou o produto: ", produtoTMP.Nome.ToUpper()), // Nome do produto capturado por um objeto temporário de igual valor ao produto que foi excluído
                DataLog = DateTime.Now,

            });

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Método privado da controller de Produtos para verificar se o Produto existe no banco.
        /// </summary>
        /// <param name="id">Id do Produtos, de tipo INT, passado durante a chamada do método.</param>
        /// <returns>Retorna um valor booleano, indicando se o produto existe ou não.</returns>
        private bool ProdutoExists(int id)
        {
          return (_context.Produto?.Any(e => e.IdProduto == id)).GetValueOrDefault();
        }
    }
}
