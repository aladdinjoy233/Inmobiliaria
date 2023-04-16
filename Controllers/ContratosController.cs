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
			ViewBag.Success = TempData["Success"];
			ViewBag.Info = TempData["Info"];
			var lista = Repo.GetContratosValidos();
			return View(lista);
		}

		// GET: Contratos/Expired
		public ActionResult Expired()
		{
			var lista = Repo.GetContratosExpirados();
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
			ViewBag.Inmuebles = RepoInmueble.GetInmueblesParaAlquilar();
			ViewBag.Inquilinos = RepoInquilino.GetInquilinos();
			return View();
		}

		// POST: Contratos/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Contrato contrato)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.Error = "Faltan datos";
				ViewBag.Inmuebles = RepoInmueble.GetInmueblesParaAlquilar();
				ViewBag.Inquilinos = RepoInquilino.GetInquilinos();
				return View(contrato);
			}

			try
			{
				Repo.Alta(contrato);
				TempData["Success"] = "Contrato creado correctamente";
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				ViewBag.Inmuebles = RepoInmueble.GetInmueblesParaAlquilar();
				ViewBag.Inquilinos = RepoInquilino.GetInquilinos();
				return View();
			}
		}

		// GET: Contratos/Edit/5
		public ActionResult Edit(int id)
		{
			var contrato = Repo.GetContratoPorId(id);

			if (!contrato.Activo)
			{
				return RedirectToAction(nameof(Index));
			}

			ViewBag.Inmuebles = RepoInmueble.GetInmueblesParaAlquilar();
			ViewBag.Inquilinos = RepoInquilino.GetInquilinos();
			ViewBag.InmuebleActual = RepoInmueble.GetInmueblePorId(contrato.InmuebleId);
			return View(contrato);

		}

		// POST: Contratos/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, Contrato contrato)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.Error = "Faltan datos";
				ViewBag.Inmuebles = RepoInmueble.GetInmueblesParaAlquilar();
				ViewBag.Inquilinos = RepoInquilino.GetInquilinos();
				var lastContrato = Repo.GetContratoPorId(id);
				ViewBag.InmuebleActual = RepoInmueble.GetInmueblePorId(lastContrato.InmuebleId);
				return View(lastContrato);
			}

			try
			{
				contrato.IdContrato = id;
				Repo.Modificar(contrato);
				TempData["Success"] = "Contrato modificado correctamente";
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				ViewBag.Inmuebles = RepoInmueble.GetInmueblesParaAlquilar();
				ViewBag.Inquilinos = RepoInquilino.GetInquilinos();
				ViewBag.InmuebleActual = RepoInmueble.GetInmueblePorId(contrato.InmuebleId);
				return View(contrato);
			}
		}

		// GET: Contratos/Delete/5
		public ActionResult Delete(int id)
		{
			var contrato = Repo.GetContratoPorId(id);

			if (!contrato.Activo)
			{
				return RedirectToAction(nameof(Index));
			}
			
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
				TempData["Success"] = "Contrato eliminado correctamente";
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				contrato.Inmueble = RepoInmueble.GetInmueblePorId(contrato.InmuebleId);
				return View(contrato);
			}
		}

		// GET: Contratos/Terminate/5
		public ActionResult Terminate(int id)
		{
			var contrato = Repo.GetContratoPorId(id);

			if (!contrato.Activo)
			{
				return RedirectToAction(nameof(Index));
			}

			contrato.Inmueble = RepoInmueble.GetInmueblePorId(contrato.InmuebleId);
			return View(contrato);
		}

		// POST: Contratos/Terminate/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Terminate(int id, Contrato contrato)
		{
			try
			{
				var multa = Repo.Terminar(id);
				TempData["Info"] = $"Contrato terminado correctamente. Multa a pagar: ${multa}";
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				var contratoDB = Repo.GetContratoPorId(id);
				contratoDB.Inmueble = RepoInmueble.GetInmueblePorId(contratoDB.InmuebleId);
				return View(contratoDB);
			}
		}

		// GET: Contratos/Renovate/5
		public ActionResult Renovate(int id)
		{
			var contrato = Repo.GetContratoPorId(id);
			contrato.Inmueble = RepoInmueble.GetInmueblePorId(contrato.InmuebleId);
			return View(contrato);
		}

		// GET: Contratos/RenovateForm/5
		public ActionResult RenovateForm(int id)
		{
			ViewBag.Inmuebles = RepoInmueble.GetInmueblesParaAlquilar();
			ViewBag.Inquilinos = RepoInquilino.GetInquilinos();
			var contrato = Repo.GetContratoPorId(id);

			// Agregarle 3 años al contrato por defecto
			contrato.FechaInicio = contrato.FechaFin ?? DateTime.Now;
			DateTime fechaFinViejo = contrato.FechaFin ?? DateTime.Now;
			contrato.FechaFin = fechaFinViejo.AddYears(3);

			ViewBag.InmuebleActual = RepoInmueble.GetInmueblePorId(contrato.InmuebleId);
			return View(contrato);
		}

		// POST: Contratos/RenovateForm/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult RenovateForm(int id, Contrato contrato)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.Error = "Faltan datos";
				ViewBag.Inmuebles = RepoInmueble.GetInmueblesParaAlquilar();
				ViewBag.Inquilinos = RepoInquilino.GetInquilinos();
				var contratoDB = Repo.GetContratoPorId(id);

				// Agregarle 3 años al contrato por defecto
				contratoDB.FechaInicio = contratoDB.FechaFin ?? DateTime.Now;
				DateTime fechaFinViejo = contratoDB.FechaFin ?? DateTime.Now;
				contratoDB.FechaFin = fechaFinViejo.AddYears(3);

				ViewBag.InmuebleActual = RepoInmueble.GetInmueblePorId(contratoDB.InmuebleId);
				return View(contratoDB);
			}

			try
			{
				// Buscar contratos previos, si existe, terminarlo antes de empezar uno nuevo
				var contratosPrevios = Repo.GetContratosIdsPorInmueble(contrato.InmuebleId);
				if (contratosPrevios.Count > 0)
				{
					foreach (var contratoPrevio in contratosPrevios)
					{
						Repo.Terminar(contratoPrevio);
					}
				}

				Repo.Renovar(contrato);
				TempData["Success"] = "Contrato renovado correctamente";
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				ViewBag.Inmuebles = RepoInmueble.GetInmueblesParaAlquilar();
				ViewBag.Inquilinos = RepoInquilino.GetInquilinos();
				var contratoDB = Repo.GetContratoPorId(id);

				// Agregarle 3 años al contrato por defecto
				contratoDB.FechaInicio = contratoDB.FechaFin ?? DateTime.Now;
				DateTime fechaFinViejo = contratoDB.FechaFin ?? DateTime.Now;
				contratoDB.FechaFin = fechaFinViejo.AddYears(3);

				ViewBag.InmuebleActual = RepoInmueble.GetInmueblePorId(contratoDB.InmuebleId);
				return View(contratoDB);
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