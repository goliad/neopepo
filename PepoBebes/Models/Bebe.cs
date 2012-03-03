using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PepoBebes.Models
{
    public class Bebe
    {
        [Key]
        public int bebeID { get; set; }


        //[MaxLength(100)]//Cuando tenga se le carga
        public string dni { get; set; }

        //[MaxLength(100)]//Cuando tenga se le carga o edita
        public string nombre { get; set; }

        //[MaxLength(2)]//Si o No
        public String vive { get; set; }

        //[Required(ErrorMessage = "Debe ingresar")]
        [DataType(DataType.Date)]
        public DateTime fechaNacimiento { get; set; }


        public int sexoID { get; set; }
        public virtual Sexos sexo { get; set; }

        //[Range(26, 43)]
        public int edadGestacional { get; set; }

        //[Required]
        //[Range(100, 8000)]
        public double peso { get; set; }


        //[MaxLength(100)]
        //[RegularExpression(@"[0-9]+",
        //    ErrorMessage = "Error Historia Clinica.")]
        public string hc { get; set; }


        //[MaxLength(2)]//Si o No
        public string mamaCanguro { get; set; }

        //Tip: Agregar Historial de riesgo, historial de peso.
        public int riesgoID { get; set; }
        public virtual Riesgos riesgo { get; set; }

        public int madreID { get; set; }
        public virtual Madre madre { get; set; }

        public virtual ICollection<HistorialNeo> historialNeo { get; set; }
        public virtual ICollection<HistorialAgenda> historialAgenda { get; set; }
    }

    public class Sexos
    {
        [Key]
        public int sexoID { get; set; }
        public string descripcion { get; set; }
        public string icon { get; set; }
        public virtual ICollection<Bebe> bebe { get; set; }
    }

    public class Riesgos
    {
        [Key]
        public int riesgoID { get; set; }
        public string descripcion { get; set; }
        public string icon { get; set; }

        public virtual ICollection<Bebe> bebe { get; set; }
    }
}