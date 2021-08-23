using ANTS.Models;
using System;
using System.Collections.Generic;
using ANTS.Authentication;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ANTS.Controllers
{
    [Authorize]
    [CustomerAuthentication]
    public class CustomerController : Controller
    {
        
        // GET: Customer
        ANTSEntities context = new ANTSEntities();
        public ActionResult Index()
        {
            var packages = (from p in context.Packages
                        where p.approvestatus == "active"
                        select p).ToList();

            return View(packages);
        }

        [HttpPost]
        public ActionResult Index(string searching)
        {
            var list = (from p in context.Packages
                        where p.packagename.Contains(searching)
                        select p).ToList();
            return View(list);
        }

        public ActionResult PackageDetails(int id)
        {
            var p = context.Packages.FirstOrDefault(e => e.packageid == id);
            return View(p);
        }
        
        public ActionResult BuyPackage(int id)
        {
            var p = context.Packages.FirstOrDefault(e => e.packageid == id);
            return View(p);
        }
        [HttpPost]
        public ActionResult BuyPackage(Package pk, string phone, string paytype, int quantity, string address)
        {
            var id = Convert.ToInt32(Session["id"]);
            var p = context.Packages.FirstOrDefault(e => e.packageid == pk.packageid);
            if (ModelState.IsValid)
            {
                 Order o = new Order();
            o.sellerid = p.userid;
            o.customerid = id;
            o.customerphone = phone;
            o.customeraddress = address;
            o.packageid = p.packageid;
            o.ordername = p.packagename;
            o.paytype = paytype;
            o.quantity = quantity;
            var v = quantity * p.price;
            o.totalprice = v;
            o.createdat = DateTime.Now;
            o.status = "unsold";
            context.Orders.Add(o);
            context.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        public ActionResult CustomerProfile()
        {
            var id = Convert.ToInt32(Session["id"]);
            var user = context.Users.FirstOrDefault(e => e.userid == id);

            return View(user);
        }

        public ActionResult EditProfile(int id)
        {
            var user = context.Users.FirstOrDefault(e => e.userid == id);
            return View(user);
        }

        [HttpPost]
        public ActionResult EditProfile(User u)
        {
            var user = context.Users.FirstOrDefault(e => e.userid == u.userid);
            context.Entry(user).CurrentValues.SetValues(u);
            context.SaveChanges();
            return RedirectToAction("CustomerProfile");
        }

        public ActionResult Notices()
        {
            var users = context.Notices.ToList();
            return View(users);
        }

        public ActionResult Transactions()
        {
            var id = Convert.ToInt32(Session["id"]);
            var transaction = (from a in context.Accounts
                            where a.userid == id
                            orderby a.createdat descending
                            select a).ToList();
            return View(transaction);
        }

        public ActionResult TransactionDetails(int id)
        {
            var transaction = context.Accounts.FirstOrDefault(e => e.accountid == id);
            return View(transaction);
        }

        public ActionResult Orderhistory()
        {
            var id = Convert.ToInt32(Session["id"]);
            var orders = (from o in context.Orders
                          where o.customerid == id
                          orderby o.createdat descending
                          select o).ToList();
            return View(orders);

        }

        [HttpPost]
        public ActionResult Orderhistory(string searching)
        {
            var id = Convert.ToInt32(Session["id"]);
            var list = (from o in context.Orders
                        where o.customerid == id
                        where o.status == "sold"
                        where o.status == "unsold"
                        orderby o.createdat descending
                        where o.ordername.Contains(searching)
                        select o).ToList();
            return View(list);
        }

        public ActionResult Orderdetails(int id)
        {
            var orders = context.Orders.FirstOrDefault(e => e.orderid == id);
            return View(orders);

        }

        
        public ActionResult CancelOrder(int id)
        {
            var orders = context.Orders.FirstOrDefault(e => e.orderid == id);
            return View(orders);

        }


        [HttpPost]
        public ActionResult CancelOrder(int id, int? arg2)
        {
            var orders = context.Orders.FirstOrDefault(e => e.orderid == id);
            orders.status = "cancelled";
            context.SaveChanges();
            return RedirectToAction("Orderhistory");

        }


        public ActionResult GiveRating()
        {
            return View();
        }

        public ActionResult Complain()
        {
            Rating r = new Rating();
            return View(r);
        }
        [HttpPost]
        public ActionResult Complain(Rating rt)
        {
            var id = Convert.ToInt32(Session["id"]);
            
            if (ModelState.IsValid)
            {
                Rating r = new Rating();
                r.userid = id;
                r.packageid = rt.packageid;
                r.rating1 = rt.rating1;
                r.complain = rt.complain;
                r.complainstatus = "unresolved";
                context.Ratings.Add(r);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(rt);
            
        }

        public ActionResult BlogWriting()
        {
            Blog b = new Blog();
            return View(b);
        }
        [HttpPost]
        public ActionResult BlogWriting(Blog b)
        {
            var id = Convert.ToInt32(Session["id"]);

            if (ModelState.IsValid)
            {
                Blog bg = new Blog();
                bg.userid = id;
                bg.blog1 = b.blog1;
                context.Blogs.Add(bg);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(b);

        }
      
    }
}