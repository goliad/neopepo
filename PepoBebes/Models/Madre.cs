using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PepoBebes.Models
{
    public class Madre
    {
        [Key]
        public int madreID { get; set; }

        [MaxLength(100, ErrorMessage = "maximo 100 caracteres")]//100 por que algunos documentos son raros como uruguayo o asi
        public string dni { get; set; }

        [MaxLength(100, ErrorMessage = "maximo 100 caracteres")]
        public string apellido { get; set; }

        [MaxLength(100, ErrorMessage = "maximo 100 caracteres")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Debe ingresar")]
        [DataType(DataType.Date)]
        public DateTime fechaNacimiento { get; set; }

        [Required(ErrorMessage = "Debe ingresar")]
        [Range(10, 100)]
        public int edad { get; set; }

        [MaxLength(200)]
        public string domicilio { get; set; }

        [MaxLength(100)]
        public string localidad { get; set; }

        public int departamentoID { get; set; }
        public virtual Departamentos departamento { get; set; }

        [MaxLength(100)]
        public string telefono { get; set; }

        [MaxLength(100)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
    ErrorMessage = "ingrese un email valido.")]
        public string email { get; set; }
        //? lo hace nullable

        public virtual ICollection<Bebe> bebes { get; set; }
    }
}