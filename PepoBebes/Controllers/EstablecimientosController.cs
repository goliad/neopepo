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
    public class EstablecimientosController : Controller
    {
        private Context db = new Context();

        //
        // GET: /Establecimientos/

        public ViewResult Index()
        {
            var establecimientos = db.Establecimientos.Include(e => e.departamento);
            return View(establecimientos.ToList());
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
                    ExcelImportEstab excelImport = new ExcelImportEstab();
                    
                    // store the file inside ~/App_Data/uploads folder
                    string strExtension = Path.GetExtension(fileName);
                string strUploadFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + strExtension ;
                    var path = Path.Combine(Server.MapPath("~/UploadFiles"),strUploadFileName );
                    file.SaveAs(path);

                    String resul = Convert.ToString(excelImport.importToSQL(strExcelConn(path, strUploadFileName)));
                //}
            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("Index","Establecimientos");
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
        // GET: /Establecimientos/Details/5

        public ViewResult Details(int id)
        {
            Establecimientos establecimientos = db.Establecimientos.Find(id);
            return View(establecimientos);
        }

        //
        // GET: /Establecimientos/Create

        public ActionResult Create()
        {
            ViewBag.departamentoID = new SelectList(db.Departamentos, "departamentoID", "descripcion");
            return View();
        } 

        //
        // POST: /Establecimientos/Create

        [HttpPost]
        public ActionResult Create(Establecimientos establecimientos)
        {
            if (ModelState.IsValid)
            {
                db.Establecimientos.Add(establecimientos);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.departamentoID = new SelectList(db.Departamentos, "departamentoID", "descripcion", establecimientos.departamentoID);
            return View(establecimientos);
        }
        
        //
        // GET: /Establecimientos/Edit/5
 
        public ActionResult Edit(int id)
        {
            Establecimientos establecimientos = db.Establecimientos.Find(id);
            ViewBag.departamentoID = new SelectList(db.Departamentos, "departamentoID", "descripcion", establecimientos.departamentoID);
            return View(establecimientos);
        }

        //
        // POST: /Establecimientos/Edit/5

        [HttpPost]
        public ActionResult Edit(Establecimientos establecimientos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(establecimientos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.departamentoID = new SelectList(db.Departamentos, "departamentoID", "descripcion", establecimientos.departamentoID);
            return View(establecimientos);
        }

        //
        // GET: /Establecimientos/Delete/5
 
        public ActionResult Delete(int id)
        {
            Establecimientos establecimientos = db.Establecimientos.Find(id);
            return View(establecimientos);
        }

        //
        // POST: /Establecimientos/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Establecimientos establecimientos = db.Establecimientos.Find(id);
            db.Establecimientos.Remove(establecimientos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult EstablecimientosDpto(int id) {
            var establecimientos = db.Establecimientos.Include(e => e.departamento).Where(e => e.departamentoID==id);
            return View(establecimientos.ToList());
        }
    }
}