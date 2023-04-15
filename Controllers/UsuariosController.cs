using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inmobiliaria.Controllers
{
	public class UsuariosController : Controller
	{
		private IConfiguration configuration;
		private IWebHostEnvironment environment;
		private string hashSalt = "";
		private RepositorioUsuario Repo;

		public UsuariosController(IConfiguration configuration, IWebHostEnvironment environment)
		{
			this.configuration = configuration;
			this.environment = environment;
			this.hashSalt = configuration["Salt"] ?? "";
			this.Repo = new RepositorioUsuario();
		}

		// GET: Usuarios
		[Authorize(Policy = "Administrador")]
		public ActionResult Index()
		{
			var usuarios = Repo.GetUsuarios();
			return View(usuarios);
		}

		// GET: Usuarios/Create
		[Authorize(Policy = "Administrador")]
		public ActionResult Create()
		{
			ViewBag.Roles = Usuario.ObtenerRoles();
			return View();
		}

		// POST: Usuarios/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "Administrador")]
		public ActionResult Create(Usuario usuario)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.Roles = Usuario.ObtenerRoles();
				return View();
			}
			try
			{
				// Comprobar que el email ingresado no existe en la BD
				var usuarioDB = Repo.GetUsuarioPorEmail(usuario.Email);
				if (usuarioDB.IdUsuario > 0)
				{
					ModelState.AddModelError("Email", "El email ingresado ya existe");
					ViewBag.Error = "El email ingresado ya existe";
					ViewBag.Roles = Usuario.ObtenerRoles();
					return View();
				}

				// Comprobar que las contraseñas sean iguales
				if (usuario.Password != usuario.ConfirmPassword)
				{
					ModelState.AddModelError("ConfirmPassword", "Las contraseñas no coinciden");
					ViewBag.Error = "Las contraseñas no coinciden";
					ViewBag.Roles = Usuario.ObtenerRoles();
					return View();
				}

				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: usuario.Password,
					salt: System.Text.Encoding.ASCII.GetBytes(hashSalt),
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 10000,
					numBytesRequested: 256 / 8
				));
				usuario.Password = hashed;

				usuario.Rol = User.IsInRole("Administrador") ? usuario.Rol : (int)enRoles.Empleado;

				int res = Repo.Alta(usuario);
				Repo.ModificarContraseña(usuario);
				if (usuario.AvatarFile != null && usuario.IdUsuario > 0)
				{
					string wwwPath = environment.WebRootPath;
					string path = Path.Combine(wwwPath, "Uploads");
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}

					string fileName = $"avatar_{usuario.IdUsuario + Path.GetExtension(usuario.AvatarFile.FileName)}";
					string pathCompleto = Path.Combine(path, fileName);
					usuario.Avatar = Path.Combine("/Uploads", fileName);

					// Guardar la foto en la memoria
					using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
					{
						usuario.AvatarFile.CopyTo(stream);
					}
					Repo.ModificarAvatar(usuario);
				}

				return RedirectToAction(nameof(Index));
			}
			catch
			{
				ViewBag.Roles = Usuario.ObtenerRoles();
				return View();
			}
		}

		// GET: Usuarios/Perfil
		[Authorize]
		public ActionResult Perfil()
		{
			ViewData["Title"] = "Mi perfil";
			ViewBag.Roles = Usuario.ObtenerRoles();
			var IdUsuario = Convert.ToInt32(User.FindFirst("IdUsuario")?.Value);
			var usuario = Repo.GetUsuarioPorId(IdUsuario);
			TempData["Perfil"] = true;
			return View("Edit", usuario);
		}

		// GET: Usuarios/Edit/5
		[Authorize(Policy = "Administrador")]
		public ActionResult Edit(int id)
		{
			TempData["Perfil"] = false;
			ViewBag.Roles = Usuario.ObtenerRoles();
			var usuario = Repo.GetUsuarioPorId(id);
			return View(usuario);
		}

		// POST: Usuarios/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<ActionResult> Edit(int id, Usuario usuario)
		{

			// Contraseña no es obligatorio en edicion
			if (ModelState.ContainsKey("Password")) ModelState.Remove("Password");
			if (ModelState.ContainsKey("ConfirmPassword")) ModelState.Remove("ConfirmPassword");

			var usuarioDB = Repo.GetUsuarioPorId(usuario.IdUsuario);

			if (!ModelState.IsValid)
			{
				ViewBag.Roles = Usuario.ObtenerRoles();
				return View(usuarioDB);
			}

			try
			{
				// Comprobar que el email ingresado no existe en la BD
				var usuarioEmailCheck = Repo.GetUsuarioPorEmail(usuario.Email);
				if (usuarioEmailCheck.IdUsuario > 0 && usuarioEmailCheck.IdUsuario != usuario.IdUsuario)
				{
					ModelState.AddModelError("Email", "El email ingresado ya existe");
					ViewBag.Error = "Email no puede coincidir con otro usuario";
					ViewBag.Roles = Usuario.ObtenerRoles();
					return View(usuarioDB);
				}

				var modoEdicionDePerfil = TempData.ContainsKey("Perfil") && (bool)TempData.Peek("Perfil") == true;

				if (usuario.Password != null)
				{
					if (usuario.ConfirmPassword == null)
					{
						ModelState.AddModelError("ConfirmPassword", "No puede ser vacia");
						ViewBag.Error = "Confirmacion vacia";
						ViewBag.Roles = Usuario.ObtenerRoles();
						return View(usuarioDB);
					}

					if (usuario.Password != usuario.ConfirmPassword)
					{
						ModelState.AddModelError("ConfirmPassword", "Las contraseñas no coinciden");
						ViewBag.Error = "Las contraseñas no coinciden";
						ViewBag.Roles = Usuario.ObtenerRoles();
						return View(usuarioDB);
					}

					string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
						password: usuario.Password,
						salt: System.Text.Encoding.ASCII.GetBytes(hashSalt),
						prf: KeyDerivationPrf.HMACSHA1,
						iterationCount: 10000,
						numBytesRequested: 256 / 8
					));
					usuario.Password = hashed;

					Repo.ModificarContraseña(usuario);
				}

				usuario.Rol = User.IsInRole("Administrador") ? usuario.Rol : (int)enRoles.Empleado;

				int res = Repo.Modificar(usuario);
				if (usuario.AvatarFile != null && usuario.IdUsuario > 0)
				{
					string wwwPath = environment.WebRootPath;
					string path = Path.Combine(wwwPath, "Uploads");
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}

					// Usarmos el usuario de la base de datos para ver si existe una imagen
					if (usuarioDB.Avatar != null) {
						// Borrar la foto si existe
						string existingFilePath = Path.Combine(path, $"avatar_{usuario.IdUsuario}{Path.GetExtension(usuarioDB.Avatar)}");
						if (System.IO.File.Exists(existingFilePath))
						{
							System.IO.File.Delete(existingFilePath);
						}
					}

					string fileName = $"avatar_{usuario.IdUsuario + Path.GetExtension(usuario.AvatarFile.FileName)}";
					string pathCompleto = Path.Combine(path, fileName);
					usuario.Avatar = Path.Combine("/Uploads", fileName);

					// Guardar la foto en la memoria
					using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
					{
						usuario.AvatarFile.CopyTo(stream);
					}
					Repo.ModificarAvatar(usuario);
				}

				// Si estan editanto, modificar el claim
				if (modoEdicionDePerfil)
				{
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, usuario.Email ?? usuarioDB.Email),
						new Claim("FullName", $"{usuario.Nombre ?? usuarioDB.Nombre} {usuario.Apellido ?? usuarioDB.Apellido}"),
						new Claim(ClaimTypes.Role, usuario.RolNombre),
						new Claim("IdUsuario", usuario.IdUsuario.ToString()),
						new Claim("Avatar", usuario.Avatar ?? usuarioDB.Avatar ?? "")
					};

					var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

					await HttpContext.SignInAsync(
						CookieAuthenticationDefaults.AuthenticationScheme,
						new ClaimsPrincipal(claimsIdentity)
					);
				}

				if (modoEdicionDePerfil)
				{
					return RedirectToAction("Index", "Home");
				}
				else
				{
					return RedirectToAction(nameof(Index));
				}

			}
			catch
			{
				throw;
			}
		}

		// GET: Usuarios/Delete/5
		[Authorize(Policy = "Administrador")]
		public ActionResult Delete(int id)
		{
			var usuario = Repo.GetUsuarioPorId(id);
			return View(usuario);
		}

		// POST: Usuarios/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "Administrador")]
		public ActionResult Delete(int id, Usuario usuario)
		{
			try
			{
				var u = Repo.GetUsuarioPorId(id);
				
				// Borrar la foto si existe
				string wwwPath = environment.WebRootPath;
				string path = Path.Combine(wwwPath, "Uploads");
				string existingFilePath = Path.Combine(path, $"avatar_{u.IdUsuario}{Path.GetExtension(u.Avatar)}");
				if (System.IO.File.Exists(existingFilePath))
				{
					System.IO.File.Delete(existingFilePath);
				}

				Repo.Eliminar(id);
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				var u = Repo.GetUsuarioPorId(id);
				return View(u);
			}
		}

		// GET: Usuarios/Login
		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			TempData["returnUrl"] = returnUrl;
			return View();
		}

		// POST: Usuarios/Login
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginView login)
		{
			try
			{
				var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string) ? "/Home" : TempData["returnUrl"]?.ToString();
				if (ModelState.IsValid)
				{
					string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
						password: login.Password,
						salt: System.Text.Encoding.ASCII.GetBytes(hashSalt),
						prf: KeyDerivationPrf.HMACSHA1,
						iterationCount: 10000,
						numBytesRequested: 256 / 8
					));

					var usuario = Repo.GetUsuarioPorEmail(login.Email);

					// Salir temprano por si el usuario no existe o la contraseña es incorrecta
					if (usuario == null || usuario.Password != hashed)
					{
						ModelState.AddModelError("", "Usuario o contraseña incorrectos");
						TempData["returnUrl"] = returnUrl;
						return View();
					}

					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, usuario.Email),
						new Claim("FullName", $"{usuario.Nombre} {usuario.Apellido}"),
						new Claim(ClaimTypes.Role, usuario.RolNombre),
						new Claim("IdUsuario", usuario.IdUsuario.ToString()),
						new Claim("Avatar", usuario.Avatar ?? "")
					};

					var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

					await HttpContext.SignInAsync(
						CookieAuthenticationDefaults.AuthenticationScheme,
						new ClaimsPrincipal(claimsIdentity)
					);

					TempData.Remove("returnUrl");
					return Redirect(returnUrl);
				}

				TempData["returnUrl"] = returnUrl;
				return View();
			}
			catch
			{
				throw;
			}
		}

		// Get: /Salir
		[Route("salir", Name = "logout")]
		public async Task<ActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Home");
		}
	}
}