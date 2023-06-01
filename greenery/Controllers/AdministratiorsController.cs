using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using greenery.Models;

namespace greenery.Controllers
{
    public class AdministratiorsController : Controller
    {
        private DBGreeneryEntities db = new DBGreeneryEntities();

        // GET: Administratiors
        public ActionResult Dashboard()
        {
            return View(db.Administratiors.ToList());
        }

        [HttpGet]

        public ActionResult a_SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult a_SignUp(Administratior administratior)
        {
            if (ModelState.IsValid)
            {
                db.Administratiors.Add(administratior);
                db.SaveChanges();

                return RedirectToAction("UserLogin");
            }

            return View();
        }
       /* public ActionResult a_Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult a_Login(tempuser a_user)
        {
            if (ModelState.IsValid)
            {
                var adminx = db.Administratiors.Where(c => c.admin_email.Equals(a_user.Email)
                                && c.admin_password.Equals(a_user.Password)).FirstOrDefault();

                if (adminx != null)
                {
                    FormsAuthentication.SetAuthCookie(a_user.Email, false);
                    Session["a_id"] = adminx.admin_id;
                    Session["adminEmail"] = a_user.Password;
                    return RedirectToAction("a_Dashboard", "Administratiors");
                    //return Content("Login Successful!");
                }
                else
                {
                    ViewBag.Failed = "Enter Correct Email and Password";
                    return View();
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            return RedirectToAction("Index", "Home");
        }*/

        /*public ActionResult a_Dashboard()
        {
            /*string email = Convert.ToString(Session["adminEmail"]);
            var user = db.Administratiors.Where(u => u.admin_email.Equals(email)).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("a_Login");
            }
            else
            {
                return View(user);
            }
            
        }*/

        // GET: Administratiors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administratior administratior = db.Administratiors.Find(id);
            if (administratior == null)
            {
                return HttpNotFound();
            }
            return View(administratior);
        }

        // GET: Administratiors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administratiors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "admin_id,admin_name,admin_email,admin_password")] Administratior administratior)
        {
            if (ModelState.IsValid)
            {
                db.Administratiors.Add(administratior);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(administratior);
        }

        // GET: Administratiors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administratior administratior = db.Administratiors.Find(id);
            if (administratior == null)
            {
                return HttpNotFound();
            }
            return View(administratior);
        }

        // POST: Administratiors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "admin_id,admin_name,admin_email,admin_password")] Administratior administratior)
        {
            if (ModelState.IsValid)
            {
                db.Entry(administratior).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(administratior);
        }

        // GET: Administratiors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administratior administratior = db.Administratiors.Find(id);
            if (administratior == null)
            {
                return HttpNotFound();
            }
            return View(administratior);
        }

        // POST: Administratiors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Administratior administratior = db.Administratiors.Find(id);
            db.Administratiors.Remove(administratior);
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
