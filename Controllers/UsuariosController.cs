using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria.Models;
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
		// [Authorize(Policy = "Administrador")]
		public ActionResult Index()
		{
			var usuarios = Repo.GetUsuarios();
			return View(usuarios);
		}

		// GET: Usuarios/Create
		// [Authorize(Policy = "Administrador")]
		public ActionResult Create()
		{
			ViewBag.Roles = Usuario.ObtenerRoles();
			return View();
		}

		// POST: Usuarios/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		// [Authorize(Policy = "Administrador")]
		public ActionResult Create(Usuario usuario)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.Roles = Usuario.ObtenerRoles();
				return View();
			}
			try
			{
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: usuario.Password,
					salt: System.Text.Encoding.ASCII.GetBytes(hashSalt),
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 10000,
					numBytesRequested: 256 / 8
				));
				usuario.Password = hashed;

				usuario.Rol = User.IsInRole("Administrator") ? usuario.Rol : (int)enRoles.Empleado;

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

		// GET: Usuarios/Edit/5
		// [Authorize(Policy = "Administrador")]
		public ActionResult Edit(int id)
		{
			ViewBag.Roles = Usuario.ObtenerRoles();
			var usuario = Repo.GetUsuarioPorId(id);
			return View(usuario);
		}

		// POST: Usuarios/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		// [Authorize(Policy = "Administrador")]
		public ActionResult Edit(int id, Usuario usuario)
		{

			if (!ModelState.IsValid && ModelState.ErrorCount > 1
				&& (ModelState.ContainsKey("Nombre")
				|| ModelState.ContainsKey("Apellido")
				|| ModelState.ContainsKey("Email"))
			)
			{
				ViewBag.Roles = Usuario.ObtenerRoles();
				var usuarioErr = Repo.GetUsuarioPorId(id);
				return View(usuarioErr);
			}

			try
			{

				if (usuario.Password != null)
				{
					string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
						password: usuario.Password,
						salt: System.Text.Encoding.ASCII.GetBytes(hashSalt),
						prf: KeyDerivationPrf.HMACSHA1,
						iterationCount: 10000,
						numBytesRequested: 256 / 8
					));
					usuario.Password = hashed;

					Console.WriteLine(usuario.Password);
					Repo.ModificarContraseña(usuario);
				}

				usuario.Rol = User.IsInRole("Administrator") ? usuario.Rol : (int)enRoles.Empleado;

				int res = Repo.Modificar(usuario);
				if (usuario.AvatarFile != null && usuario.IdUsuario > 0)
				{
					string wwwPath = environment.WebRootPath;
					string path = Path.Combine(wwwPath, "Uploads");
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}

					var usuarioDB = Repo.GetUsuarioPorId(usuario.IdUsuario);
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

				return RedirectToAction(nameof(Index));
			}
			catch
			{
				throw;
				// ViewBag.Roles = Usuario.ObtenerRoles();
				// return View();
			}
		}

		// GET: Usuarios/Delete/5
		// [Authorize(Policy = "Administrador")]
		public ActionResult Delete(int id)
		{
			var usuario = Repo.GetUsuarioPorId(id);
			return View(usuario);
		}

		// POST: Usuarios/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		// [Authorize(Policy = "Administrador")]
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
	}
}