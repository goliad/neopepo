using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace Datos
{
    // : dos puntos para heredar lo que tiene la clase Datos.
    public class Bebes : Datos
    {

        public void insertarProducto(String nombre, double precio)
        {
            comando.CommandText = "insertProducto";
            comando.Parameters.Add(new OleDbParameter("@paramNombre", nombre));
            comando.Parameters.Add(new OleDbParameter("@paramPrecio", precio));
            conexion.Open();
            comando.ExecuteNonQuery();
            comando.Dispose();
            conexion.Close();

        }
        public void modificarProducto(int id, String nombre, double precio)
        {
            comando.CommandText = "modificarProducto";
            comando.Parameters.Add(new OleDbParameter("@paramId", id));
            comando.Parameters.Add(new OleDbParameter("@paramNombre", nombre));
            comando.Parameters.Add(new OleDbParameter("@paramPrecio", precio));
            conexion.Open();
            comando.ExecuteNonQuery();
            comando.Dispose();
            conexion.Close();
        }

        public void eliminarProducto(int id)
        {
            comando.CommandText = "eliminarProducto";
            comando.Parameters.Add(new OleDbParameter("@paramId", id));
            conexion.Open();
            comando.ExecuteNonQuery();
            comando.Dispose();
            conexion.Close();

        }
        public System.Data.DataSet obtenerBebesSexo()
        {
            return obtenerSelect("obtenerSexosCantidad");
        }

        public System.Data.DataSet obtenerBebesDepartamento()
        {
            return obtenerSelect("obtenerDepartamentosCantidad");
        }
    }
}
