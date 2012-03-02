using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PepoBebes.Models
{
    public class Context : DbContext
    {
        //public Context() : base("NeoPepo") { }


        public DbSet<Bebe> Bebes { get; set; }
        public DbSet<Madre> Madres { get; set; }

        public DbSet<Departamentos> Departamentos { get; set; }

        public DbSet<Agenda> Agenda { get; set; }



        public DbSet<Status> Status { get; set; }

        public DbSet<Sexos> Sexos { get; set; }

        public DbSet<Riesgos> Riesgos { get; set; }

        public DbSet<Establecimientos> Establecimientos { get; set; }

        public DbSet<HistorialAgenda> HistorialAgenda { get; set; }

        public DbSet<Log_Bebe> Log_Bebes { get; set; }

        public DbSet<HistorialNeo> HistorialNeo { get; set; }
    }
}