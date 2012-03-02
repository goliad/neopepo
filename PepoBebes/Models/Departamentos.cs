using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PepoBebes.Models
{
    public class Departamentos
    {
        [Key]
        public int departamentoID { get; set; }
        public string descripcion { get; set; }

        public decimal latitud { get; set; }
        public decimal longitud { get; set; }

        public string provincia { get; set; }
        public virtual ICollection<Madre> madre { get; set; }

        public virtual ICollection<Establecimientos> establecimientos { get; set; }
    }
}