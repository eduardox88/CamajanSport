using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamajanSport.BOL
{
    [Table("Equipos")]
    public class Equipo
    {
        [Key]
        public int idEquipo { get; set; }
        public string Nombre { get; set; }
        [MaxLength(3)]
        public string Abrev { get; set; }
        public byte[] Imagen { get; set; }
        public int idDeporte { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaIngreso { get { return DateTime.Today; } }
    }
}
