using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AfincoApp.DAL;
using AfincoApp.Models;
using AfincoApp.Utils;

namespace AfincoApp.Controllers
{
    [Authorize]
    [Common.SessionExpireFilter]


    public class MovimentacoesController : Controller
    {
        private AfincoContext db = new AfincoContext();


        // GET: Movimentacoes/Details/5

        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Movimentacao movimentacao = db.Movimentacoes.Find(id);
                ViewBag.BalancoID = movimentacao.BalancoID;
                if (movimentacao == null)
                {
                    return HttpNotFound();
                }
                return View(movimentacao);
            }
            catch (Exception ex)
            {
                Common.LogErros(ex.TargetSite.ToString() + ex.Source.ToString() + ex.Message.ToString());
                return View("~/Views/Home/Index.cshtml");
            }

        }

        // GET: Movimentacoes/Create
        [Common.PermissaoIntermediaria]

        public ActionResult Create(int BalancoID)
        {
            try
            {
                ViewBag.BalancoID = BalancoID;
                return View();
            }
            catch (Exception ex)
            {
                Common.LogErros(ex.TargetSite.ToString() + ex.Source.ToString() + ex.Message.ToString());
                return View("~/Views/Home/Index.cshtml");
            }

        }

        // POST: Movimentacoes/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Common.PermissaoIntermediaria]

        public ActionResult Create(Movimentacao movimentacao)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Movimentacoes.Add(movimentacao);
                    db.SaveChanges();
                    return RedirectToAction("Edit","Balancos", new {id = movimentacao.BalancoID });
                }

                ViewBag.BalancoID = new SelectList(db.Balancos, "BalancoID", "BalancoID", movimentacao.BalancoID);
                return View(movimentacao);
            }
            catch (Exception ex)
            {
                Common.LogErros(ex.TargetSite.ToString() + ex.Source.ToString() + ex.Message.ToString());
                return View("~/Views/Home/Index.cshtml");
            }

        }

        // GET: Movimentacoes/Edit/5
        [Common.PermissaoIntermediaria]

        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Movimentacao movimentacao = db.Movimentacoes.Find(id);
                if (movimentacao == null)
                {
                    return HttpNotFound();
                }
                ViewBag.BalancoID = movimentacao.BalancoID;
                return View(movimentacao);
            }
            catch (Exception ex)
            {
                Common.LogErros(ex.TargetSite.ToString() + ex.Source.ToString() + ex.Message.ToString());
                return View("~/Views/Home/Index.cshtml");
            }


        }

        // POST: Movimentacoes/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Common.PermissaoIntermediaria]

        public ActionResult Edit( Movimentacao movimentacao)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(movimentacao).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Edit", "Balancos", new { id = movimentacao.BalancoID });
                }
                ViewBag.BalancoID = new SelectList(db.Balancos, "BalancoID", "BalancoID", movimentacao.BalancoID);
                return View(movimentacao);
            }
            catch (Exception ex)
            {
                Common.LogErros(ex.TargetSite.ToString() + ex.Source.ToString() + ex.Message.ToString());
                return View("~/Views/Home/Index.cshtml");
            }

        }

        // GET: Movimentacoes/Delete/5
        [Common.PermissaoIntermediaria]

        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Movimentacao movimentacao = db.Movimentacoes.Find(id);
                ViewBag.BalancoID = movimentacao.BalancoID;
                if (movimentacao == null)
                {
                    return HttpNotFound();
                }
                return View(movimentacao);
            }
            catch (Exception ex)
            {
                Common.LogErros(ex.TargetSite.ToString() + ex.Source.ToString() + ex.Message.ToString());
                return View("~/Views/Home/Index.cshtml");
            }

        }

        // POST: Movimentacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [Common.PermissaoIntermediaria]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Movimentacao movimentacao = db.Movimentacoes.Find(id);
                db.Movimentacoes.Remove(movimentacao);
                db.SaveChanges();
                return RedirectToAction("Edit", "Balancos", new { id = movimentacao.BalancoID });
            }
            catch (Exception ex)
            {
                Common.LogErros(ex.TargetSite.ToString() + ex.Source.ToString() + ex.Message.ToString());
                return View("~/Views/Home/Index.cshtml");
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
