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

namespace AfincoApp.Controllers
{
    public class BalancosController : Controller
    {
        private AfincoContext db = new AfincoContext();

        // GET: Balancos
        public ActionResult Index()
        {
            var balancos = db.Balancos.Include(b => b.Cliente);
            return View(balancos.ToList());
        }

        // GET: Balancos/Details/5
        public ActionResult Details(int? id)
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

        // GET: Balancos/Create
        public ActionResult Create()
        {
            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nome");
            return View();
        }

        // POST: Balancos/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BalancoID,Ano,Periodo,ClienteID")] Balanco balanco)
        {
            if (ModelState.IsValid)
            {
                db.Balancos.Add(balanco);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nome", balanco.ClienteID);
            return View(balanco);
        }

        // GET: Balancos/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Balancos/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BalancoID,Ano,Periodo,ClienteID")] Balanco balanco)
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

        // GET: Balancos/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Balancos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Balanco balanco = db.Balancos.Find(id);
            db.Balancos.Remove(balanco);
            db.SaveChanges();
            return RedirectToAction("Index");
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
