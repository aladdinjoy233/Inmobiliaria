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
	public class ContratosController : Controller
	{
		private readonly RepositorioContrato Repo;
		private readonly RepositorioInmueble RepoInmueble;
		private readonly RepositorioInquilino RepoInquilino;
		private readonly RepositorioPropietario RepoPropietario;

		public ContratosController()
		{
			Repo = new RepositorioContrato();
			RepoInmueble = new RepositorioInmueble();
			RepoInquilino = new RepositorioInquilino();
			RepoPropietario = new RepositorioPropietario();
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
			var contrato = Repo.GetContratoPorId(id);
			contrato.Inmueble = RepoInmueble.GetInmueblePorId(contrato.InmuebleId);
			return View(contrato);
		}

		// GET: Contratos/Create
		public ActionResult Create()
		{
			try
			{
				ViewBag.Inmuebles = RepoInmueble.GetInmueblesParaAlquilar();
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
			ViewBag.Inmuebles = RepoInmueble.GetInmueblesParaAlquilar();
			ViewBag.Inquilinos = RepoInquilino.GetInquilinos();
			var contrato = Repo.GetContratoPorId(id);
			ViewBag.InmuebleActual = RepoInmueble.GetInmueblePorId(contrato.InmuebleId);
			return View(contrato);
		}

		// POST: Contratos/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, Contrato contrato)
		{
			try
			{
				contrato.IdContrato = id;
				Repo.Modificar(contrato);
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				throw;
			}
		}

		// GET: Contratos/Delete/5
		public ActionResult Delete(int id)
		{
			var contrato = Repo.GetContratoPorId(id);
			contrato.Inmueble = RepoInmueble.GetInmueblePorId(contrato.InmuebleId);
			return View(contrato);
		}

		// POST: Contratos/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, Contrato contrato)
		{
			try
			{
				Repo.Eliminar(id);
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: Contratos/Buscar?searchTerm=
		public JsonResult Buscar(string searchTerm)
		{
			try {
				var res = Repo.Buscar(searchTerm);
				return Json(res);
			}
			catch
			{
				throw;
			}
		}

		public JsonResult Obtener(int id)
		{
			var contrato = Repo.GetContratoPorId(id);
			return Json(contrato.MontoMensual);
		}
	}
}