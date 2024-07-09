using Microsoft.AspNetCore.Mvc;
using AppLogin.Models;
using AppLogin.Data;
using AppLogin.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;

namespace AppLogin.Controllers
{
    public class AccesoController : Controller
    {
        private readonly AppDBContext _appDbContext;

        public AccesoController(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(UsuarioMV modelo)
        {
            if (ModelState.IsValid)
            {
                Usuario usuario = new Usuario
                {
                    NombreCompleto = modelo.NombreCompleto,
                    Correo = modelo.Correo,
                    Clave = modelo.Clave,
                };

                _appDbContext.Usuarios.Add(usuario);
                await _appDbContext.SaveChangesAsync();

                if (usuario.IdUsuario != 0)
                {
                    return RedirectToAction("Login", "Acceso");
                }
                else
                {
                    ViewData["Mensaje"] = "No se pudo crear el usuario, error fatal";
                }
            }

            // Si llegamos hasta aquí, algo falló, mostrar el formulario nuevamente
            return View(modelo);
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            Usuario? usuarioEncontrado = await _appDbContext.Usuarios
                .Where(u => u.Correo == modelo.Correo && u.Clave == modelo.Clave)
                .FirstOrDefaultAsync();

            if (usuarioEncontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias";
                return View();
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuarioEncontrado.NombreCompleto)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties
            {
                AllowRefresh = true,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
            );

            return RedirectToAction("Index", "Home");
        }
    }
}
