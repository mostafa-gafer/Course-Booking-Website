using Microsoft.AspNet.Identity;
using mvcIdentity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace mvcIdentity.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var userid = User.Identity.GetUserId();
                var user = db.Users.Where(n => n.Id == userid).SingleOrDefault();
                ViewBag.usertype = user.UserType;
            }

            return View(db.Topics.ToList());
        }
        [Authorize(Roles ="Instructor")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(Contact contact)
        {
            var mail = new MailMessage();
            var loginInfo = new NetworkCredential("mostafa.fathy85.mf@gmail.com", "password");
            mail.From = new MailAddress(contact.Email);
            mail.To.Add(new MailAddress("mostafa.fathy85.mf@gmail.com"));
            mail.Subject = contact.subject;
            mail.Body = contact.Message;
            var smtpclient = new SmtpClient("stmp.gmail.com", 587);
            smtpclient.EnableSsl = true;
            smtpclient.Credentials = loginInfo;
            smtpclient.Send(mail);
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [AllowAnonymous]
        public PartialViewResult displayCrs(int id)
        {
            var x = db.Courses.Where(n => n.Top_Id == id).ToList();
            return PartialView("_coursesPartial", x);
        }

       [Authorize]
        public ActionResult Apply(string Message)
        {
            var userId = User.Identity.GetUserId();
            var courseId = (int)Session["CourseId"];
            var check = db.ApplyForCourses.Where(n => n.CourseId == courseId && n.UserId == userId).ToList();
            if (check.Count < 1)
            {
                var course = new ApplyForCourse();
                course.CourseId = courseId;
                course.UserId = userId;
                course.ApplyDate = DateTime.Now;
                db.ApplyForCourses.Add(course);
                db.SaveChanges();
                ViewBag.result = "enroll Success ";
                return PartialView("_enrollPartial", ViewBag.result);
            }
            else
            {
                ViewBag.result = "enrolled before ";
                return PartialView("_enrollPartial", ViewBag.result);

            }
        }

        public ActionResult getCourseByUser()
        {
            var userId = User.Identity.GetUserId();
            
            return View(db.ApplyForCourses.Where(n => n.UserId == userId).ToList());
        }
        public ActionResult CourseDetials(int id)
        {
            var course = db.ApplyForCourses.Find(id);
            if(course == null)
            {
                ViewBag.erorr = "ERROR NOT FOUND";
                return PartialView("_CrsDetialsPartial", ViewBag.erorr);
            }
            return PartialView("_CrsDetialsPartial", course);

        }
        
        public ActionResult enrollDelete(int id)
        {
            var course = db.ApplyForCourses.Find(id);
            db.ApplyForCourses.Remove(course);
            db.SaveChanges();
            
            return RedirectToAction("getCourseByUser");

        }
        /**********instructor**********/
        public ActionResult getCoursesByIns()
        {
            var userId = User.Identity.GetUserId();
            var courses = from app in db.ApplyForCourses
                          join course in db.Courses
                          on app.CourseId equals course.Crs_Id
                          where course.user.Id == userId
                          select app;
            var grouped = from cour in courses
                          group cour by cour.course.Crs_Name
                         into gr
                          select new coursesViewModel
                          {
                              courseTitle = gr.Key,
                              items = gr
                          };
            var result = grouped.ToList();
            if (result.Count() == 0)
            {
                return RedirectToAction("Create", "Courses");
            }
            else
            {
                
                return View(result);

            }
        }
        /*******search******/
        [AllowAnonymous]
        [HttpPost]
        public ActionResult search(string searchname)
        {
            SearchModel _SearchModel = new SearchModel();
            //System.Threading.Thread.Sleep(2000);
            var result = db.Courses.Where(n => n.Crs_Name.Contains(searchname)
            || n.Crs_Description.Contains(searchname)
            || n.Topic.Top_Name.Contains(searchname)).ToList();

            _SearchModel.MyCourses = result;
            _SearchModel.MyInstructors = new List<ApplicationUser>();
            _SearchModel.MyTopics = new List<Topic>();

            return PartialView("_searchPartial", _SearchModel);



        }
        /*******Profile******/
        public ActionResult profile()
        {
            var userid = User.Identity.GetUserId();
            var user = db.Users.Where(n => n.Id == userid).SingleOrDefault();
            return View(user);

        }

    }
}