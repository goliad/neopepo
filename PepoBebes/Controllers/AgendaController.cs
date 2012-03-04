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
    public class AgendaController : Controller
    {
        private Context db = new Context();

        //
        // GET: /Agenda/

        public ActionResult Index()
        {
            DateTime semanaAntes = DateTime.Today.AddDays(-7);
            ViewBag.Message = db.Agenda.OrderBy(a => a.fecha).Where(a => a.Status.description == "Nuevo").Where(a => a.Status.description == "Nuevo" && (semanaAntes < a.fecha)&& (a.fecha<DateTime.Today)).ToList().Count.ToString();
            return View("Index");
        }

        //
        // GET: /Agenda/Details/5

        public ViewResult Details(int id)
        {
            Agenda agenda = db.Agenda.Find(id);
            return View(agenda);
        }

        //
        // GET: /Agenda/Create

        public ActionResult Create()
        {
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "description");
            ViewBag.bebeID = new SelectList(db.Bebes, "bebeID", "lugarNacimiento");
            return View();
        } 

        //
        // POST: /Agenda/Create

        [HttpPost]
        public ActionResult Create(Agenda agenda)
        {
            if (ModelState.IsValid)
            {
                db.Agenda.Add(agenda);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "description", agenda.StatusID);
            ViewBag.bebeID = new SelectList(db.Bebes, "bebeID", "lugarNacimiento", agenda.bebeID);
            return View(agenda);
        }
        
        //
        // GET: /Agenda/Edit/5
 
        public ActionResult Edit(int id)
        {
            Agenda agenda = db.Agenda.Find(id);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "description", agenda.StatusID);
            ViewBag.bebeID = new SelectList(db.Bebes, "bebeID", "lugarNacimiento", agenda.bebeID);
            return View(agenda);
        }


        //
        // POST: /Agenda/Edit/5

        [HttpPost]
        public ActionResult Edit(Agenda agenda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agenda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "description", agenda.StatusID);
            ViewBag.bebeID = new SelectList(db.Bebes, "bebeID", "lugarNacimiento", agenda.bebeID);
            return View(agenda);
        }

        //
        // GET: /Agenda/Delete/5
 
        public ActionResult Delete(int id)
        {
            Agenda agenda = db.Agenda.Find(id);
            return View(agenda);
        }

        //
        // POST: /Agenda/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Agenda agenda = db.Agenda.Find(id);
            db.Agenda.Remove(agenda);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        public ActionResult Completar(int id) {
            Agenda agenda = db.Agenda.Find(id);
            agenda.StatusID = 2;
            db.Entry(agenda).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Inicio", "Agenda");
        }


        public ActionResult rows(int idBebe)
        {
            var historial = from h in db.HistorialAgenda
                            select h;

            historial = historial.Where(h => h.bebeID.Equals(idBebe));

            return PartialView("rows", historial.ToList());
        }

        public ActionResult row()
        {
            //la ultima fila insertada
            int id = db.HistorialAgenda.Max(u => u.historialAgendaID);
            HistorialAgenda historial = db.HistorialAgenda.Find(id);
            return PartialView("row", historial);
        }


        public ActionResult VerMadre(int id)
        {
            Madre madre = db.Madres.Find(id);
            ViewBag.departamentoID = new SelectList(db.Departamentos, "departamentoID", "descripcion", madre.departamentoID);
            return View(madre);
        }

        public ActionResult Inicio()
        {
            DateTime semanaAntes = DateTime.Today.AddDays(-7);
            ViewBag.Message = db.Agenda.OrderBy(a => a.fecha).Where(a => a.Status.description == "Nuevo").Where(a => a.Status.description == "Nuevo" && (semanaAntes < a.fecha) && (a.fecha < DateTime.Today)).ToList().Count.ToString();
            return PartialView("Inicio");
        }


    }
}