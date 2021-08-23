using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ANTS.Models;
using ANTS.Authentication;

namespace ANTS.Controllers
{
    public class RegistrationController : Controller
    {
        ANTSEntities context = new ANTSEntities();
        public ActionResult Create()
        {
            User u = new User();
            u.createdat = DateTime.Now;
            u.status = "Valid";
            return View(u);
        }
        [HttpPost]
        public ActionResult Create(User u, String ConfirmPassword)
        {
            if (ModelState.IsValid)
            {
                if (u.password != ConfirmPassword)
                {
                    ViewBag.match = "Password did not match";
                    User p = new User();
                    p.createdat = DateTime.Now;
                    p.status = "Valid";
                    return View(p);
                }
                var pr = context.Users.FirstOrDefault(e => e.email == u.email);
                if(pr== null)
                {
                    context.Users.Add(u);
                    context.SaveChanges();
                    TempData["ErrorMessage"] = "Sign up successful,please login!";
                    return RedirectToAction("../Login/Index");
                }
                else
                {
                    ViewBag.email = "This email exists";
                    User z = new User();
                    z.createdat = DateTime.Now;
                    z.status = "Valid";
                    return View(z);
                }

            }
            User x = new User();
            x.createdat = DateTime.Now;
            x.status = "Valid";
            return View(x);
        }
    }
}