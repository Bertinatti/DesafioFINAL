using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DesafioFINAL.Data;
using DesafioFINAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace DesafioFINAL.Controllers
{
    /// <summary>
    /// Acesso restrito aos usuários logados e com a role(função) de administrador
    /// </summary>
    [Authorize(Roles = "Administrador")]
    public class LogProdutosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public LogProdutosController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
       
        /// <summary>
        /// Método da controller de LogProdutos que captura os dados do banco e monta a view com os dados.
        /// </summary>
        /// <returns>Retorna a view Inicial do Log de Produtos com os dados capturados do banco.</returns>
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LogProdutos.Include(l => l.Produto);
            return View(await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// Método da controller de LogProdutos que exibe a view Details. 
        /// </summary>
        /// <param name="id">Id do LogProdutos, de tipo INT, capturado pelo clique do botão e enviado através de asp-route.</param>
        /// <returns>Retorna a view com os Detalhes desse Log.</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LogProdutos == null)
            {
                return NotFound();
            }

            var logProdutos = await _context.LogProdutos
                .Include(l => l.Produto)
                .FirstOrDefaultAsync(m => m.IdLog == id);
            if (logProdutos == null)
            {
                return NotFound();
            }

            return View(logProdutos);
        }

        /// <summary>
        /// Método da controller de LogProdutos que exibe a view Create.
        /// </summary>
        /// <returns>Retorna a view de Criação de um novo LogProdutos.</returns>
        public IActionResult Create()
        {
            // ViewData com os Produtos cadastrados no banco, para popular um select de Produtos da view Create
            ViewData["IdProduto"] = new SelectList(_context.Produto, "IdProduto", "Nome");
            return View();
        }

        /// <summary>
        /// Método da controller de LogProdutos para efetuar o Post com os dados do novo LogProdutos que será salvo no banco.
        /// </summary>
        /// <param name="logProdutos">Objeto da Classe LogProdutos que será salvo no banco.</param>
        /// <returns>Retorna para a Index, em caso de sucesso, ou exibe quais campos estão errados, permanecendo na tela de Criação.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdLog,EmailUsuario,IdProduto,AcaoLog,DataLog")] LogProdutos logProdutos)
        {
            bool emailExistente = _userManager.Users.Any(x => x.Email == logProdutos.EmailUsuario);
            if (emailExistente == false)
            {
                ModelState.AddModelError("EmailUsuario", "Usuário não está cadastrado");
            }

            if (ModelState.IsValid)
            {
                _context.Add(logProdutos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdProduto"] = new SelectList(_context.Produto, "IdProduto", "Nome", logProdutos.IdProduto);
            return View(logProdutos);
        }

        /// <summary>
        /// Método da controller de LogProdutos que exibe a view Edit.
        /// </summary>
        /// <param name="id">Id do LogProdutos, de tipo INT, capturado pelo clique do botão e enviado através de asp-route.</param>
        /// <returns>Retorna a view de Edição com os dados salvos do Log.</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LogProdutos == null)
            {
                return NotFound();
            }

            var logProdutos = await _context.LogProdutos.FindAsync(id);
            if (logProdutos == null)
            {
                return NotFound();
            }
            ViewData["IdProduto"] = new SelectList(_context.Produto, "IdProduto", "Nome", logProdutos.IdProduto);
            return View(logProdutos);
        }

        /// <summary>
        /// Método da controller de LogProdutos para efetuar o Post com os dados editados do LogProduto que será atualizado.
        /// </summary>
        /// <param name="id">Id do LogProdutos, de tipo INT, que foi capturado no método acima durante o redirecionamento para essa view, através do clique de um botão e por asp-route.</param>
        /// <param name="logProdutos">Objeto da Classe LogProdutos que terá seus dados atualizados.</param>
        /// <returns>Retorna para a Index, em caso de sucesso, ou exibe quais campos estão errados, permanecendo na tela de Edição.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdLog,EmailUsuario,IdProduto,AcaoLog,DataLog")] LogProdutos logProdutos)
        {
            if (id != logProdutos.IdLog)
            {
                return NotFound();
            }

            // Verifica se o e-mail/usuário existe no banco
            bool emailExistente = _userManager.Users.Any(x => x.Email == logProdutos.EmailUsuario);


            if (emailExistente == false)
            {
                ModelState.AddModelError("EmailUsuario", "Usuário não está cadatraddo.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(logProdutos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LogProdutosExists(logProdutos.IdLog))
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
            ViewData["IdProduto"] = new SelectList(_context.Produto, "IdProduto", "Nome", logProdutos.IdProduto);
            return View(logProdutos);
        }

        /// <summary>
        /// Método da controller de LogProdutos que exibe a view Delete
        /// </summary>
        /// <param name="id">Id do LogProdutos, de tipo INT, capturado pelo clique do botão e enviado através de asp-route.</param>
        /// <returns>Retorna a view de Deleção com os dados salvos do Log.</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LogProdutos == null)
            {
                return NotFound();
            }

            var logProdutos = await _context.LogProdutos
                .Include(l => l.Produto)
                .FirstOrDefaultAsync(m => m.IdLog == id);
            if (logProdutos == null)
            {
                return NotFound();
            }

            return View(logProdutos);
        }

        /// <summary>
        /// Método da controller de LogProdutos para efetuar o Post com os dados do Log que será deletado.
        /// </summary>
        /// <param name="id">Id do LogProdutos, de tipo INT, que foi capturado no método acima durante o redirecionamento para essa view, através do clique de um botão e por asp-route.</param>
        /// <returns>Retorna para a Index, em caso de sucesso, ou exibe os erros da Deleção.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LogProdutos == null)
            {
                return Problem("A entidade 'ApplicationDbContext.LogProdutos'  é nula.");
            }
            var logProdutos = await _context.LogProdutos.FindAsync(id);
            if (logProdutos != null)
            {
                _context.LogProdutos.Remove(logProdutos);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Método privado da controller de LodProdutos para verificar se o Log existe no banco.
        /// </summary>
        /// <param name="id">Id do LogProdutos, de tipo INT, passado durante a chamada do método.</param>
        /// <returns>Retorna um valor booleano, indicando se o Log existe ou não.</returns>
        private bool LogProdutosExists(int id)
        {
          return (_context.LogProdutos?.Any(e => e.IdLog == id)).GetValueOrDefault();
        }
    }
}
