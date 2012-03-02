using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PepoBebes.Models
{
    public class Log_Bebe
    {
        [Key]
        public int id { get; set; }
        public string usuario { get; set; }
        public DateTime fecha { get; set; }
        public string accion { get; set; }
        public int idBebe { get; set; }
    }
}