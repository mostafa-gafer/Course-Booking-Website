using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using mvcIdentity.Models;

namespace mvcIdentity.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Courses
        [AllowAnonymous]
        public ActionResult Index()
        {
            var courses = db.Courses.Include(c => c.Topic);

            if (Request.IsAuthenticated)
            {
                var userid = User.Identity.GetUserId();
                var user = db.Users.Where(n => n.Id == userid).SingleOrDefault();
                ViewBag.usertype = user.UserType;
            }

            return View(courses.ToList());
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Where(n => n.Crs_Id == id).SingleOrDefault();
            if (course == null)
            {
                return HttpNotFound();
            }
            Session["CourseId"] = course.Crs_Id;
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            ViewBag.Top_Id = new SelectList(db.Topics, "Top_Id", "Top_Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course course, HttpPostedFileBase photo)
        {
            
            if (ModelState.IsValid)
            {
                if (photo != null)
                {
                    //upload photo on server folder
                    photo.SaveAs(Server.MapPath($"~/Attachs/Course/{photo.FileName}"));
                    //save path in student object
                    course.Crs_Photo = photo.FileName;
                }
                course.UserId = User.Identity.GetUserId();
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Top_Id = new SelectList(db.Topics, "Top_Id", "Top_Name", course.Top_Id);
            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.Top_Id = new SelectList(db.Topics, "Top_Id", "Top_Name", course.Top_Id);
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course course , HttpPostedFileBase photo)
        {
            
            
            
            if (ModelState.IsValid)
            {
                if (photo != null)
                {
                    
                    try
                    {
                        string oldpathphtoto = Server.MapPath($"~/Attachs/Course/{course.Crs_Photo}");
                        //to delete old path
                        System.IO.File.Delete(oldpathphtoto);
                        //upload photo on server folder
                        photo.SaveAs(Server.MapPath($"~/Attachs/Course/{photo.FileName}"));
                        //save path in student object
                        course.Crs_Photo = photo.FileName;
                    }
                    catch
                    {
                        //upload photo on server folder
                        photo.SaveAs(Server.MapPath($"~/Attachs/Course/{photo.FileName}"));
                        //save path in student object
                        course.Crs_Photo = photo.FileName;
                    }
                    
                    
                }
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Top_Id = new SelectList(db.Topics, "Top_Id", "Top_Name", course.Top_Id);
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
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
