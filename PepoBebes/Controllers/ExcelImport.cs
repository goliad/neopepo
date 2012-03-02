using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace PepoBebes
{
    public class ExcelImport
    {

        // Get the row counts in SQL Server table.
        public int GetRowCounts()
        {
            int iRowCount = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer2005DBConnectionString"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("select count(*) from BEBES", conn);
                conn.Open();

                // Execute the SqlCommand and get the row counts.
                iRowCount = (int)cmd.ExecuteScalar();
            }

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
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer2005DBConnectionString"].ToString()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                conn.Open();
                string strSQLcmd = "";
                int cantFilasExcel = dtExcel.Rows.Count;
                for (int i = 0; i < cantFilasExcel; i++)
                {
                    strSQLcmd =
                    "IF (SELECT COUNT(*)FROM MADRES WHERE dni = '" + dtExcel.Rows[i]["DNI"] + "') = 0 " +
                        " begin " +

                        "IF (SELECT COUNT(*)FROM DEPARTAMENTOS WHERE departamento = '" + dtExcel.Rows[i]["departamento"] + "') = 0 " +
                        " begin " +
                        " INSERT INTO DEPARTAMENTOS(departamento) VALUES('" +
                        dtExcel.Rows[i]["departamento"] + "') " +
                        " end " +
                    " else " +
                        " begin " +
                            " UPDATE DEPARTAMENTOS " +
                            " SET [cantEst] = [cantEst] + 1 " +
                            " WHERE (departamento = '" + dtExcel.Rows[i]["departamento"] + "') " +
                        " end " +


                        " declare @idDepartamento as int " +
                        " declare @idLocalidad as int " +
                        " SELECT @idDepartamento = [id] FROM DEPARTAMENTOS WHERE departamento = '" +
                        dtExcel.Rows[i]["departamento"] + "' " +


                        " INSERT INTO MADRES(dni,apellido,nombre,fechaNacimiento,edad,domicilio,localidad,departamento,telefono)"+ 
                        " VALUES('" + 
                        dtExcel.Rows[i]["DNI"] +"','"+
                        dtExcel.Rows[i]["Apellido"] +"','"+
                        dtExcel.Rows[i]["Nombre"] +"','"+
                        refineFNac(dtExcel.Rows[i]["Edad"]) + "'," +
                        refineEdad(dtExcel.Rows[i]["Edad"]) + ",'" +
                        dtExcel.Rows[i]["calle"] +" " + 
                        dtExcel.Rows[i]["N°"] +"  "+ refineBarrio(dtExcel.Rows[i]["Barrio"]) + "',@idLocalidad,@idDepartamento,'"+
                        dtExcel.Rows[i]["Telefono"] + "') " +                               
                        " end " +
                    " else " +
                        " begin " +
                            " UPDATE MADRES " +
                            " SET [cantHijos] = [cantHijos] + 1 " +
                            " WHERE (dni = '" + dtExcel.Rows[i]["DNI"] + "') " +
                        " end " +
                    " declare @idMadre as int " +
                    " SELECT @idMadre = [id] FROM MADRES WHERE dni = '"+dtExcel.Rows[i]["DNI"]+"' "+

                    " INSERT INTO BEBES (fechaNacimiento,fechaIngreso,sexo,edadGestacional,peso,diagnostico,lugarNacimiento,derivacion,mamaCanguro,observaciones,hc,idMadre) VALUES ('" +
                    dtExcel.Rows[i]["fechaNacimiento"] + "','" +
                    dtExcel.Rows[i]["fechaIngreso"] + "'," +
                    refineSexo(dtExcel.Rows[i]["sexo"]) + "," +
                    refineEG(dtExcel.Rows[i]["EG"]) + "," +
                    refinePeso(dtExcel.Rows[i]["peso"]) +",'"+
                    dtExcel.Rows[i]["diagnostico"] + "','" +
                    dtExcel.Rows[i]["lugarNacimiento"] + "','" +
                    dtExcel.Rows[i]["derivacion"] + "'," +
                    refineMamaCanguro(dtExcel.Rows[i]["mamaCanguro"]) + ",'" +
                    dtExcel.Rows[i]["observaciones"] + "','" +
                    dtExcel.Rows[i]["hc"]+ "'," +
                    "@idMadre" + ");" +
                    " declare @dateIter as DateTime; "+
                    " declare @dateFin as DateTime; " +
                    " declare @dateInicio as DateTime; " +
                    " set @dateFin = DATEADD (month , 24 ,'"+
                    dtExcel.Rows[i]["fechaNacimiento"] + "')"+
                    " set @dateIter ='"+dtExcel.Rows[i]["fechaNacimiento"] +"'"+
                    " set @dateInicio ='" + dtExcel.Rows[i]["fechaNacimiento"] + "'" +

                    " WHILE (@dateIter BETWEEN @dateInicio AND @dateFin )" +
                        " BEGIN"+
                        " INSERT INTO AGENDA(fecha,bebeID) VALUES (" +
                        " @dateIter,IDENT_CURRENT('BEBES')) "+
                        " set @dateIter =DATEADD (month , 1 , @dateIter)" +
                        " END";

                    cmd.CommandText = strSQLcmd;
                    cmd.ExecuteNonQuery();
                }
            }

            // Get the row counts after importing.
            int iEndCount = GetRowCounts();

            return iEndCount - iStartCount;
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

        private object refineFNac(object d)
        {
            if (String.IsNullOrEmpty(d.ToString()))
            {
                return DateTime.Now.ToString();
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
            return d;
        }

        private object refineBarrio(object b)
        {
            if (String.IsNullOrEmpty(b.ToString()))
            {
                return "";
            }
            return "B°: "+b.ToString();
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