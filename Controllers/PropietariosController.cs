using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria.Controllers
{
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
			var lista = Repo.GetPropietarios();
			return View(lista);
		}

		// GET: Propietarios/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: Propietarios/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Propietarios/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				// TODO: Add insert logic here

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
			return View();
		}

		// POST: Propietarios/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				// TODO: Add update logic here

				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: Propietarios/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: Propietarios/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				// TODO: Add delete logic here

				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}