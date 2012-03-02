using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PepoBebes.Models
{
    public class Agenda
    {
        public int agendaID { get; set; }
        [DataType(DataType.Date)]
        public DateTime fecha { get; set; }
        public int bebeID { get; set; }

        [MaxLength(100)]
        public string notas { get; set; }


        public int StatusID { get; set; }
        public virtual Status Status { get; set; }

        public virtual Bebe bebe { get; set; }
    }

    public class Status
    {
        public int StatusID { get; set; }
        public string description { get; set; }
        public virtual ICollection<Agenda> agenda { get; set; }
    }
}