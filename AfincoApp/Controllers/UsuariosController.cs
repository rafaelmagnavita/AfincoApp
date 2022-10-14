using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AfincoApp.DAL;
using AfincoApp.Models;
using AfincoApp.Utils;


namespace AfincoApp.Controllers
{
    public class UsuariosController : Controller
    {
        private AfincoContext db = new AfincoContext();

        // GET: Usuarios
        public ActionResult Index()
        {
            try
            {
                return View(db.Usuarios.ToList());
            }
            catch (Exception ex)
            {
                Common.LogErros(ex.TargetSite.ToString() + ex.Source.ToString() + ex.Message.ToString());
                return View("~/Views/Home/Index.cshtml");
            }
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Usuario usuario = db.Usuarios.Find(id);
                if (usuario == null)
                {
                    return HttpNotFound();
                }
                return View(usuario);
            }
            catch (Exception ex)
            {
                Common.LogErros(ex.TargetSite.ToString() + ex.Source.ToString() + ex.Message.ToString());
                return View("~/Views/Home/Index.cshtml");
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        // POST: Usuarios/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Usuario usuario)
        {
            try
            {
                var usuarioexiste = db.Usuarios.Where(a => a.Login == usuario.Login).FirstOrDefault();
                if (usuarioexiste != null && usuarioexiste.Senha == usuario.Senha)
                {
                    Session["usuario"] = usuarioexiste;
                    var ticket = FormsAuthentication.Encrypt(new FormsAuthenticationTicket(
                    1, usuarioexiste.Nome, DateTime.Now, DateTime.Now.AddHours(12), true, usuarioexiste.Tipo.ToString()));
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticket);
                    Response.Cookies.Add(cookie);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Login ou Senha Incorretos");
                return View();
            }
            catch (Exception ex)
            {
                Common.LogErros(ex.TargetSite.ToString() + ex.Source.ToString() + ex.Message.ToString());
                return View();
            }
        }

        public ActionResult LogOut()
        {
            try
            {
                Session.Clear();
                Session.Abandon();
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Usuarios");
            }
            catch (Exception ex)
            {
                Common.LogErros("LogOut: " + ex.Message);
                return View("~/Views/Home/Index.cshtml");
            }

        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            try
            {
                return View();

            }
            catch (Exception ex)
            {
                Common.LogErros("LogOut: " + ex.Message);
                return View("~/Views/Home/Index.cshtml");
            }
        }

        // POST: Usuarios/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UsuarioID,Nome,Sobrenome,Login,Senha")] Usuario usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Usuarios.Add(usuario);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(usuario);
            }
            catch (Exception ex)
            {
                Common.LogErros("LogOut: " + ex.Message);
                return View("~/Views/Home/Index.cshtml");
            }

        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Usuario usuario = db.Usuarios.Find(id);
                if (usuario == null)
                {
                    return HttpNotFound();
                }
                return View(usuario);
            }
            catch (Exception ex)
            {
                Common.LogErros("LogOut: " + ex.Message);
                return View("~/Views/Home/Index.cshtml");
            }

        }

        // POST: Usuarios/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UsuarioID,Nome,Sobrenome,Login,Senha")] Usuario usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(usuario).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(usuario);
            }
            catch (Exception ex)
            {
                Common.LogErros("LogOut: " + ex.Message);
                return View("~/Views/Home/Index.cshtml");
            }
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Usuario usuario = db.Usuarios.Find(id);
                if (usuario == null)
                {
                    return HttpNotFound();
                }
                return View(usuario);
            }
            catch (Exception ex)
            {
                Common.LogErros("LogOut: " + ex.Message);
                return View("~/Views/Home/Index.cshtml");
            }

        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Usuario usuario = db.Usuarios.Find(id);
                db.Usuarios.Remove(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Common.LogErros("LogOut: " + ex.Message);
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
