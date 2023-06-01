using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using greenery.Models;

namespace greenery.Controllers
{
    public class FeedbacksController : Controller
    {
        private DBGreeneryEntities db = new DBGreeneryEntities();
       
        // GET: Feedbacks
        public ActionResult Index()
        {
            var feedbacks = db.Feedbacks.Include(f => f.Client);
            return View(feedbacks.ToList());
        }

        // GET: Feedbacks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // GET: Feedbacks/Create
        public ActionResult Create()
        {
            ViewBag.client_id = new SelectList(db.Clients, "client_id", "userName");
            return View();
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "feedback_id,client_id,comments,date_")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                
                db.Feedbacks.Add(feedback);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.client_id = new SelectList(db.Clients, "client_id", "userName", feedback.client_id);
            return View(feedback);
        }
        [HttpGet]
        public ActionResult Feedback()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Feedback(Feedback feedback)
        {
            

                using (var db = new DBGreeneryEntities())
                {
                    Feedback fed = new Feedback();

                    fed.comments = feedback.comments;
                    fed.date_ = System.DateTime.Now;
                    if (Session["u_id"] != null)
                    {
                        fed.client_id = Convert.ToInt32(Session["u_id"].ToString());
                    }
                    else
                    {
                        return RedirectToAction("UserLogin", "Clients");
                    }



                    if (feedback.Client != null)
                    {
                        fed.Client = new Client()
                        {

                            userName = feedback.Client.userName,
                            Phone = feedback.Client.Phone,
                            Password = feedback.Client.Password,
                            Email = feedback.Client.Email,


                        };
                    }

                    db.Feedbacks.Add(fed);
                    db.SaveChanges();

                    return RedirectToAction("showProduct", "Plants");
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
