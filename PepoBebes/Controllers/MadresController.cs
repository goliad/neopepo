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

    public class MadresController : Controller
    {
        private Context db = new Context();

        //
        // GET: /Madres/

        [Authorize(Roles = "administrador ")] //No permite el acceso a carga de datos si no tiene el rol administrador
        public ViewResult Index()
        {
            var madres = db.Madres.Include(m => m.departamento);
            return View(madres.ToList());
        }

        //
        // GET: /Madres/Details/5

        public ViewResult Details(int id)
        {
            Madre madre = db.Madres.Find(id);
            return View(madre);
        }

        //
        // GET: /Madres/Create

        public ActionResult Create()
        {
            ViewBag.departamentoID = new SelectList(db.Departamentos, "departamentoID", "descripcion");
            return View();
        } 

        //
        // POST: /Madres/Create

        [HttpPost]
        public ActionResult Create(Madre madre)
        {
            if (ModelState.IsValid)
            {
                db.Madres.Add(madre);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.departamentoID = new SelectList(db.Departamentos, "departamentoID", "descripcion", madre.departamentoID);
            return View(madre);
        }
        
        //
        // GET: /Madres/Edit/5
 
        public ActionResult Edit(int id)
        {
            Madre madre = db.Madres.Find(id);
            ViewBag.departamentoID = new SelectList(db.Departamentos, "departamentoID", "descripcion", madre.departamentoID);
            return View(madre);
        }

        //
        // POST: /Madres/Edit/5

        [HttpPost]
        public ActionResult Edit(Madre madre)
        {
            if (ModelState.IsValid)
            {
                db.Entry(madre).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.departamentoID = new SelectList(db.Departamentos, "departamentoID", "descripcion", madre.departamentoID);
            return View(madre);
        }

        //
        // GET: /Madres/Delete/5
 
        public ActionResult Delete(int id)
        {
            Madre madre = db.Madres.Find(id);
            return View(madre);
        }

        //
        // POST: /Madres/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Madre madre = db.Madres.Find(id);
            db.Madres.Remove(madre);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        //Detalles de Bebes x Madre
        public ActionResult BebesMadre(int id)
        {
            var b = db.Bebes.Where(a => a.madreID.Equals(id));
            return View(b.ToList());
        }





        //CONSULTAS
        //
        //Consultas de madres
        public ViewResult Consultas()
        {
            var madres = db.Madres.Include(m => m.departamento);
            return View(madres.ToList());
        }



        public ActionResult BebesDepartamentos(JQueryDataTableParamModel param)
        {
            var allMadres = db.Bebes.ToList();
            IEnumerable<Bebe> filteredMadres;
            
            //Check whether the madre should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var dniFilter = Convert.ToString(Request["sSearch_1"]);
                var apellidoFilter = Convert.ToString(Request["sSearch_2"]);
                var nameFilter = Convert.ToString(Request["sSearch_3"]);

                //Optionally check whether the columns are searchable at all 
                var isDniSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isApellidoSearchable = Convert.ToBoolean(Request["bSearchable_2"]);
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_3"]);

                filteredMadres = db.Bebes.ToList().Where(m => isNameSearchable && m.nombre.ToLower().Contains(param.sSearch.ToLower())
                               ||
                               isDniSearchable && m.dni.ToLower().Contains(param.sSearch.ToLower())
                               ||
                               isApellidoSearchable && m.hc.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredMadres = allMadres;
            }

            var isDniSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isApellidoSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isNameSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Bebe, string> orderingFunction = (c => 
                                                           sortColumnIndex == 1 && isDniSortable ? c.madre.departamentoID.ToString() :
                                                           sortColumnIndex == 2 && isApellidoSortable ? c.madre.departamento.descripcion :
                                                           sortColumnIndex == 3 && isNameSortable ? c.nombre :
                                                           "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredMadres = filteredMadres.OrderBy(c => c.madre.departamentoID.ToString());
            else
                filteredMadres = filteredMadres.OrderByDescending(c => c.madre.departamentoID.ToString());

            //var displayedMadres = filteredMadres.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var displayedMadres = filteredMadres.Skip(param.iDisplayStart).Take(allMadres.Count);


            var result = from c in displayedMadres select new[] { c.madre.departamentoID.ToString(), c.madre.departamento.descripcion, c.madre.dni, c.dni,c.nombre, c.hc, c.fechaNacimiento.ToShortDateString()};
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allMadres.Count(),
                iTotalDisplayRecords = filteredMadres.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }
        //-------------------------------------
        //FIN CONSULTAS------------------------






        //-----------------------------------------------------
        // Codigo para el Data Table 
        public ActionResult AjaxHandler(JQueryDataTableParamModel param)
        {
            var allMadres = db.Madres.ToList();
            IEnumerable<Madre> filteredMadres;
            //Check whether the madre should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);
                var dniFilter = Convert.ToString(Request["sSearch_2"]);
                var apellidoFilter = Convert.ToString(Request["sSearch_3"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isDniSearchable = Convert.ToBoolean(Request["bSearchable_2"]);
                var isApellidoSearchable = Convert.ToBoolean(Request["bSearchable_3"]);

                filteredMadres = db.Madres.ToList().Where(m => isNameSearchable && m.nombre.ToLower().Contains(param.sSearch.ToLower())
                               ||
                               isDniSearchable && m.dni.ToLower().Contains(param.sSearch.ToLower())
                               ||
                               isApellidoSearchable && m.apellido.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredMadres = allMadres;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isDniSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isApellidoSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Madre, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.nombre :
                                                           sortColumnIndex == 2 && isDniSortable ? c.dni :
                                                           sortColumnIndex == 3 && isApellidoSortable ? c.apellido :
                                                           "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredMadres = filteredMadres.OrderBy(orderingFunction);
            else
                filteredMadres = filteredMadres.OrderByDescending(orderingFunction);

            var displayedMadres = filteredMadres.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            var result = from c in displayedMadres select new[] {c.madreID.ToString(),c.madreID.ToString(), c.dni, c.apellido, c.nombre,c.fechaNacimiento.ToShortDateString(),c.edad.ToString(),c.domicilio,c.localidad,c.departamento.descripcion,c.telefono, c.email };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allMadres.Count(),
                iTotalDisplayRecords = filteredMadres.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }
    }
}