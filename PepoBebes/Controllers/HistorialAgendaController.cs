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
    public class HistorialAgendaController : Controller
    {
        private Context db = new Context();

        //
        // GET: /HistorialAgenda/

        public ViewResult Index()
        {
            var historial = db.HistorialAgenda.Include(h => h.bebe);
            return View(historial.ToList());
        }

        //
        // GET: /HistorialAgenda/Details/5

        public ViewResult Details(int id)
        {
            HistorialAgenda historialagenda = db.HistorialAgenda.Find(id);
            return View(historialagenda);
        }

        //
        // GET: /HistorialAgenda/Create

        public ActionResult Create(int idBebe)
        {
            Bebe bebe = db.Bebes.Find(idBebe);
            HistorialAgenda historial = new HistorialAgenda()
            {
                bebeID = bebe.bebeID,
                bebe = bebe,
                fecha = DateTime.Today
            };

            //ViewBag.bebeID = new SelectList(db.Bebes, "bebeID", "lugarNacimiento");
            return PartialView(historial);
        }

        //
        // POST: /Historial/Create

        [HttpPost]
        public ActionResult Create(HistorialAgenda historial)
        {
            if (ModelState.IsValid)
            {
                db.HistorialAgenda.Add(historial);
                db.SaveChanges();
                return Content(Boolean.TrueString);
            }

            return PartialView(historial);
        }
        
        //
        // GET: /HistorialAgenda/Edit/5
 
        public ActionResult Edit(int id)
        {
            HistorialAgenda historialagenda = db.HistorialAgenda.Find(id);
            ViewBag.bebeID = new SelectList(db.Bebes, "bebeID", "dni", historialagenda.bebeID);
            return View(historialagenda);
        }

        //
        // POST: /HistorialAgenda/Edit/5

        [HttpPost]
        public ActionResult Edit(HistorialAgenda historialagenda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(historialagenda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.bebeID = new SelectList(db.Bebes, "bebeID", "dni", historialagenda.bebeID);
            return View(historialagenda);
        }

        //
        // GET: /HistorialAgenda/Delete/5
 
        public ActionResult Delete(int id)
        {
            HistorialAgenda historialagenda = db.HistorialAgenda.Find(id);
            return View(historialagenda);
        }

        //
        // POST: /HistorialAgenda/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            HistorialAgenda historialagenda = db.HistorialAgenda.Find(id);
            db.HistorialAgenda.Remove(historialagenda);
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