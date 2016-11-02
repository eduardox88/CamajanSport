using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CamajanSport.BOL
{
    [Table("deportes")]
    public class Deporte
    {
        [Key]
        public int IdDeporte { get; set; }

        public string Nombre { get; set; }

        public byte?[] Imagen { get; set; }

        public bool? Activo { get; set; }

        public DateTime? FechaIngreso { get; set; }

    }       
}