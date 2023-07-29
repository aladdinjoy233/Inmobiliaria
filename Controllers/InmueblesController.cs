using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria.Controllers
{
	[Authorize]
	public class InmueblesController : Controller
	{
		private readonly RepositorioInmueble Repo;
		private readonly RepositorioPropietario RepoPropietario;

		public InmueblesController()
		{
			Repo = new RepositorioInmueble();
			RepoPropietario = new RepositorioPropietario();
		}

		// GET: Inmuebles
		public ActionResult Index()
		{
			ViewBag.Success = TempData["Success"];
			var lista = Repo.GetInmuebles();
			return View(lista);
		}

		// GET: Inmuebles/Available
		public ActionResult Available()
		{
			var lista = Repo.GetDisponibles();
			return View(lista);
		}

		// GET: Inmuebles/Details/5
		public ActionResult Details(int id)
		{
			var inmueble = Repo.GetDetalleInmueble(id);
			return View(inmueble);
		}

		// GET: Inmuebles/Create
		public ActionResult Create()
		{
			ViewBag.Usos = Inmueble.ObtenerUsos();
			ViewBag.Tipos = Inmueble.ObtenerTipos();
			ViewBag.Propietarios = RepoPropietario.GetPropietarios();
			return View();
		}

		// POST: Inmuebles/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Inmueble inmueble)
		{
			if (!ModelState.IsValid || inmueble.Uso == 0 || inmueble.Tipo == 0)
			{
				ViewBag.Error = "Faltan datos";
				if (inmueble.Uso == 0) ModelState.AddModelError("Uso", "El uso es requerido");
				if (inmueble.Tipo == 0)	ModelState.AddModelError("Tipo", "El tipo es requerido");
				ViewBag.Usos = Inmueble.ObtenerUsos();
				ViewBag.Tipos = Inmueble.ObtenerTipos();
				ViewBag.Propietarios = RepoPropietario.GetPropietarios();
				return View(inmueble);
			}

			try
			{
				Repo.Alta(inmueble);
				TempData["Success"] = "Inmueble creado correctamente";
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				ViewBag.Usos = Inmueble.ObtenerUsos();
				ViewBag.Tipos = Inmueble.ObtenerTipos();
				ViewBag.Propietarios = RepoPropietario.GetPropietarios();
				return View();
			}
		}

		// GET: Inmuebles/Edit/5
		public ActionResult Edit(int id)
		{
			ViewBag.Propietarios = RepoPropietario.GetPropietarios();
			ViewBag.Usos = Inmueble.ObtenerUsos();
			ViewBag.Tipos = Inmueble.ObtenerTipos();
			var inmueble = Repo.GetInmueblePorId(id);
			return View(inmueble);
		}

		// POST: Inmuebles/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, Inmueble inmueble)
		{
			if (!ModelState.IsValid || inmueble.Uso == 0 || inmueble.Tipo == 0)
			{
				ViewBag.Error = "Faltan datos";
				if (inmueble.Uso == 0) ModelState.AddModelError("Uso", "El uso es requerido");
				if (inmueble.Tipo == 0)	ModelState.AddModelError("Tipo", "El tipo es requerido");
				ViewBag.Usos = Inmueble.ObtenerUsos();
				ViewBag.Tipos = Inmueble.ObtenerTipos();
				ViewBag.Propietarios = RepoPropietario.GetPropietarios();
				return View(inmueble);
			}
			try
			{
				inmueble.IdInmueble = id;
				Repo.Modificar(inmueble);
				TempData["Success"] = "Inmueble modificado correctamente";
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				ViewBag.Usos = Inmueble.ObtenerUsos();
				ViewBag.Tipos = Inmueble.ObtenerTipos();
				ViewBag.Propietarios = RepoPropietario.GetPropietarios();
				return View(inmueble);
			}
		}

		// GET: Inmuebles/Delete/5
		public ActionResult Delete(int id)
		{
			var inmueble = Repo.GetInmueblePorId(id);
			return View(inmueble);
		}

		// POST: Inmuebles/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, Inmueble inmueble)
		{
			try
			{
				Repo.Eliminar(id);
				TempData["Success"] = "Inmueble eliminado correctamente";
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View(inmueble);
			}
		}

		// GET: Inmuebles/Buscar?searchTerm=
		public JsonResult Buscar(string searchTerm)
		{
			var res = Repo.Buscar(searchTerm);
			return Json(res);
		}

		// GET: Inmuebles/Buscar/5?searchTerm
		public JsonResult BuscarConId(int id, string searchTerm)
		{
			var res = Repo.BuscarConId(id, searchTerm);
			return Json(res);
		}
	}
}