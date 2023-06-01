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
    public class ClientsController : Controller
    {
        private DBGreeneryEntities db = new DBGreeneryEntities();

        // GET: Clients
        public ActionResult Index()
        {
            return View(db.Clients.ToList());
        }

        [HttpGet]

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(Client Client)
        {
            if (ModelState.IsValid)
            {
                db.Clients.Add(Client);
                db.SaveChanges();

                return RedirectToAction("UserLogin");
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingEmail(string Email)
        {
            try
            {
                return Json(!IsEmailExists(Email));
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        private bool IsEmailExists(string email)=> Client.FindByEmail(email) != null;


        [HttpGet]
        public ActionResult UserLogin()
        {
            return View();
        }


        [HttpPost]
        public ActionResult UserLogin(tempuser user)
        {
            if (ModelState.IsValid)
            {
                var clientx = db.Clients.Where(c => c.Email.Equals(user.Email)
                                && c.Password.Equals(user.Password)).FirstOrDefault();

                if (clientx != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    Session["u_id"] = clientx.client_id;
                    Session["CustomerEmail"] = user.Email;
                    return RedirectToAction("showProduct", "Plants");
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

            return RedirectToAction("Index","Home");
        }






        public ActionResult Dashboard()
        {
            string email = Convert.ToString(Session["CustomerEmail"]);
            var user = db.Clients.Where(u => u.Email.Equals(email)).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("UserLogin");
            }
            else
            {
                return View(user);
            }
        }
        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "client_id,userName,Email,Address,Phone,Password")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(client);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "client_id,userName,Email,Address,Phone,Password")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
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
