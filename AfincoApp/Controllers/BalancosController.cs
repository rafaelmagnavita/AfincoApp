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
    [Common.SessionExpireFilter]
    [Authorize]

    public class BalancosController : Controller
    {

        private AfincoContext db = new AfincoContext();

        // GET: Balancos/Importar/

        public ActionResult Importar()
        {
            try
            {
                List<Movimentacao> movimentacoes = Common.ImportarExcel("", @"C:\Logs\teste.xlsx");
                ViewBag.Movimentacoes = movimentacoes;
                return View();
            }
            catch (Exception ex)
            {
                Common.LogErros(ex.TargetSite.ToString() + ex.Source.ToString() + ex.Message.ToString());
                return View("~/Views/Home/Index.cshtml");
            }
        }

        // POST: Balancos/Importar
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Common.PermissaoIntermediaria]

        public ActionResult Importar(Balanco balanco, List<Movimentacao> movimentacoes)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Balancos.Add(balanco);
                    db.SaveChanges();
                    foreach (Movimentacao movimentacao in movimentacoes)
                    {
                        if (ModelState.IsValid)
                        {
                            movimentacao.BalancoID = balanco.BalancoID;
                            db.Movimentacoes.Add(movimentacao);
                            db.SaveChanges();
                        }
                    }

                    return RedirectToAction("Edit", "Balancos", new { id = balanco.BalancoID });
                }
                return RedirectToAction("Edit", "Balancos", new { id = balanco.BalancoID });
            }
            catch (Exception ex)
            {
                Balanco balanco1 = db.Balancos.Find(balanco.BalancoID);
                foreach (Movimentacao movimentacao in db.Movimentacoes.Where(a => a.BalancoID == balanco.BalancoID))
                {
                    db.Movimentacoes.Remove(movimentacao);
                    db.SaveChanges();
                }
                db.Balancos.Remove(balanco1);
                db.SaveChanges();
                Common.LogErros(ex.TargetSite.ToString() + ex.Source.ToString() + ex.Message.ToString());
                return View("~/Views/Home/Index.cshtml");
            }

        }

        // GET: Balancos/Details/5
        public ActionResult Details(int? id)
        {

            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Balanco balanco = db.Balancos.Find(id);
                if (balanco == null)
                {
                    return HttpNotFound();
                }
                return View(balanco);
            }
            catch (Exception ex)
            {
                Common.LogErros(ex.TargetSite.ToString() + ex.Source.ToString() + ex.Message.ToString());
                return View("~/Views/Home/Index.cshtml");
            }
        }

        // GET: Balancos/Create
        [Common.PermissaoIntermediaria]

        public ActionResult Create(int ClienteID)
        {

            try
            {
                ViewBag.ClienteID = ClienteID;
                return View();
            }
            catch (Exception ex)
            {
                Common.LogErros(ex.TargetSite.ToString() + ex.Source.ToString() + ex.Message.ToString());
                return View("~/Views/Home/Index.cshtml");
            }

        }

        // POST: Balancos/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Common.PermissaoIntermediaria]

        public ActionResult Create(Balanco balanco)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Balancos.Add(balanco);
                    db.SaveChanges();
                    return RedirectToAction("Edit", "Clientes", new { id = balanco.ClienteID });
                }

                ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nome", balanco.ClienteID);
                return View(balanco);
            }
            catch (Exception ex)
            {
                Common.LogErros(ex.TargetSite.ToString() + ex.Source.ToString() + ex.Message.ToString());
                return View("~/Views/Home/Index.cshtml");
            }

        }

        // GET: Balancos/Edit/5
        [Common.PermissaoIntermediaria]

        public ActionResult Edit(int? id)
        {

            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Balanco balanco = db.Balancos.Find(id);
                if (balanco == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nome", balanco.ClienteID);
                return View(balanco);
            }
            catch (Exception ex)
            {
                Common.LogErros(ex.TargetSite.ToString() + ex.Source.ToString() + ex.Message.ToString());
                return View("~/Views/Home/Index.cshtml");
            }


        }

        // POST: Balancos/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Common.PermissaoIntermediaria]

        public ActionResult Edit([Bind(Include = "BalancoID,Ano,Periodo,ClienteID")] Balanco balanco)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(balanco).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nome", balanco.ClienteID);
                return View(balanco);
            }
            catch (Exception ex)
            {
                Common.LogErros(ex.TargetSite.ToString() + ex.Source.ToString() + ex.Message.ToString());
                return View("~/Views/Home/Index.cshtml");
            }

        }

        // GET: Balancos/Delete/5
        [Common.PermissaoIntermediaria]

        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Balanco balanco = db.Balancos.Find(id);
                if (balanco == null)
                {
                    return HttpNotFound();
                }
                return View(balanco);
            }
            catch (Exception ex)
            {
                Common.LogErros(ex.TargetSite.ToString() + ex.Source.ToString() + ex.Message.ToString());
                return View("~/Views/Home/Index.cshtml");
            }

        }

        // POST: Balancos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Common.PermissaoIntermediaria]

        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Balanco balanco = db.Balancos.Find(id);
                db.Balancos.Remove(balanco);
                db.SaveChanges();
                return RedirectToAction("Index");
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
