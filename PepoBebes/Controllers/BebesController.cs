using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PepoBebes.Models;
using System.IO;

namespace PepoBebes.Controllers
{ 
    public class BebesController : Controller
    {
        private Context db = new Context();

        //
        // GET: /Bebes/

        public ViewResult Index()
        {
            var bebes = db.Bebes.Include(b => b.sexo).Include(b => b.riesgo).Include(b => b.madre);
            return View(bebes.ToList());
        }

        // This action handles the form POST and the upload
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                //if (verficarExcel(file.FileName))
                //{
                // extract only the fielname
                var fileName = Path.GetFileName(file.FileName);
                ExcelImport excelImport = new ExcelImport();

                // store the file inside ~/App_Data/uploads folder
                string strExtension = Path.GetExtension(fileName);
                string strUploadFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + strExtension;
                var path = Path.Combine(Server.MapPath("~/UploadFiles"), strUploadFileName);
                file.SaveAs(path);

                String resul = Convert.ToString(excelImport.importToSQL(strExcelConn(path, strUploadFileName)));
                //}
            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("Index", "Establecimientos");
        }

        public string strExcelConn(string strPath, string strUploadFileName)
        {
            string strExtension = Path.GetExtension(strUploadFileName);
            if (strExtension == ".xls")
            {
                // Excel 97-2003
                return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strPath + ";Extended Properties='Excel 8.0;HDR=YES;'";
            }
            else
            {
                // Excel 2007
                return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strPath + ";Extended Properties='Excel 12.0 Xml;HDR=YES;'";
            }
        }

        public bool verficarExcel(string strFileName)
        {
            // Get the extension of the Excel spreadsheet.
            string strExtension = Path.GetExtension(strFileName);
            // Validate the file extension.
            if (strExtension != ".xls" && strExtension != ".xlsx")
            {
                return false;
            }
            return true;
        }


        //
        // GET: /Bebes/Details/5

        public ViewResult Details(int id)
        {
            Bebe bebe = db.Bebes.Find(id);
            return View(bebe);
        }

        //
        // GET: /Bebes/Create

        public ActionResult Create(int id)
        {
            Madre m = db.Madres.Find(id);
            Bebe b = new Bebe
            {
                madreID=m.madreID,
                madre=m
            };
            ViewBag.sexoID = new SelectList(db.Sexos, "sexoID", "descripcion");
            ViewBag.riesgoID = new SelectList(db.Riesgos, "riesgoID", "descripcion");
            ViewBag.madreID = new SelectList(db.Madres, "madreID", "dni");
            return View(b);
        }


        public void agendar18(Bebe bebe) {
            DateTime fechaAgenda = bebe.fechaNacimiento;
            for (int i = 0; i < 18; i++)
            {
                Agenda ag18 = new Agenda
                {
                    bebeID = bebe.bebeID,
                    fecha = fechaAgenda,
                    StatusID = 1
                };
                db.Agenda.Add(ag18);
                fechaAgenda=fechaAgenda.AddMonths(1);
            }
        }

        void guardarlog(Bebe bebe, string accion){
            Log_Bebe lb = new Log_Bebe
            {
                usuario = User.Identity.Name,
                fecha = DateTime.Now,
                idBebe = bebe.bebeID,
                accion = accion
            };
            db.Log_Bebes.Add(lb);
        }
        //
        // POST: /Bebes/Create

        [HttpPost]
        public ActionResult Create(Bebe bebe)
        {
            if (ModelState.IsValid)
            {
                agendar18(bebe);//Agendar los 18 eventos para el bebe

                db.Bebes.Add(bebe);

                db.SaveChanges();

                guardarlog(bebe, "crear");//Guarda el log del usuario
                db.SaveChanges();

                //Redirigir al index de Madres
                return RedirectToAction("Edit", "Bebes", new { id= bebe.bebeID});  
            }

            ViewBag.sexoID = new SelectList(db.Sexos, "sexoID", "descripcion", bebe.sexoID);
            ViewBag.riesgoID = new SelectList(db.Riesgos, "riesgoID", "descripcion", bebe.riesgoID);
            ViewBag.madreID = new SelectList(db.Madres, "madreID", "dni", bebe.madreID);
            return View(bebe);
        }
        
        //
        // GET: /Bebes/Edit/5
 
        public ActionResult Edit(int id)
        {
            Bebe bebe = db.Bebes.Find(id);
            ViewBag.sexoID = new SelectList(db.Sexos, "sexoID", "descripcion", bebe.sexoID);
            ViewBag.riesgoID = new SelectList(db.Riesgos, "riesgoID", "descripcion", bebe.riesgoID);
            ViewBag.madreID = new SelectList(db.Madres, "madreID", "dni", bebe.madreID);
            return View(bebe);
        }

        //
        // POST: /Bebes/Edit/5

        [HttpPost]
        public ActionResult Edit(Bebe bebe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bebe).State = EntityState.Modified;

                guardarlog(bebe, "editar");//Guarda el log del usuario

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.sexoID = new SelectList(db.Sexos, "sexoID", "descripcion", bebe.sexoID);
            ViewBag.riesgoID = new SelectList(db.Riesgos, "riesgoID", "descripcion", bebe.riesgoID);
            ViewBag.madreID = new SelectList(db.Madres, "madreID", "dni", bebe.madreID);
            return View(bebe);
        }

        //
        // GET: /Bebes/Delete/5
 
        public ActionResult Delete(int id)
        {
            Bebe bebe = db.Bebes.Find(id);
            return View(bebe);
        }

        //
        // POST: /Bebes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Bebe bebe = db.Bebes.Find(id);
            db.Bebes.Remove(bebe);

            guardarlog(bebe, "eliminar");//Guarda el log del usuario
            
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        //Detalles de eventos en la agenda para un Bebe
        public ActionResult AgendaBebe(int id)
        {
            var agenda = db.Agenda.Where(a => a.bebeID.Equals(id));
            return View(agenda.ToList());
        }

        public ActionResult HistorialNeoBebe(int id)
        {
            var historialNeo = db.HistorialNeo.Where(a => a.bebeID.Equals(id));
            return View(historialNeo.ToList());
        }




        //-----------------------------------------------------
        // Codigo para el Data Table 
        public ActionResult AjaxHandler(JQueryDataTableParamModel param)
        {
            var allBebes = db.Bebes.ToList();
            IEnumerable<Bebe> filteredBebes;
            //Check whether the Bebe should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);
                var dniFilter = Convert.ToString(Request["sSearch_2"]);
                var hcFilter = Convert.ToString(Request["sSearch_3"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isDniSearchable = Convert.ToBoolean(Request["bSearchable_2"]);
                var isHCSearchable = Convert.ToBoolean(Request["bSearchable_3"]);

                filteredBebes = db.Bebes.ToList().Where(b => isNameSearchable && b.nombre.ToLower().Contains(param.sSearch.ToLower())
                               ||
                               isDniSearchable && b.dni.ToLower().Contains(param.sSearch.ToLower())
                               ||
                               isHCSearchable && b.hc.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredBebes = allBebes;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isDniSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isHCSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Bebe, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.nombre :
                                                           sortColumnIndex == 2 && isDniSortable ? c.dni :
                                                           sortColumnIndex == 3 && isHCSortable ? c.hc :
                                                           "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredBebes = filteredBebes.OrderBy(orderingFunction);
            else
                filteredBebes = filteredBebes.OrderByDescending(orderingFunction);

            var displayedBebes = filteredBebes.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            var result = from c in displayedBebes select new[] { c.bebeID.ToString(), c.dni, c.nombre, c.fechaNacimiento.ToShortDateString(), c.sexo.descripcion, c.edadGestacional.ToString(), c.peso.ToString(), c.mamaCanguro, c.hc, c.riesgo.descripcion, c.madre.dni };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allBebes.Count(),
                iTotalDisplayRecords = filteredBebes.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }
    }
}