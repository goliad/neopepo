using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PepoBebes.Models
{
    public class SampleData : DropCreateDatabaseIfModelChanges<Context>
    {
        protected override void Seed(Context context)
        {
            new List<Sexos>
            {
                new Sexos { descripcion = "Masculino" },
                new Sexos { descripcion = "Femenino" },
                new Sexos { descripcion = "Indefinido" },
            }.ForEach(a => context.Sexos.Add(a));

            new List<Status>
            {
                new Status { description = "Nuevo" },
                new Status { description = "Completado" },
                new Status { description = "Cancelado" },
            }.ForEach(a => context.Status.Add(a));

            new List<Riesgos>
            {
                new Riesgos { descripcion = "Nada" },
                new Riesgos { descripcion = "Poco" },
                new Riesgos { descripcion = "Mucho" },
            }.ForEach(a => context.Riesgos.Add(a));

            new List<Departamentos>
            {
                new Departamentos { descripcion="Capital" },
                new Departamentos { descripcion = "La Banda" },
                new Departamentos { descripcion = "Frias" },
            }.ForEach(a => context.Departamentos.Add(a));

            context.SaveChanges();
        }
    }
}