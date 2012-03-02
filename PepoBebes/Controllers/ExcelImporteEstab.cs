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
    public class ExcelImportEstab
    {

        // Get the row counts in SQL Server table.
        public int GetRowCounts()
        {
            int iRowCount = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Context"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("select count(*) from ESTABLECIMIENTOS", conn);
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
                OleDbDataAdapter da = new OleDbDataAdapter("select * from  [establecimientos$] ", conn);

                // Fill the DataTable with data from the Excel spreadsheet.
                da.Fill(dtExcel);
            }

            return dtExcel;
        }


        public int importToSQL(string strExcelConn)
        {
            DataTable dtExcel = RetrieveData(strExcelConn);
            int iStartCount = GetRowCounts();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Context"].ToString()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                conn.Open();
                string strSQLcmd = "";
                int cantFilasExcel = dtExcel.Rows.Count;
                for (int i = 0; i < cantFilasExcel; i++)
                {
                    strSQLcmd =
                    "IF (SELECT COUNT(*)FROM DEPARTAMENTOS WHERE descripcion = '" + dtExcel.Rows[i]["departamento"] + "') = 0 " +
                        " begin " +
                        " INSERT INTO DEPARTAMENTOS(descripcion,latitud,longitud) VALUES('" +
                        dtExcel.Rows[i]["departamento"] + "',0,0) " +
                        " end " +
                    //" else " +
                    //    " begin " +
                    //        " UPDATE DEPARTAMENTOS " +
                    //        " SET [cantidad] = [cantidad] + 1 " +
                    //        " WHERE (departamento = '" + dtExcel.Rows[i]["departamento"] + "') " +
                    //    " end " +

                    //"IF (SELECT COUNT(*)FROM LOCALIDADES WHERE localidad = '" + dtExcel.Rows[i]["localidad"] + "') = 0 " +
                    //    " begin " +
                    //    " INSERT INTO LOCALIDADES(localidad) VALUES('" +
                    //    dtExcel.Rows[i]["localidad"] + "') " +
                    //    " end " +
                    //" else " +
                    //    " begin " +
                    //        " UPDATE LOCALIDADES " +
                    //        " SET [cantidad] = [cantidad] + 1 " +
                    //        " WHERE (localidad = '" + dtExcel.Rows[i]["localidad"] + "') " +
                    //    " end " +
                    " declare @idDepartamento as int " +
                    " declare @idLocalidad as int " +
                " SELECT @idDepartamento = [departamentoID] FROM DEPARTAMENTOS WHERE descripcion = '" + dtExcel.Rows[i]["departamento"] + "' " +
                //" SELECT @idLocalidad = [id] FROM LOCALIDADES WHERE localidad = '" + dtExcel.Rows[i]["localidad"] + "' " +
                    " INSERT INTO ESTABLECIMIENTOS"+
                    " (codigo,nombre,telefono,telefono2,domicilio,departamentoID,localidad,director,barrio,cp,email,web,plannacer) " +
                    "VALUES ('" + dtExcel.Rows[i]["codigo"] + "','"+
                    dtExcel.Rows[i]["nombre"] + "','"+
                    dtExcel.Rows[i]["telefono"] + "','" +
                    dtExcel.Rows[i]["telefono2"] +
                                        dtExcel.Rows[i]["domicilio"] + "','" +
                    "',@idDepartamento,'@idLocalidad','" +
                    dtExcel.Rows[i]["director"] +"','"+

                    dtExcel.Rows[i]["barrio"] + "','" +
                    dtExcel.Rows[i]["cp"] + "','" +
                    dtExcel.Rows[i]["email"] + "','" +
                    dtExcel.Rows[i]["web"] + "'," +
                   refinePlanNacer(dtExcel.Rows[i]["plannacer"]) + ") ";

                    cmd.CommandText = strSQLcmd;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();

            }

            // Get the row counts after importing.
            int iEndCount = GetRowCounts();

            return iEndCount - iStartCount;
        }

        private object refinePlanNacer(object p)
        {
            if (p.ToString() == "SI")
            {
                return 1;
            }
            return 0;
        }
        private object refineEG(object eg)
        {
            if (String.IsNullOrEmpty(eg.ToString()))
            {
                return "0";
            }
            return eg;
        }

        private object refineEdad(object d)
        {
            if (String.IsNullOrEmpty(d.ToString()))
            {
                return DateTime.Now.ToString();
            }
            return DateTime.Now.ToString();
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


    }
}