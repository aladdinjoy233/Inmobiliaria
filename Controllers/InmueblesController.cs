using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria.Controllers
{
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
			var lista = Repo.GetInmuebles();
			return View(lista);
		}

		// GET: Inmuebles/Details/5
		public ActionResult Details(int id)
		{
			var inmueble = Repo.GetInmueblePorId(id);
			return View(inmueble);
		}

		// GET: Inmuebles/Create
		public ActionResult Create()
		{
			try
			{
				ViewBag.Propietarios = RepoPropietario.GetPropietarios();
				return View();
			}
			catch
			{
				throw;
			}
		}

		// POST: Inmuebles/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Inmueble inmueble)
		{
			try
			{
				Repo.Alta(inmueble);
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: Inmuebles/Edit/5
		public ActionResult Edit(int id)
		{
			ViewBag.Propietarios = RepoPropietario.GetPropietarios();
			var inmueble = Repo.GetInmueblePorId(id);
			return View(inmueble);
		}

		// POST: Inmuebles/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, Inmueble inmueble)
		{
			try
			{
				inmueble.IdInmueble = id;
				Repo.Modificar(inmueble);
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				// ViewBag.Propietarios = RepoPropietario.GetPropietarios();
				// return View(inmueble);
				throw;
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
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				throw;
			}
		}
	}
}