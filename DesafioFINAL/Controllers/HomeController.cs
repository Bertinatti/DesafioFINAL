using DesafioFINAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DesafioFINAL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Método que chama a exibição da página Index da Home. 
        /// </summary>
        /// <returns>Retorna a visualização da página Inicial</returns>
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Método que chama a exibição da página Privacy da Home
        /// </summary>
        /// <returns>Retorna a visualização da página de Privacidade</returns>
        public IActionResult Privacy()
        {
            return View();
        }
        /// <summary>
        /// Método que chama a exibição da página Error da Home
        /// </summary>
        /// <returns>Retorna ai visualização da página de Erros</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}