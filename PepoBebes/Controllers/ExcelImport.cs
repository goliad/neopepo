using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using PepoBebes.Models;

namespace PepoBebes
{
    public class ExcelImport
    {
        private Context db = new Context();

        // Get the row counts in SQL Server table.
        public int GetRowCounts()
        {
            int iRowCount = 0;

            iRowCount = db.Bebes.Count();

            return iRowCount;
        }

        // Retrieve data from the Excel spreadsheet.
        public DataTable RetrieveData(string strConn)
        {
            DataTable dtExcel = new DataTable();

            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                // Initialize an OleDbDataAdapter object.
                OleDbDataAdapter da = new OleDbDataAdapter("select * from  [bebes$] ", conn);

                // Fill the DataTable with data from the Excel spreadsheet.
                da.Fill(dtExcel);
            }

            return dtExcel;
        }


        public int importToSQL(string strExcelConn)
        {
            DataTable dtExcel = RetrieveData(strExcelConn);
            int iStartCount = GetRowCounts();
                int cantFilasExcel = dtExcel.Rows.Count;
                for (int i = 0; i < cantFilasExcel; i++)
                {
                    Madre m = new Madre
                    {
                        dni = Convert.ToString(dtExcel.Rows[i]["DNI"]),
                        apellido =Convert.ToString(dtExcel.Rows[i]["Apellido"]),
                        nombre = Convert.ToString(dtExcel.Rows[i]["Nombre"]),
                        fechaNacimiento = refineFNac(dtExcel.Rows[i]["Edad"]),
                        edad = Convert.ToInt32(dtExcel.Rows[i]["Edad"]),
                        domicilio = Convert.ToString(dtExcel.Rows[i]["domicilio"]),
                        localidad = Convert.ToString(dtExcel.Rows[i]["localidad"]),
                        departamento = buscarDepartamento((string)dtExcel.Rows[i]["departamento"]),
                        email = "(sin dato)",
                        telefono=Convert.ToString(dtExcel.Rows[i]["telefono"])
                    };
                    db.Madres.Add(m);
                    Bebe b = new Bebe
                    {
                        madreID = m.madreID,
                        madre = m,
                        fechaNacimiento = Convert.ToDateTime(dtExcel.Rows[i]["fechaNacimiento"]),
                        dni="(sin dato)",
                        vive="Si",
                        nombre = Convert.ToString(dtExcel.Rows[i]["bebe"]),
                        sexo = buscarSexo(Convert.ToString(dtExcel.Rows[i]["sexo"])),
                        edadGestacional = Convert.ToInt32(dtExcel.Rows[i]["EG"]),
                        peso = Convert.ToInt32(dtExcel.Rows[i]["peso"]),
                        hc = Convert.ToString(dtExcel.Rows[i]["hc"]),
                        mamaCanguro = Convert.ToString(dtExcel.Rows[i]["mamaCanguro"]),
                        riesgo = db.Riesgos.Find(1)
                    }; 
                    db.Bebes.Add(b);
                    HistorialNeo h = new HistorialNeo
                    {
                        fecha = DateTime.Today,
                        tipo = "Ingreso",
                        fechaIngreso = Convert.ToDateTime(dtExcel.Rows[i]["fechaIngreso"]),
                        pesoNeo = Convert.ToInt32(dtExcel.Rows[i]["peso"]),
                        lugarNacimiento = Convert.ToString(dtExcel.Rows[i]["lugarNacimiento"]),
                        derivacion = Convert.ToString(dtExcel.Rows[i]["derivacion"]),
                        medicoReceptor = "(sin dato)",
                        diagnostico = Convert.ToString(dtExcel.Rows[i]["diagnostico"]),
                        fechaEgreso = DateTime.Today,
                        medicoAlta = "(sin dato)",
                        responsable = Convert.ToString(dtExcel.Rows[i]["Alta"]),
                        observaciones = Convert.ToString(dtExcel.Rows[i]["observaciones"]),
                        bebe=b
                        
                    };
                    
                    db.HistorialNeo.Add(h);
                    agendar18(b);//Agendar los 18 eventos para el bebe
                    db.SaveChanges();
                    guardarlog(b, "crear");//Guarda el log del usuario
                    db.SaveChanges();
            }

            // Get the row counts after importing.
            int iEndCount = GetRowCounts();

            return iEndCount - iStartCount;
        }

        void guardarlog(Bebe bebe, string accion)
        {
            Log_Bebe lb = new Log_Bebe
            {
                usuario = "excelImport",
                fecha = DateTime.Now,
                idBebe = bebe.bebeID,
                accion = accion
            };
            db.Log_Bebes.Add(lb);
        }

        public void agendar18(Bebe bebe)
        {
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
                fechaAgenda = fechaAgenda.AddMonths(1);
            }
        }
        private object refinePeso(object p)
        {
            if (String.IsNullOrEmpty(p.ToString()))
            {
                return "0";
            }
            return p;
        }
        private object refineEG(object eg)
        {
            if (String.IsNullOrEmpty(eg.ToString()))
            {
                return "0";
            }
            return eg;
        }

        private DateTime refineFNac(object d)
        {
            if (String.IsNullOrEmpty(d.ToString()))
            {
                return DateTime.Today;
            }
            int a = 0;
            a = int.Parse(d.ToString());
            DateTime fNac = new DateTime();
            a = a + 1;
            fNac= DateTime.Now.AddYears(-a);// la planilla es de 2011 por eso el +1 de a
            return fNac;
        }

        private object refineEdad(object d)
        {
            if (String.IsNullOrEmpty(d.ToString()))
            {
                return 0;
            }
            return d.ToString();
        }

        private Departamentos buscarDepartamento(string strDepartamento) {
            var dep = db.Departamentos.ToArray();            
            for (int i = 0; i < dep.Length; i++)
            {
                if (dep[i].descripcion == strDepartamento)
                {
                    return dep[i];
                }
            }
            return db.Departamentos.Find(1);
        }

        private Sexos buscarSexo(string strSexo)
        {
            var s = db.Sexos.ToArray();
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].descripcion == strSexo)
                {
                    return s[i];
                }
            }
            return db.Sexos.Find(3);
        }

        private Riesgos buscarRiesgo(string strRiesgo)
        {
            var r = db.Riesgos.ToArray();
            for (int i = 0; i < r.Length; i++)
            {
                if (r[i].descripcion == strRiesgo)
                {
                    return r[i];
                }
            }
            return db.Riesgos.Find(1);
        }


        private object refineBarrio(object b)
        {
            if (String.IsNullOrEmpty(b.ToString()))
            {
                return "";
            }
            return "B°: "+b.ToString();
        }

        private object refineStr(object str)
        {
            if (String.IsNullOrEmpty(str.ToString()))
            {
                return "(sin dato)";
            }
            return str.ToString();
        }

        private object refineDate(object d)
        {
            if (String.IsNullOrEmpty(d.ToString()))
            {
                return DateTime.Now.ToString();
            }
            return d;
        }

        private object refineSexo(object s)
        {
            if (String.IsNullOrEmpty(s.ToString()))
            {
                return "0";
            }
            if (s.ToString() == "Femenino")
            {
                return 1;
            }
            if (s.ToString() == "Masculino")
            {
                return 2;
            }
            if (s.ToString() == "Indefinido")
            {
                return 3;
            }
            return s;
        }

        private object refineMamaCanguro(object m)
        {
            if (m.ToString() == "SI")
            {
                return 1;
            }
            return 0;
        }
    }
}