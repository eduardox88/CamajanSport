using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CamajanSport.BOL
{
    [Table("Publicaciones")]
    public class Publicacion
    {
        [Key]
        public int IdPublicacion { get; set; }
        public int IdEquipo1 { get; set; }
        public int IdEquipo2 { get; set; }

        public string Jugador1 { get; set; }
        public string Jugador2 { get; set; }
        public bool EsMatchup { get; set; }
        public string Titulo { get; set; }
        public string Pronostico { get; set; }
        public string Analisis { get; set; }
        public DateTime FechaJuego { get; set; }
        public bool EsPremium { get; set; }
        public int IdEstadoResultado { get; set; }
        public DateTime FechaIngreso { get; set; }
        public int IdUsuario { get; set; }

        [ForeignKey("IdEquipo1")]
        public virtual Equipo Equipo1 { get; set; }
        [ForeignKey("IdEquipo2")]
        public virtual Equipo Equipo2 { get; set; }

        [ForeignKey("IdEstadoResultado")]
        public virtual EstadoResultado EstadoResultado { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
