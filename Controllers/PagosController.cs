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
	public class PagosController : Controller
	{
		private readonly RepositorioPago Repo;
		private readonly RepositorioContrato RepoContrato;

		public PagosController()
		{
			Repo = new RepositorioPago();
			RepoContrato = new RepositorioContrato();
		}

		// GET: Pagos
		public ActionResult Index()
		{
			var lista = Repo.GetPagos();
			return View(lista);
		}

		// GET: Pagos/Details/5
		public ActionResult Details(int id)
		{
			var pago = Repo.GetPagoPorId(id);
			return View(pago);
		}

		// GET: Pagos/Create
		public ActionResult Create()
		{
			try
			{
				return View();
			}
			catch
			{
				throw;
			}
		}

		// POST: Pagos/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Pago pago)
		{
			try
			{
				Repo.Alta(pago);
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: Pagos/Edit/5
		public ActionResult Edit(int id)
		{
			var pago = Repo.GetPagoPorId(id);
			return View(pago);
		}

		// POST: Pagos/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, Pago pago)
		{
			try
			{
				pago.IdPago = id;
				Repo.Modificar(pago);
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				throw;
			}
		}

		// GET: Pagos/Delete/5
		public ActionResult Delete(int id)
		{
			var pago = Repo.GetPagoPorId(id);
			return View(pago);
		}

		// POST: Pagos/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, Pago pago)
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

		// GET: Pagos/ObtenerUltimoPago/5
		public JsonResult ObtenerUltimoPago(int id)
		{
			var ultimoPago = Repo.ObtenerUltimoPago(id);
			return Json(ultimoPago);
		}
	}
}