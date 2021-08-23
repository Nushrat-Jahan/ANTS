using ANTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ANTS.Authentication;

namespace ANTS.Controllers
{
    public class ManagerController : Controller
    {
        ANTSEntities context = new ANTSEntities();
        // GET: Manager

        [ManagerAuthentication]
        public ActionResult Index()
        {
            return View();
        }


        [ManagerAuthentication]
        public ActionResult CreateSeller()
        {
            User u = new User();
            u.createdat = DateTime.Now;
            return View(u);
  
        }

        [ManagerAuthentication]
        [HttpPost]
        public ActionResult CreateSeller(User u)
        {
            if (ModelState.IsValid)
            {
                context.Users.Add(u);
                context.SaveChanges();
                return RedirectToAction("/Index");
            }
            return View();
        }

        [ManagerAuthentication]
        public ActionResult ViewUsers()
        {
            var users = context.Users.ToList();
            return View(users);
        }

     

        [ManagerAuthentication]
        [HttpPost]
        public ActionResult ViewUsers(string searching)
        {
            var users = (from u in context.Users
                        where u.name.Contains(searching)
                        select u).ToList();
            return View(users);
        }


        [ManagerAuthentication]
        public ActionResult EditUser(int id)
        {
            var user = context.Users.FirstOrDefault(e => e.userid == id);
            return View(user);
        }


        [ManagerAuthentication]
        [HttpPost]
        public ActionResult EditUser(User u)
        {
            var user = context.Users.FirstOrDefault(e => e.userid == u.userid);
            context.Entry(user).CurrentValues.SetValues(u);
            context.SaveChanges();
            return RedirectToAction("/Index");
        }


        [ManagerAuthentication]
        public ActionResult DeleteUser(int id)
        {
            var user = context.Users.FirstOrDefault(e => e.userid == id);
            return View(user);
        }


        [ManagerAuthentication]
        [HttpPost]
        [ActionName("DeleteUser")]
        public ActionResult DeleteUserU(int id)
        {
            var user = context.Users.FirstOrDefault(e => e.userid == id);
            context.Users.Remove(user);
            context.SaveChanges();
            return RedirectToAction("/Index");
        }


        [ManagerAuthentication]
        public ActionResult ViewPackages()
        {
            var packages = context.Packages.ToList();
            return View(packages);
        }


        [ManagerAuthentication]
        [HttpPost]
        public ActionResult ViewPackages(string status, int id)
        {
            var oldp = context.Packages.FirstOrDefault(e => e.packageid == id);
            oldp.approvestatus = status;

            context.SaveChanges();
            return RedirectToAction("ViewPackages");
        }



        [ManagerAuthentication]
        [HttpPost]
        public ActionResult ViewSearchPackages(string searching)
        {
            var pack = (from e in context.Packages
                        where e.packagename.Contains(searching)
                        select e).ToList();
            return View(pack);
        }


        [ManagerAuthentication]
        public ActionResult EditProfile(int id)
        {

            var p = context.Users.FirstOrDefault(e => e.userid == id);
            return View(p);
        }

        [ManagerAuthentication]
        [HttpPost]
        public ActionResult EditProfile(User p, string ConfirmPassword)
        {
            var oldp = context.Users.FirstOrDefault(e => e.userid == p.userid);
            oldp.name = p.name;
            oldp.email = p.email;
            oldp.image = p.image;
            oldp.phone = p.phone;

            if (p.password == ConfirmPassword)
            {
                oldp.password = p.password;
            }

            context.SaveChanges();
            return RedirectToAction("/Index");
        }
    }
}