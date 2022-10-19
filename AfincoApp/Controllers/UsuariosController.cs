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
        [Authorize]
        [Common.SessionExpireFilter]
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
        [Authorize]
        [Common.SessionExpireFilter]

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
                ViewBag.Errado = "Login ou Senha Incorretos";
                return View();
            }
            catch (Exception ex)
            {
                Common.LogErros(ex.TargetSite.ToString() + ex.Source.ToString() + ex.Message.ToString());
                return View();
            }
        }

        [Authorize]
        [Common.SessionExpireFilter]

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
        [Authorize]
        [Common.SessionExpireFilter]

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
        [Authorize]
        [Common.SessionExpireFilter]

        public ActionResult Create(Usuario usuario)
        {
            try
            {
                if (!Common.LoginExiste(usuario.Login, db))
                {
                    if (ModelState.IsValid && Common.TemPermissao(Enums.TiposUsuario.Master))
                    {
                        db.Usuarios.Add(usuario);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.Errado = "Login já Existente";
                return View(usuario);
            }
            catch (Exception ex)
            {
                Common.LogErros("LogOut: " + ex.Message);
                return View("~/Views/Home/Index.cshtml");
            }

        }

        // GET: Usuarios/Edit/5
        [Authorize]
        [Common.SessionExpireFilter]

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
                if (Common.TemPermissao(Enums.TiposUsuario.Master))
                    ViewBag.Permitido = true;
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
        [Authorize]
        [Common.SessionExpireFilter]

        public ActionResult Edit(Usuario usuario)
        {
            try
            {
                string LoginAnterior = db.Usuarios.Find(usuario.UsuarioID).Login;
                if (!Common.LoginExiste(usuario.Login, db) || usuario.Login == LoginAnterior)
                    if (ModelState.IsValid && Common.TemPermissao(Enums.TiposUsuario.Master))
                    {
                        var local = db.Set<Usuario>().Local.FirstOrDefault(f => f.UsuarioID == usuario.UsuarioID);
                        if (local != null)
                        {
                            db.Entry(local).State = EntityState.Detached;
                        }
                        db.Entry(usuario).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                ViewBag.Errado = "Login já existe nesse sistema";
                return View(usuario);
            }
            catch (Exception ex)
            {
                Common.LogErros("LogOut: " + ex.Message);
                return View("~/Views/Home/Index.cshtml");
            }
        }

        // GET: Usuarios/Delete/5
        [Authorize]
        [Common.SessionExpireFilter]

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
        [Authorize]
        [Common.SessionExpireFilter]

        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                if (Common.TemPermissao(Enums.TiposUsuario.Master))
                {
                    Usuario usuario = db.Usuarios.Find(id);
                    db.Usuarios.Remove(usuario);
                    db.SaveChanges();
                }
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
