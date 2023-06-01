using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using greenery.Models;

namespace greenery.Controllers
{
    public class PlantsController : Controller
    {
        private DBGreeneryEntities db = new DBGreeneryEntities();
        [HttpGet]
        // GET: Plants
        public ActionResult Index()
        {
            return View(db.Plants.ToList());
        }
        [HttpPost]

        // GET: Plants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plant plant = db.Plants.Find(id);
            if (plant == null)
            {
                return HttpNotFound();
            }
            return View(plant);
        }

        // GET: Plants/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Plants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "plant_id,plant_name,plant_price,category,guidance,imagePath,ImageFile")] Plant plant)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(plant.ImageFile.FileName);
                string extension = Path.GetExtension(plant.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                plant.imagePath = "~/Images/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);

                plant.ImageFile.SaveAs(fileName);
                using (DBGreeneryEntities db = new DBGreeneryEntities())
                {
                    db.Plants.Add(plant);
                    db.SaveChanges();
                }
                ModelState.Clear();

                return RedirectToAction("Index", "Plants");
            }

            return View(plant);
        }
        // GET: Image
        public ActionResult showProduct()
        {

            Plant imageModel = new Plant();
           // Session["u_id"] = 2;
            if (TempData["cart"] != null)
            {
                float x = 0;
                List<cart> li2 = TempData["cart"] as List<cart>;
                foreach (var item in li2)
                {
                    x += item.bill;

                }

                TempData["total"] = x;
            }
            TempData.Keep();

            return View(db.Plants.OrderByDescending(x => x.plant_id).ToList());
        }
        // GET: Image
        
        public ActionResult Adtocart(int? Id)
        {

            Plant p = db.Plants.Where(x => x.plant_id == Id).SingleOrDefault();
            return View(p);
        }

        List<cart> li = new List<cart>();
        [HttpPost]
        //removed tbl_product pi
        public ActionResult Adtocart(Plant pi, string qty, int Id)
        {
            Plant p = db.Plants.Where(x => x.plant_id == Id).SingleOrDefault();

            cart c = new cart();
            c.productid = p.plant_id;
            c.price = (float)p.plant_price;
            c.qty = Convert.ToInt32(qty);
            c.bill = c.price * c.qty;
            c.productname = p.plant_name;
            if (TempData["cart"] == null)
            {
                li.Add(c);
                TempData["cart"] = li;

            }
            else
            {
                List<cart> li2 = TempData["cart"] as List<cart>;
                int flag = 0;
                foreach (var item in li2)
                {
                    if (item.productid == c.productid)
                    {
                        item.qty += c.qty;
                        item.bill += c.bill;
                        flag = 1;

                    }

                }
                if (flag == 0)
                {
                    li2.Add(c);
                }

                TempData["cart"] = li2;
            }

            TempData.Keep();



            return RedirectToAction("showProduct");
        }

        public ActionResult checkout()
        {
            TempData.Keep();


            return View();
        }

        [HttpPost]
        public ActionResult checkout(Order o)
        {

            using (var db = new DBGreeneryEntities())
            {
                List<cart> li = TempData["cart"] as List<cart>;
                foreach (var item in li)
                {
                    Order od = new Order();
                    od.orderdate = System.DateTime.Now;
                    od.plant_id = item.productid;
                    if (Session["u_id"] != null)
                    { 
                        od.client_id = Convert.ToInt32(Session["u_id"].ToString()); 
                    }
                    else
                    {
                        return RedirectToAction("UserLogin", "Clients");
                    }
                    od.quantity = item.qty;
                    od.Order_UnitPrice = (int)item.price;
                    od.Order_bill = item.bill;
                    od.orderstatus = item.orderstatus;


                    if (o.Client != null)
                    {
                        od.Client = new Client()
                        {

                            userName = o.Client.userName,
                            Phone = o.Client.Phone,
                            Password = o.Client.Password,
                            Email = o.Client.Email,
                            Address = o.Client.Address,


                        };
                    }


                    db.Orders.Add(od);
                    db.SaveChanges();


                }
                TempData.Remove("total");
                TempData.Remove("cart");
                
                TempData.Keep();
                TempData["msg"] = "Order has been confirmed";
                return RedirectToAction("showProduct");
            }
            
        }

        public ActionResult Remove(int? id)
        {

            li = TempData["cart"] as List<cart>;
            cart c = li.Where(x => x.productid == id).SingleOrDefault();
            li.Remove(c);

            float h = 0;
            foreach (var item in li)
            {
                h += item.bill;

            }
            TempData["total"] = h;
            TempData.Keep();
            return RedirectToAction("checkout");

        }

        // GET: Plants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plant plant = db.Plants.Find(id);
            if (plant == null)
            {
                return HttpNotFound();
            }
            return View(plant);
        }

        // POST: Plants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "plant_id,plant_name,plant_price,category,guidance,imagePath")] Plant plant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(plant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(plant);
        }

        // GET: Plants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plant plant = db.Plants.Find(id);
            if (plant == null)
            {
                return HttpNotFound();
            }
            return View(plant);
        }

        // POST: Plants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Plant plant = db.Plants.Find(id);
            db.Plants.Remove(plant);
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
