using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ANTS.Models;
using ANTS.Authentication;


namespace ANTS.Controllers
{
    public class SellerController : Controller
    {
        ANTSEntities context = new ANTSEntities();
        // GET: Seller
        [SellerAuthentication]
        public ActionResult Index()
        {
            return View();
        }
        [SellerAuthentication]
        public ActionResult Show()
        {
            var id = Convert.ToInt32(Session["id"].ToString());
            var list = (from p in context.Packages
                        where p.userid == id
                        select p).ToList();
            return View(list);
        }
        [SellerAuthentication]
        [HttpPost]
        public ActionResult Show(string searching)
        {
            var list = (from p in context.Packages
                        where p.packagename.Contains(searching)
                        select p).ToList();
            return View(list);
        }

        [SellerAuthentication]
        public ActionResult Create()
        {
            return View();
        }
        [SellerAuthentication]
        [HttpPost]
        public ActionResult Create(Package p)
        {
            if (ModelState.IsValid)
            {
                p.createdat = DateTime.Now;
                p.userid = Convert.ToInt32(Session["id"].ToString());
                context.Packages.Add(p);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }


        [SellerAuthentication]
        public ActionResult Edit(int id)
        {

            var p = context.Packages.FirstOrDefault(e => e.packageid == id);
            return View(p);
        }

        [SellerAuthentication]
        [HttpPost]
        public ActionResult Edit(Package p)
        {
            if (ModelState.IsValid)
            {
                var oldp = context.Packages.FirstOrDefault(e => e.packageid == p.packageid);
                oldp.packagename = p.packagename;
                oldp.price = p.price;
                oldp.category = p.category;
                oldp.discount = p.discount;
                oldp.details = p.details;
                oldp.location = p.location;
                oldp.advertisement = p.advertisement;
                context.SaveChanges();
                return RedirectToAction("Show");
            }
            return View(p);
        }
        [SellerAuthentication]
        public ActionResult Details(int id)
        {
            var p = context.Packages.FirstOrDefault(e => e.packageid == id);
            return View(p);
        }
        [SellerAuthentication]
        public ActionResult Delete(int id)
        {
            var p = context.Packages.FirstOrDefault(e => e.packageid == id);
            return View(p);
        }
        [SellerAuthentication]
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteP(int id)
        {
            var pr = context.Packages.FirstOrDefault(e => e.packageid == id);
            context.Packages.Remove(pr);
            context.SaveChanges();
            return RedirectToAction("Show");
        }

        //For Order table
        [SellerAuthentication]
        public ActionResult ShowOrders()
        {
            var id = Convert.ToInt32(Session["id"].ToString());
            var list = (from p in context.Orders
                        where p.sellerid == id
                        select p).ToList();
            return View(list);
        }
        [SellerAuthentication]
        [HttpPost]
        public ActionResult ShowOrders(string status,int id)
        {
            var oldp = context.Orders.FirstOrDefault(e => e.orderid == id);
            oldp.status = status;

            context.SaveChanges();
            return RedirectToAction("ShowOrders");
        }
        [SellerAuthentication]
        [HttpPost]
        public ActionResult ShowSearchOrders(string searching)
        {
            var list = (from p in context.Orders
                        where p.ordername.Contains(searching)
                        select p).ToList();
            return View(list);
        }

        //Dashboard
        [SellerAuthentication]
        public ActionResult Dashboard()
        {
            var id = Convert.ToInt32(Session["id"].ToString());
            var price = (from p in context.Orders
                         where p.sellerid == id
                         where p.status =="accepted"
                         select (p.totalprice));

            if (price != null)
            {
                var sum = price.Sum();
                ViewData["price"] = sum;
            }
            else
            {
                ViewData["price"] = 0;
            }

            var currentmonth = DateTime.Now.Month;
            var monthprice = (from p in context.Orders
                         where p.sellerid == id
                         where p.status == "accepted"
                         where p.createdat.Month == currentmonth
                         select (p.totalprice));
            if (monthprice != null)
            {
                var mon = monthprice.Sum();
                ViewData["monthprice"] = mon;
            }
            else
            {
                ViewData["monthprice"] = 0;
            }

            var orderpending = (from p in context.Orders
                              where p.sellerid == id
                              where p.status == "pending"
                              select p);
            if (orderpending != null)
            {
                var pending = orderpending.Count();
                ViewData["orderpending"] = pending;
            }
            else
            {
                ViewData["orderpending"] = 0;
            }


            return View();

        }

        [SellerAuthentication]
        public ActionResult EditProfile(int id)
        {

            var p = context.Users.FirstOrDefault(e => e.userid == id);
            return View(p);
        }

        [SellerAuthentication]
        [HttpPost]
        public ActionResult EditProfile(User p,string ConfirmPassword)
        {
            var oldp = context.Users.FirstOrDefault(e => e.userid == p.userid);
            oldp.name = p.name;
            oldp.email = p.email;
            oldp.image = p.image;
            oldp.phone = p.phone;

            if (p.password == ConfirmPassword)
            {
                oldp.password = p.password;
                context.SaveChanges();
                return View(oldp);
            }
            else
            {
                ViewBag.match = "Password did not match";
                return View(p);
            }
        }

        [SellerAuthentication]
        public ActionResult ShowNotice()
        {
            var id = Convert.ToInt32(Session["id"].ToString());
            var list = (from p in context.Notices
                        where p.userid == id
                        select p).ToList();
            return View(list);
        }
    }
}