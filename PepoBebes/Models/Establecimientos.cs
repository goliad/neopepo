using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PepoBebes.Models
{
    public class Establecimientos
    {
        [Key]
        public int establecimientoID { get; set; }

        [Required]
        public string codigo { get; set; }

        [MaxLength(100)]
        public string nombre { get; set; }

        [MaxLength(100)]
        public string telefono { get; set; }

        [MaxLength(100)]
        public string telefono2 { get; set; }

        [MaxLength(200)]
        public string domicilio { get; set; }

        public int departamentoID { get; set; }
        public virtual Departamentos departamento { get; set; }

        [MaxLength(100)]
        public string localidad { get; set; }

        [MaxLength(100)]
        public string director { get; set; }

        [MaxLength(100)]
        public string barrio { get; set; }

        [MaxLength(20)]
        public string cp { get; set; }

        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [MaxLength(100)]
        public string web { get; set; }

        public int planNacer { get; set; }
    }
}