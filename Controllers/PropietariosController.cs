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
	public class PropietariosController : Controller
	{
		private readonly RepositorioPropietario Repo;

		public PropietariosController()
		{
			Repo = new RepositorioPropietario();
		}
		// GET: Propietarios
		public ActionResult Index()
		{
			ViewBag.Success = TempData["Success"];
			var lista = Repo.GetPropietarios();
			return View(lista);
		}

		// GET: Propietarios/Details/5
		public ActionResult Details(int id)
		{
			var propietario = Repo.GetDetallePropietario(id);
			return View(propietario);
		}

		// GET: Propietarios/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Propietarios/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Propietario propietario)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.Error = "Faltan datos";
				return View(propietario);
			}

			try
			{
				Repo.Alta(propietario);
				TempData["Success"] = "Propietario creado con exito!";
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: Propietarios/Edit/5
		public ActionResult Edit(int id)
		{
			var propietario = Repo.GetPropietarioPorId(id);
			return View(propietario);
		}

		// POST: Propietarios/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, Propietario propietario)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.Error = "Faltan datos";
				return View(propietario);
			}

			try
			{
				Repo.Modificar(propietario);
				TempData["Success"] = "Propietario modificado con exito!";
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View(propietario);
			}
		}

		// GET: Propietarios/Delete/5
		public ActionResult Delete(int id)
		{
			var propietario = Repo.GetPropietarioPorId(id);
			return View(propietario);
		}

		// POST: Propietarios/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, Propietario propietario)
		{
			try
			{
				Repo.Eliminar(id);
				TempData["Success"] = "Propietario eliminado con exito!";
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				ViewBag.Error = "No se pudo eliminar el propietario";
				return View(propietario);
			}
		}

		// GET: Propietarios/Buscar
		public JsonResult Buscar(string searchTerm)
		{
			var res = Repo.Buscar(searchTerm);
			return Json(res);
		}
	}
}