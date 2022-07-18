// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using DesafioFINAL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace DesafioFINAL.Areas.Identity.Pages.Account
{
    [Authorize(Roles ="Administrador")]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Key]
            [Column("IdUsuario", TypeName = "integer")]
            [Display(Name = "Id do Usuário")]
            public int IdUsuario { get; set; }

            [Required]
            [StringLength(250, ErrorMessage = "O {0} pode ter no máximo {1} digitos.")]
            [Column("Nome", TypeName = "varchar(250)")]
            [Display(Name = "Nome")]
            public string Nome { get; set; }

            [Required]
            [StringLength(250, ErrorMessage = "O {0} pode ter no máximo {1} digitos.")]
            [Column("Sobrenome", TypeName = "varchar(250)")]
            [Display(Name = "Sobrenome")]
            public string Sobrenome { get; set; }

            [Required]
            [StringLength(20, ErrorMessage = "O {0} pode ter no máximo {1} digitos.")]
            [Column("CEP", TypeName = "varchar(20)")]
            [Display(Name = "CEP")]
            public string CEP { get; set; }

            [Required]
            [StringLength(250, ErrorMessage = "O {0} pode ter no máximo {1} digitos.")]
            [Column("Endereco", TypeName = "varchar(250)")]
            [Display(Name = "Endereço")]
            public string Endereco { get; set; }

            [Required]
            [Column("Numero", TypeName = "integer")]
            [Display(Name = "Número")]
            public int Numero { get; set; }

            [Required]
            [StringLength(250, ErrorMessage = "O {0} pode ter no máximo {1} digitos.")]
            [Column("Bairro", TypeName = "varchar(250)")]
            [Display(Name = "Bairro")]
            public string Bairro { get; set; }

            [Required]
            [StringLength(250, ErrorMessage = "O {0} pode ter no máximo {1} digitos.")]
            [Column("Cidade", TypeName = "varchar(250)")]
            [Display(Name = "Cidade")]
            public string Cidade { get; set; }

            [Required]
            [StringLength(2, ErrorMessage = "O {0} pode ter no máximo {1} digitos.")]
            [Column("Estado", TypeName = "varchar(2)")]
            [Display(Name = "Estado")]
            public string Estado { get; set; }

            [Required]
            [Column("TipoUsuario", TypeName = "varchar(50)")]
            [Display(Name = "Tipo de Usuário")]
            public string TipoUsuario { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Senha")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirme a senha")]
            [Compare("Password", ErrorMessage = "As senhas digitadas não são iguais.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new Usuario
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Nome = Input.Nome,
                    Sobrenome = Input.Sobrenome,
                    CEP = Input.CEP,
                    Endereco = Input.Endereco,
                    Numero = Input.Numero,
                    Bairro = Input.Bairro,
                    Cidade = Input.Cidade,
                    Estado = Input.Estado,
                    TipoUsuario = Input.TipoUsuario,
                };
       
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);             

                if (result.Succeeded)
                {
                    _logger.LogInformation("Usuário cadastrou uma nova conta com senha.");

                    if (user.TipoUsuario.Equals(MyRoles.RoleAdmin))
                    {
                        await _userManager.AddToRoleAsync(user, MyRoles.RoleAdmin);
                    }
                    else if (user.TipoUsuario.Equals(MyRoles.RoleStaff))
                    {
                        await _userManager.AddToRoleAsync(user, MyRoles.RoleStaff);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, MyRoles.RoleStaff);
                    }

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirme seu e-mail",
                        $"Por favor, confirme a sua conta<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicando aqui</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        //await _signInManager.SignInAsync(user, isPersistent: true);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Não foi possível cria o usuário '{nameof(IdentityUser)}'. " +
                    $"Verifique '{nameof(IdentityUser)}' não é uma classe abstrata e tem um construtor sem parâmetros, ou alternativamente " +
                    $"substituiu a página em /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("A aplicação necessita de um e-mail para o usuário.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
