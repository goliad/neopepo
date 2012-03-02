using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PepoBebes.Models
{
    public class HistorialNeo
    {
        [Key]
        public int historialNeoID { get; set; }
        [DataType(DataType.Date)]
        public DateTime fecha { get; set; }

        [MaxLength(10)]
        public string tipo { get; set; } //Ingreso o Reingreso

        [Required]
        [DataType(DataType.Date)]
        public DateTime fechaIngreso { get; set; }

        [Required]
        [Range(100, 8000)]
        public double pesoNeo { get; set; }

        [MaxLength(100)]
        public string lugarNacimiento { get; set; }

        [MaxLength(100)]
        public string derivacion { get; set; }

        [MaxLength(100)]
        public string medicoReceptor { get; set; }

        [MaxLength(250)]
        public string diagnostico { get; set; }

        [DataType(DataType.Date)]
        public DateTime fechaEgreso { get; set; }
        
        [MaxLength(100)]
        public string medicoAlta { get; set; }
        
        [MaxLength(250)]
        public string responsable { get; set; }//Persona que lo retira


        [MaxLength(250)]
        public string observaciones { get; set; }

        public int bebeID { get; set; }
        public virtual Bebe bebe { get; set; }
    }
}