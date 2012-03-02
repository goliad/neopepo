using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Common;
using System.IO;
using System.Data;
using System.Configuration;

namespace ExcelToSQL
{
    public partial class _Default : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            actualizarGrid();
        }

        protected void ButtonImportar_Click(object sender, EventArgs e)
        {
            if (fupExcel.HasFile)
            {
                string strFileName = Server.HtmlEncode(fupExcel.FileName);
                if (verficarExcel(strFileName))
                {
                    ExcelImport excelImport = new ExcelImport();

                    lblMessages.Text = Convert.ToString(excelImport.importToSQL(strExcelConn(uploadExcelToServer(strFileName)))) + " registros fueron importados";
                    actualizarGrid();
                }
            }

        }

        public bool verficarExcel(string strFileName)
        {
            // Get the extension of the Excel spreadsheet.
            string strExtension = Path.GetExtension(strFileName);
            // Validate the file extension.
            if (strExtension != ".xls" && strExtension != ".xlsx")
            {
                Response.Write("<script>alert('Por favor seleccione un archivo Excel!');</script>");
                return false;
            }
            return true;
        }

        public string uploadExcelToServer(string strFileName)
        {
            string strExtension = Path.GetExtension(strFileName);
            // Generate the file name to save.
            string strUploadFileName = "~/UploadFiles/" + DateTime.Now.ToString("yyyyMMddHHmmss") + strExtension;
            // Save the Excel spreadsheet on server.
            fupExcel.SaveAs(Server.MapPath(strUploadFileName));
            // Generate the connection string for Excel file.
            return strUploadFileName;
        }


        public string strExcelConn(string strUploadFileName)
        {
            string strExtension = Path.GetExtension(strUploadFileName);
            if (strExtension == ".xls")
            {
                // Excel 97-2003
                return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Server.MapPath(strUploadFileName) + ";Extended Properties='Excel 8.0;HDR=YES;'";
            }
            else
            {
                // Excel 2007
                return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath(strUploadFileName) + ";Extended Properties='Excel 12.0 Xml;HDR=YES;'";
            }
        }


        public void actualizarGrid()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer2005DBConnectionString"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("select * from Vista_Bebes", conn);
                conn.Open();

                // Execute the SqlCommand and get the row counts.

                this.GridViewBebes.DataSource = cmd.ExecuteReader();
                this.GridViewBebes.DataBind();
            }
        }

        public void verDetalles(int id)
        {
            Session.Add("idBebe", id);
            Response.Redirect("DetallesBebe.aspx");
        }

        public void verDiagnostico(int id)
        {
            Session.Add("idBebe", id);
            Response.Redirect("DiagnosticoBebe.aspx");
        }

        protected void lnkAgregar_Click(object sender, EventArgs e)
        {
        }

        protected void GridViewBebes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "detalles")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                // row contains current Clicked Gridview Row
                String bebeId = row.Cells[1].Text;
                verDetalles(Convert.ToInt32(bebeId));
            }
            if (e.CommandName == "diagnostico")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                // row contains current Clicked Gridview Row
                String bebeId = row.Cells[1].Text;
                verDiagnostico(Convert.ToInt32(bebeId));
            }
        }
    }
}


