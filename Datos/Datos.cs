using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data.Sql;
using System.Data;

namespace Datos
{
    public class Datos
    {

        protected OleDbConnection conexion = new OleDbConnection("Provider=SQLOLEDB.1;Persist Security Info=False;User ID=sa; Password=; Initial Catalog=NeoPepo; Data Source= RaMoNs-Laptop\\SQLEXPRESS");
        protected OleDbCommand comando = new OleDbCommand();
        protected OleDbCommand consulta = new OleDbCommand();

        public static System.String buscaServidor()
        {
            SqlDataSourceEnumerator Descubridor_de_sql = SqlDataSourceEnumerator.Instance;
            DataTable sqls = Descubridor_de_sql.GetDataSources();
            foreach (DataRow servSQL in sqls.Rows)
            {
                String servString = "Data Source=" + servSQL["ServerName"] + "\\SQLEXPRESS";
                return servString;
            }
            return "Error";
        }

        public Datos()
        {
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            comando.Connection = conexion;

            consulta.CommandType = System.Data.CommandType.StoredProcedure;
            consulta.Connection = conexion;
        }

        public System.Data.DataSet obtenerSelect(String procedimientoAlmacenado)
        {
            try
            {
                conexion.Open();
                consulta.CommandText = procedimientoAlmacenado;

                System.Data.DataSet t = new System.Data.DataSet();
                OleDbDataAdapter td = new OleDbDataAdapter(consulta);
                td.Fill(t);
                //consulta.ExecuteNonQuery();
                //consulta.Dispose();      
                conexion.Close();
                return t;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
