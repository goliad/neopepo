using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PepoBebes.Models
{
    public class HistorialAgenda
    {
        [Key]
        public int historialAgendaID { get; set; }
        [DataType(DataType.Date)]
        public DateTime fecha { get; set; }

        public int bebeID { get; set; }

        [MaxLength(250)]
        [Required]
        public string descripcion { get; set; }

        public virtual Bebe bebe { get; set; }
    }

}