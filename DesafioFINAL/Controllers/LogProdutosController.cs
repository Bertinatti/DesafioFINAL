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

        // GET: LogProdutos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LogProdutos.Include(l => l.Produto);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: LogProdutos/Details/5
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

        // GET: LogProdutos/Create
        public IActionResult Create()
        {
            ViewData["IdProduto"] = new SelectList(_context.Produto, "IdProduto", "Nome");
            return View();
        }

        // POST: LogProdutos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdLog,EmailUsuario,IdProduto,AcaoLog,DataLog")] LogProdutos logProdutos)
        {
            bool emailExistente = _userManager.Users.Any(x => x.Email == logProdutos.EmailUsuario);
            if (emailExistente == false)
            {
                ModelState.AddModelError("EmailUsuario", "Usuário não está cadatraddo");
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

        // GET: LogProdutos/Edit/5
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

        // POST: LogProdutos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdLog,EmailUsuario,IdProduto,AcaoLog,DataLog")] LogProdutos logProdutos)
        {
            if (id != logProdutos.IdLog)
            {
                return NotFound();
            }

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

        // GET: LogProdutos/Delete/5
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

        // POST: LogProdutos/Delete/5
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

        private bool LogProdutosExists(int id)
        {
          return (_context.LogProdutos?.Any(e => e.IdLog == id)).GetValueOrDefault();
        }
    }
}
