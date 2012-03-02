using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;

namespace ExcelToSQL
{
    public partial class Formulario_web1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        // Retrieve data from SQL Server table
        protected DataTable RetrieveData()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer2005DBConnectionString"].ToString()))
            {
                // Initialize a SqlDataAdapter object.
                SqlDataAdapter da = new SqlDataAdapter("select * from Vista_Madres", conn);

                // Fill the DataTable with data from SQL Server table.
                da.Fill(dt);
            }

            return dt;
        }

        // Export data to an Excel spreadsheet via ADO.NET
        protected void ExportToExcel(string strConn, DataTable dtSQL)
        {
            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                // Create a new sheet in the Excel spreadsheet.
                OleDbCommand cmd = new OleDbCommand("create table Madres(apellido varchar(100), nombre varchar(100))", conn);

                // Open the connection.
                conn.Open();

                // Execute the OleDbCommand.
                cmd.ExecuteNonQuery();

                cmd.CommandText = " INSERT INTO Madres (apellido, nombre) values (?,?)";

                // Add the parameters.
                cmd.Parameters.Add("apellido", OleDbType.VarChar, 100, "apellido");
                cmd.Parameters.Add("nombre", OleDbType.VarChar, 100, "nombre");

                // Initialize an OleDBDataAdapter object.
                OleDbDataAdapter da = new OleDbDataAdapter(" select * from Madres", conn);

                // Set the InsertCommand of OleDbDataAdapter, 
                // which is used to insert data.
                da.InsertCommand = cmd;

                // Changes the Rowstate()of each DataRow to Added,
                // so that OleDbDataAdapter will insert the rows.
                foreach (DataRow dr in dtSQL.Rows)
                {
                    dr.SetAdded();
                }

                // Insert the data into the Excel spreadsheet.
                da.Update(dtSQL);

            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string strDownloadFileName = "";
            string strExcelConn = "";

            if (rblExtension.SelectedValue == "2003")
            {
                // Excel 97-2003
                strDownloadFileName = "~/DownloadFiles/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                strExcelConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Server.MapPath(strDownloadFileName) + ";Extended Properties='Excel 8.0;HDR=Yes'";
            }
            else
            {
                // Excel 2007
                strDownloadFileName = "~/DownloadFiles/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                strExcelConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath(strDownloadFileName) + ";Extended Properties='Excel 12.0 Xml;HDR=Yes'";
            }

            // Retrieve data from SQL Server table.
            DataTable dtSQL = RetrieveData();

            // Export data to an Excel spreadsheet.
            ExportToExcel(strExcelConn, dtSQL);

            if (rblDownload.SelectedValue == "Yes")
            {
                hlDownload.Visible = true;

                // Display the download link.
                hlDownload.Text = "Click here to download file.";
                hlDownload.NavigateUrl = strDownloadFileName;
            }
            else
            {
                // Hide the download link.
                hlDownload.Visible = false;
            }
        }
    }
}