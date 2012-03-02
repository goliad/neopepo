using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PepoBebes.Models;

namespace PepoBebes.Controllers
{ 
    public class LogBebesController : Controller
    {
        private Context db = new Context();

        //
        // GET: /LogBebes/

        public ViewResult Index()
        {
            return View(db.Log_Bebes.ToList());
        }

        //
        // GET: /LogBebes/Details/5

        public ViewResult Details(int id)
        {
            Log_Bebe log_bebe = db.Log_Bebes.Find(id);
            return View(log_bebe);
        }

        //
        // GET: /LogBebes/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /LogBebes/Create

        [HttpPost]
        public ActionResult Create(Log_Bebe log_bebe)
        {
            if (ModelState.IsValid)
            {
                db.Log_Bebes.Add(log_bebe);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(log_bebe);
        }
        
        //
        // GET: /LogBebes/Edit/5
 
        public ActionResult Edit(int id)
        {
            Log_Bebe log_bebe = db.Log_Bebes.Find(id);
            return View(log_bebe);
        }

        //
        // POST: /LogBebes/Edit/5

        [HttpPost]
        public ActionResult Edit(Log_Bebe log_bebe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(log_bebe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(log_bebe);
        }

        //
        // GET: /LogBebes/Delete/5
 
        public ActionResult Delete(int id)
        {
            Log_Bebe log_bebe = db.Log_Bebes.Find(id);
            return View(log_bebe);
        }

        //
        // POST: /LogBebes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Log_Bebe log_bebe = db.Log_Bebes.Find(id);
            db.Log_Bebes.Remove(log_bebe);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}