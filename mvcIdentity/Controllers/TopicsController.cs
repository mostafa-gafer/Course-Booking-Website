using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mvcIdentity.Models;

namespace mvcIdentity.Controllers
{
    [Authorize(Roles = "Admins")]
    public class TopicsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Topics
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Topics.ToList());
        }

        // GET: Topics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // GET: Topics/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Topic topic, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                if (photo != null)
                {
                    //upload photo on server folder
                    photo.SaveAs(Server.MapPath($"~/Attachs/Topic/{photo.FileName}"));
                    //save path in student object
                    topic.Top_Photo= photo.FileName;
                }
                db.Topics.Add(topic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(topic);
        }

        // GET: Topics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Topic topic, HttpPostedFileBase photo)
        {
            
            if (ModelState.IsValid)
            {
                if (photo != null)
                {
                    try
                    {
                        string oldpathphtoto = Server.MapPath($"~/Attachs/Topic/{topic.Top_Photo}");
                        //to delete old path
                        System.IO.File.Delete(oldpathphtoto);
                    }
                    catch
                    { 
                    //upload photo on server folder
                    photo.SaveAs(Server.MapPath($"~/Attachs/Topic/{photo.FileName}"));
                    //save path in student object
                    topic.Top_Photo = photo.FileName;
                    }
                }
                db.Entry(topic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(topic);
        }

        // GET: Topics/Delete/5
        public ActionResult Delete(int? id)
        {
            Topic topic = db.Topics.Find(id);
            db.Topics.Remove(topic);
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
