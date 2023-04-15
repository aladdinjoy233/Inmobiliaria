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
	public class InquilinosController : Controller
	{
		private readonly RepositorioInquilino Repo;

		public InquilinosController()
		{
			Repo = new RepositorioInquilino();
		}
		// GET: Inquilinos
		public ActionResult Index()
		{
			ViewBag.Success = TempData["Success"];
			var lista = Repo.GetInquilinos();
			return View(lista);
		}

		// GET: Inquilinos/Details/5
		public ActionResult Details(int id)
		{
			var inquilino = Repo.GetInquilinoPorId(id);
			return View(inquilino);
		}

		// GET: Inquilinos/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Inquilinos/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Inquilino inquilino)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.Error = "Faltan datos";
				return View(inquilino);
			}
			
			try
			{
				Repo.Alta(inquilino);
				TempData["Success"] = "Inquilino creado con exito!";
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View(inquilino);
			}
		}

		// GET: Inquilinos/Edit/5
		public ActionResult Edit(int id)
		{
			var inquilino = Repo.GetInquilinoPorId(id);
			return View(inquilino);
		}

		// POST: Inquilinos/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, Inquilino inquilino)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.Error = "Faltan datos";
				return View(inquilino);
			}

			try
			{
				Repo.Modificar(inquilino);
				TempData["Success"] = "Inquilino modificado con exito!";
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View(inquilino);
			}
		}

		// GET: Inquilinos/Delete/5
		public ActionResult Delete(int id)
		{
			var inquilino = Repo.GetInquilinoPorId(id);
			return View(inquilino);
		}

		// POST: Inquilinos/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, Inquilino inquilino)
		{
			try
			{
				Repo.Eliminar(id);
				TempData["Success"] = "Inquilino eliminado con exito!";
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				ViewBag.Error = "No se pudo eliminar el inquilino";
				return View(inquilino);
			}
		}

		// GET: Inquilinos/Buscar
		public JsonResult Buscar(string searchTerm)
		{
			var res = Repo.Buscar(searchTerm);
			return Json(res);
		}
	}
}