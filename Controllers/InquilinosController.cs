using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria.Controllers
{
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
			try
			{
				Repo.Alta(inquilino);
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
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
			try
			{
				Repo.Modificar(inquilino);
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
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
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				throw;
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