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
    [Authorize]
    public class ProdutosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProdutosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Produtos
        public async Task<IActionResult> Index()
        {
            return _context.Produto != null ? 
                        View(await _context.Produto.ToListAsync()) :
                        Problem("A entidade 'ApplicationDbContext.Produto'  está vazia.");
        }

        // GET: Produtos/Details/5
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

            return View(produto);
        }

        // GET: Produtos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produtos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProduto,CodigoEAN,Nome,Preco,Estoque,Fornecedor,CNPJFornecedor")] Produto produto)
        {
            bool CodigoEANExistente = _context.Produto.Any(x => x.CodigoEAN == produto.CodigoEAN && x.IdProduto != produto.IdProduto);

            if(CodigoEANExistente == true)
            {
                ModelState.AddModelError("CodigoEAN", "Código de barras já existente.");
            }

            if (ModelState.IsValid)
            {               
                _context.Add(produto);
                await _context.SaveChangesAsync();
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

        // GET: Produtos/Edit/5
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

        // POST: Produtos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProduto,CodigoEAN,Nome,Preco,Estoque,Fornecedor,CNPJFornecedor")] Produto produto)
        {
            if (id != produto.IdProduto)
            {
                return NotFound();
            }

            bool CodigoEANExistente = _context.Produto.Any(x => x.CodigoEAN == produto.CodigoEAN && x.IdProduto != produto.IdProduto);

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

        // GET: Produtos/Delete/5
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

        // POST: Produtos/Delete/5
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

            if(produto.Nome == "PRODUTO DELETADO")
            {
                return RedirectToAction(nameof(Index));
            }

            if (produto != null)
            {
                var listalogProdutos = await _context.LogProdutos.ToListAsync();

                foreach (var log in listalogProdutos)
                {
                    if(log.IdProduto == produto.IdProduto)
                    {
                        log.IdProduto = produtoNULO.IdProduto;
                        _context.SaveChanges();
                    }
                }
         
                _context.Produto.Remove(produto);
            }

            await _context.SaveChangesAsync();

            _context.LogProdutos.Add(new LogProdutos
            {
                EmailUsuario = User.Identity.Name,
                IdProduto = produtoNULO.IdProduto,
                AcaoLog = String.Concat("Usuário deletou o produto: ", produtoTMP.Nome.ToUpper()),
                DataLog = DateTime.Now,

            });

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(int id)
        {
          return (_context.Produto?.Any(e => e.IdProduto == id)).GetValueOrDefault();
        }
    }
}
