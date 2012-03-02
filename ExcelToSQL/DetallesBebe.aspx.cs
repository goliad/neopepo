using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace ExcelToSQL
{
    public partial class Formulario_web11 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int idBebe = (int)Session["idBebe"];
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer2005DBConnectionString"].ToString()))
                {
                    SqlCommand cmd = new SqlCommand(
                         " declare @idMadre as int " +
                        " SELECT @idMadre = [idMadre] FROM BEBES WHERE id = " + idBebe +
                        " declare @idDepartamento as int " +
                        " SELECT @idDepartamento = [departamento] FROM MADRES WHERE id = @idMadre " +
                        " select * from VISTA_ESTABLECIMIENTOS where IDDEP = @idDepartamento ", conn);
                    conn.Open();

                    // Execute the SqlCommand and get the row counts.
                    GridViewEstablecimientos.DataSource = cmd.ExecuteReader();
                    GridViewEstablecimientos.DataBind();
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }

        }
    }
}