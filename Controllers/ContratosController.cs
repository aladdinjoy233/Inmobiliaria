using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria.Controllers
{
	public class ContratosController : Controller
	{
		private readonly RepositorioContrato Repo;
		private readonly RepositorioInmueble RepoInmueble;
		private readonly RepositorioInquilino RepoInquilino;

		public ContratosController()
		{
			Repo = new RepositorioContrato();
			RepoInmueble = new RepositorioInmueble();
			RepoInquilino = new RepositorioInquilino();
		}

		// GET: Contratos
		public ActionResult Index()
		{
			var lista = Repo.GetContratos();
			return View(lista);
		}

		// GET: Contratos/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: Contratos/Create
		public ActionResult Create()
		{
			try
			{
				ViewBag.Inmuebles = RepoInmueble.GetInmuebles();
				ViewBag.Inquilinos = RepoInquilino.GetInquilinos();
				return View();
			}
			catch
			{
				throw;
			}
		}

		// POST: Contratos/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Contrato contrato)
		{
			try
			{
				Repo.Alta(contrato);
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				throw;
			}
		}

		// GET: Contratos/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: Contratos/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, Contrato contrato)
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

		// GET: Contratos/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: Contratos/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, Contrato contrato)
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