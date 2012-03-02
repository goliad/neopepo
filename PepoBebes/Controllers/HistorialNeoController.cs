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
    public class HistorialNeoController : Controller
    {
        private Context db = new Context();

        //
        // GET: /HistorialNeo/

        public ViewResult Index()
        {
            var historialneo = db.HistorialNeo.Include(h => h.bebe);
            return View(historialneo.ToList());
        }

        //
        // GET: /HistorialNeo/Details/5

        public ViewResult Details(int id)
        {
            HistorialNeo historialneo = db.HistorialNeo.Find(id);
            return View(historialneo);
        }

        //
        // GET: /HistorialNeo/Create

        public ActionResult Create()
        {
            ViewBag.bebeID = new SelectList(db.Bebes, "bebeID", "dni");
            return View();
        } 

        //
        // POST: /HistorialNeo/Create

        [HttpPost]
        public ActionResult Create(HistorialNeo historialneo)
        {
            if (ModelState.IsValid)
            {
                db.HistorialNeo.Add(historialneo);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.bebeID = new SelectList(db.Bebes, "bebeID", "dni", historialneo.bebeID);
            return View(historialneo);
        }
        
        //
        // GET: /HistorialNeo/Edit/5
 
        public ActionResult Edit(int id)
        {
            HistorialNeo historialneo = db.HistorialNeo.Find(id);
            ViewBag.bebeID = new SelectList(db.Bebes, "bebeID", "dni", historialneo.bebeID);
            return View(historialneo);
        }

        //
        // POST: /HistorialNeo/Edit/5

        [HttpPost]
        public ActionResult Edit(HistorialNeo historialneo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(historialneo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.bebeID = new SelectList(db.Bebes, "bebeID", "dni", historialneo.bebeID);
            return View(historialneo);
        }

        //
        // GET: /HistorialNeo/Delete/5
 
        public ActionResult Delete(int id)
        {
            HistorialNeo historialneo = db.HistorialNeo.Find(id);
            return View(historialneo);
        }

        //
        // POST: /HistorialNeo/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            HistorialNeo historialneo = db.HistorialNeo.Find(id);
            db.HistorialNeo.Remove(historialneo);
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