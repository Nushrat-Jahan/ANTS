using ANTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ANTS.Authentication;
using System.Collections;
using System.Dynamic;
using System.Web.Routing;
using System.Data.Entity.SqlServer;

namespace ANTS.Controllers
{
    [Authorize]
    [AdminAuthentication]
    public class AdminController : Controller
    {
        ANTSEntities context = new ANTSEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewUsers()
        {
            var users = context.Users.ToList();
            return View(users);
        }
        [HttpPost]
        public ActionResult ViewUsers(string searchtext)
        {
            var users = context.Users.Where(x => x.name.Contains(searchtext)).ToList();
            return View(users);
        }

        //[AdminAuthentication]
        public ActionResult CreateManager()
        {
            User z = new User();
            z.createdat = DateTime.Now;
            return View(z);
        }

        [HttpPost]
        public ActionResult CreateManager(User u, String confirmpassword)
        {
            if (ModelState.IsValid)
            {
                if (u.password != confirmpassword)
                {
                    ViewBag.match = "Password did not match";
                    User p = new User();
                    p.createdat = DateTime.Now;
                    return View(p);
                }
                var pr = context.Users.FirstOrDefault(e => e.email == u.email);
                if (pr == null)
                {
                    context.Users.Add(u);
                    context.SaveChanges();
                    return RedirectToAction("ViewUsers");
                }
                else
                {
                    ViewBag.UniqueEmail = "This email exists";
                    User z = new User();
                    z.createdat = DateTime.Now;
                    return View(z);
                }
            }
            User x = new User();
            x.createdat = DateTime.Now;
            return View(x);
        }

        public ActionResult EditUser(int id)
        {
            var user = context.Users.FirstOrDefault(e => e.userid == id);
            return View(user);
        }

        [HttpPost]
        public ActionResult EditUser(User u)
        {
            var user = context.Users.FirstOrDefault(e => e.userid == u.userid);
            context.Entry(user).CurrentValues.SetValues(u);
            context.SaveChanges();
            return RedirectToAction("ViewUsers");
        }

        public ActionResult DeleteUser(int id)
        {
            var user = context.Users.FirstOrDefault(e => e.userid == id);
            return View(user);
        }

        [HttpPost]
        [ActionName("DeleteUser")]
        public ActionResult DeleteUserU(int id)
        {
            var user = context.Users.FirstOrDefault(e => e.userid == id);
            context.Users.Remove(user);
            context.SaveChanges();
            return RedirectToAction("ViewUsers");
        }

        public ActionResult CreateNotice()
        {
            Notice n = new Notice();
            //CHANGE WITH AUTHKEY
            n.userid = Convert.ToInt32(Session["id"]);
            n.createdat = DateTime.Now;
            n.status = "Active";
            return View(n);
        }

        [HttpPost]
        public ActionResult CreateNotice(Notice n)
        {
            if (ModelState.IsValid)
            {
                context.Notices.Add(n);
                context.SaveChanges();
                return RedirectToAction("ViewNotices");
            }
            return View();
        }

        public ActionResult ViewNotices()
        {
            var notices = context.Notices.ToList();
            var users = context.Users.ToList();

            var noticesAndNames = notices.Join(users,
                noticekey => noticekey.userid,
                namekey => namekey.userid,
                (noticekey, namekey) => new
                {
                    noticeid = noticekey.noticeid,
                    name = namekey.name,
                    usertype = noticekey.usertype,
                    notice = noticekey.notice1,
                    createdat = noticekey.createdat,
                    status = noticekey.status
                });
            //List<Notice> Lists = new List<Notice>();
            //foreach (var item in noticesAndNames)
            //{
            //    var x = new Notice();
            //    x.noticeid = item.noticeid;
            //    x.User = new User();
            //    x.User.name = item.name;
            //    x.usertype = item.usertype;
            //    x.notice1 = item.notice;
            //    x.createdat = item.createdat;
            //    x.status = item.status;
            //    Lists.Add(x);
            //}
            //return View(Lists);

            //var noticesAndNames = (from n in notices
            //                       join u in users on n.userid equals u.userid
            //                       select new
            //                       {
            //                           noticeid = n.noticeid,
            //                           name = u.name,
            //                           usertype = n.usertype,
            //                           notice = n.notice1,
            //                           createdat = n.createdat,
            //                           status = n.status
            //                       }).ToList();
            //ViewBag.myData = noticesAndNames;

            return View(noticesAndNames);
        }

        [HttpPost]
        public ActionResult ViewNotices(string searchtext)
        {
            var notices = context.Notices.ToList();
            var users = context.Users.ToList();

            var noticesAndNames = notices.Where(x => x.notice1.Contains(searchtext)).Join(users,
                noticekey => noticekey.userid,
                namekey => namekey.userid,
                (noticekey, namekey) => new
                {
                    noticeid = noticekey.noticeid,
                    name = namekey.name,
                    usertype = noticekey.usertype,
                    notice = noticekey.notice1,
                    createdat = noticekey.createdat,
                    status = noticekey.status
                });

            return View(noticesAndNames);
        }

        public ActionResult EditNotice(int id)
        {
            var notice = context.Notices.FirstOrDefault(e => e.noticeid == id);
            return View(notice);
        }

        [HttpPost]
        public ActionResult EditNotice(Notice n)
        {
            var notice = context.Notices.FirstOrDefault(e => e.noticeid == n.noticeid);
            context.Entry(notice).CurrentValues.SetValues(n);
            context.SaveChanges();
            return RedirectToAction("ViewNotices");
        }

        public ActionResult DeleteNotice(int id)
        {
            var notice = context.Notices.FirstOrDefault(e => e.noticeid == id);
            return View(notice);
        }

        [HttpPost]
        [ActionName("DeleteNotice")]
        public ActionResult DeleteNoticeN(int id)
        {
            var notice = context.Notices.FirstOrDefault(e => e.noticeid == id);
            context.Notices.Remove(notice);
            context.SaveChanges();
            return RedirectToAction("ViewNotices");
        }

        public ActionResult Dashboard()
        {
            var prevMonth = DateTime.Now.AddMonths(-1);
            var nextMonth = DateTime.Now.AddMonths(1);

            var last30DaysIncome = context.Orders.Where(x => x.status.Equals("sold") && x.createdat > prevMonth && x.createdat < nextMonth).Select(x => x.totalprice).Sum();

            var totalIncome = context.Orders.Where(x => x.status.Equals("sold")).Select(x => x.totalprice).Sum();

            var currentmonth = DateTime.Now.Month;
            var monthprice = (from p in context.Orders
                              where p.status == "sold"
                              where p.createdat.Month == currentmonth
                              select (p.totalprice));
            var monthlyIncome = monthprice.Sum();

            ViewBag.last30DaysIncome = last30DaysIncome;
            ViewBag.totalIncome = totalIncome;
            ViewBag.monthlyIncome = monthlyIncome;

            return View();
        }

        public ActionResult ViewComplains()
        {
            var modifiedComplains = from r in context.Ratings
                                    where r.complain!=null
                                    join u in context.Users on r.userid equals u.userid
                                    join p in context.Packages on r.packageid equals p.packageid
                                    select new
                                    {
                                        ratingid = r.ratingid,
                                        userid = r.userid,
                                        name = u.name,
                                        packageid = r.packageid,
                                        packagename = p.packagename,
                                        rating = r.rating1,
                                        complain = r.complain,
                                        complainstatus = r.complainstatus
                                        
                                    };
            return View(modifiedComplains);
        }

        public ActionResult EditComplain(int id)
        {
            var complain = context.Ratings.FirstOrDefault(e => e.ratingid == id);
            return View(complain);
        }


        [HttpPost]
        public ActionResult EditComplain(Rating r)
        {
            var rating = context.Ratings.FirstOrDefault(e => e.ratingid == r.ratingid);
            context.Entry(rating).CurrentValues.SetValues(r);
            context.SaveChanges();
            return RedirectToAction("ViewComplains");
        }

        public ActionResult DeleteComplain(int id)
        {
            var rating = context.Ratings.FirstOrDefault(e => e.ratingid == id);
            return View(rating);
        }

        [HttpPost]
        [ActionName("DeleteComplain")]
        public ActionResult DeleteComplainC(int id)
        {
            var rating = context.Ratings.FirstOrDefault(e => e.ratingid == id);
            context.Ratings.Remove(rating);
            context.SaveChanges();
            return RedirectToAction("ViewComplains");
        }

        public ActionResult AdminAction()
        {
            var users = context.Users.ToList();
            return View(users);
        }

        [HttpPost]
        public ActionResult AdminAction(string status, string submitButton, int id = 1, string searchText="")
        {
            if (submitButton == "Search")
            {

                var users = context.Users.Where(x => x.name.Contains(searchText)).ToList();
                return View(users);
            }
            else
            {
                var oldUser = context.Users.FirstOrDefault(e => e.userid == id);
                if (oldUser.status != status)
                {
                    int statusId = 1;
                    if (status == "Valid")
                    {
                        statusId = 1;
                    }
                    else if (status == "Invalid")
                    {
                        statusId = 2;
                    }
                    else if (status == "Banned")
                    {
                        statusId = 3;
                    }
                    var log = new Auditlog
                    {
                        adminid = Convert.ToInt32(Session["id"]),
                        userid = id,
                        createdat = DateTime.Now,
                        details = "Apatoto Thaklo",
                        actiontypeid = statusId
                    };
                    context.Auditlogs.Add(log);
                    context.SaveChanges();
                }
                oldUser.status = status;
                context.SaveChanges();
                return RedirectToAction("AdminAction");
            }
        }

        public ActionResult ViewVouchers()
        {
            var vouchersWithName = from v in context.Vouchers
                                   join u in context.Users on v.userid equals u.userid
                                   select new
                                   {
                                       voucherId = v.voucherid,
                                       voucherStatus = v.voucherstatus,
                                       voucher = v.voucher1,
                                       userId = v.userid,
                                       name = u.name
                                   };
            return View(vouchersWithName);
        }

        [HttpPost]
        public ActionResult ViewVouchers(string searchText)
        {
            var vouchersWithName = from v in context.Vouchers
                                   where v.voucher1.Contains(searchText)
                                   join u in context.Users on v.userid equals u.userid
                                   select new
                                   {
                                       voucherId = v.voucherid,
                                       voucherStatus = v.voucherstatus,
                                       voucher = v.voucher1,
                                       userId = v.userid,
                                       name = u.name
                                   };
            return View(vouchersWithName);
        }

        public ActionResult EditVoucher(int id)
        {
            var voucher = context.Vouchers.FirstOrDefault(e => e.voucherid == id);
            return View(voucher);
        }


        [HttpPost]
        public ActionResult EditVoucher(Voucher v)
        {
            var voucher = context.Vouchers.FirstOrDefault(e => e.voucherid == v.voucherid);
            context.Entry(voucher).CurrentValues.SetValues(v);
            context.SaveChanges();
            return RedirectToAction("ViewVouchers");
        }

        public ActionResult DeleteVoucher(int id)
        {
            var voucher = context.Vouchers.FirstOrDefault(e => e.voucherid == id);
            return View(voucher);
        }

        [HttpPost]
        [ActionName("DeleteVoucher")]
        public ActionResult DeleteVoucherV(int id)
        {
            var voucher = context.Vouchers.FirstOrDefault(e => e.voucherid == id);
            context.Vouchers.Remove(voucher);
            context.SaveChanges();
            return RedirectToAction("ViewVouchers");
        }

        public ActionResult CreateVoucher()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateVoucher(Voucher v)
        {
            context.Vouchers.Add(v);
            context.SaveChanges();
            return RedirectToAction("ViewVouchers");
        }

        public ActionResult ViewAuditLogs()
        {
            var auditlogs = from a in context.Auditlogs
            join adminu in context.Users on a.adminid equals adminu.userid
            join u in context.Users on a.userid equals u.userid
            join action in context.Actions on a.actiontypeid equals action.actionid
            select new
            {
                auditLogId = a.auditlogid,
                adminId = a.adminid,
                adminName = adminu.name,
                userid = a.userid,
                username = u.name,
                createdat = a.createdat,
                details = a.details,
                actiontypeid = a.actiontypeid,
                actionname = action.actionanme
            };
            return View(auditlogs);
        }
    }
}