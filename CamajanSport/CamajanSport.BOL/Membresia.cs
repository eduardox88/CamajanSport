using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CamajanSport.BOL
{
    [Table("Membresias")]
    public class Membresia
    {
        [Key]
        public int IdMembresia { get; set; }
        public string Nombre { get; set; }
       
        public decimal Precio { get; set; }
        public int Duracion { get; set; }
        public int? Promocion { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public bool Activa { get; set; }
        public decimal? Descuento { get; set; }
        public int IdUsuario { get; set; }
    }
}
