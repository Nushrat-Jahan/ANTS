using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ANTS.Models;

namespace ANTS.Controllers
{
    public class LoginController : Controller
    {

        ANTSEntities context = new ANTSEntities();
        // GET: Login
        public ActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Index(User u, string returnUrl)
        {
            var usercheck = context.Users.FirstOrDefault(e => e.email == u.email);
            if (usercheck != null)
            {
                if (usercheck.password == u.password && u.password != null)
                {

                    FormsAuthentication.SetAuthCookie(usercheck.userid.ToString(), true);
                    Session["user_type"] = usercheck.usertype;
                    Session["name"] = usercheck.name;
                    Session["id"] = usercheck.userid;
                    //return Content(usercheck.usertype);
                    if (usercheck.usertype == "Admin")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (usercheck.usertype == "Manager")
                    {
                        return RedirectToAction("Index", "Manager");
                    }
                    else if (usercheck.usertype == "Seller")
                    {
                        return RedirectToAction("Index", "Seller");
                    }
                    else if (usercheck.usertype == "Customer")
                    {
                        return RedirectToAction("Index", "Customer");
                    }

                }
                TempData["ErrorMessage"] = "Incorrect Username/Password";
                return View();
            }
            TempData["ErrorMessage"] = "This email doesn't exist";
            return View();
        }

        public ActionResult Logout()
        {
            Session["user_type"] = null;
            Session["name"] = null;
            Session["id"] = null;
            FormsAuthentication.SignOut();
            return Redirect("/Home");
        }
    }
}