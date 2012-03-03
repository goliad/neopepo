using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PepoBebes.Models;

namespace PepoBebes.Controllers
{
    public class ScrollController : Controller
    {
        private Context db = new Context();

        //
        // GET: /Scroll/
        public ViewResult AgendaScroll()
        {
            return View(db.Agenda.ToList());
        }

        public ActionResult MadresScroll()
        {
            return View();
        }

        //-----------------------------------------------------
        // Codigo para el Data Table Madres en el scroll de agenda
        public ActionResult AjaxHandlerMadres(JQueryDataTableParamModel param)
        {
            var allMadres = db.Madres.ToList();
            IEnumerable<Madre> filteredMadres;
            //Check whether the madre should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);
                var dniFilter = Convert.ToString(Request["sSearch_2"]);
                var domicilioFilter = Convert.ToString(Request["sSearch_3"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isDniSearchable = Convert.ToBoolean(Request["bSearchable_2"]);
                var isDomicilioSearchable = Convert.ToBoolean(Request["bSearchable_3"]);

                filteredMadres = db.Madres.ToList().Where(m => isNameSearchable && m.nombre.ToLower().Contains(param.sSearch.ToLower())
                               ||
                               isDniSearchable && m.apellido.ToLower().Contains(param.sSearch.ToLower())
                               ||
                               isDomicilioSearchable && m.dni.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredMadres = allMadres;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isDniSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isDomicilioSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Madre, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.nombre :
                                                           sortColumnIndex == 2 && isDniSortable ? c.apellido :
                                                           sortColumnIndex == 3 && isDomicilioSortable ? c.dni :
                                                           "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredMadres = filteredMadres.OrderBy(orderingFunction);
            else
                filteredMadres = filteredMadres.OrderByDescending(orderingFunction);

            var displayedMadres = filteredMadres.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            var result = from c in displayedMadres select new[] { c.dni, c.apellido, c.nombre, c.madreID.ToString() };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allMadres.Count(),
                iTotalDisplayRecords = filteredMadres.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }


        //Agenda
        public ActionResult AjaxHandler(JQueryDataTableParamModel param)
        {
            //Filtra los eventos nuevos
            var allAgendas = db.Agenda.Where(a => a.Status.description == "Nuevo" && a.fecha < DateTime.Today && a.fecha > DateTime.Today.AddMonths(-1)).ToList();
            
            IEnumerable<Agenda> filteredAgendas;
            //Check whether the Agendas should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);
                var addressFilter = Convert.ToString(Request["sSearch_2"]);
                var townFilter = Convert.ToString(Request["sSearch_3"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isAddressSearchable = Convert.ToBoolean(Request["bSearchable_2"]);
                var isTownSearchable = Convert.ToBoolean(Request["bSearchable_3"]);

                filteredAgendas = db.Agenda.ToList().Where(m => isNameSearchable && m.agendaID.ToString().ToLower().Contains(param.sSearch.ToLower())
                               ||
                               isAddressSearchable && m.StatusID.ToString().ToLower().Contains(param.sSearch.ToLower())
                               ||
                               isTownSearchable && m.fecha.ToShortDateString().ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredAgendas = allAgendas;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isAddressSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isTownSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Agenda, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.agendaID.ToString() :
                                                           sortColumnIndex == 2 && isAddressSortable ? c.StatusID.ToString() :
                                                           sortColumnIndex == 3 && isTownSortable ? c.fecha.ToShortDateString() :
                                                           "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredAgendas = filteredAgendas.OrderBy(orderingFunction);
            else
                filteredAgendas = filteredAgendas.OrderByDescending(orderingFunction);

            var displayedAgendas = filteredAgendas.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in displayedAgendas select new[] { c.fecha.ToShortDateString(), c.bebe.madre.nombre, c.Status.description, c.agendaID.ToString(), c.agendaID.ToString() };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allAgendas.Count(),
                iTotalDisplayRecords = filteredAgendas.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

    }
}
