using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CamajanSport.Models
{
    public class Deporte
    {

        public int IdDeporte { get; set; }
        [Required,MinLength(3)]
        public string Nombre { get; set; }        
        public byte[] Imagen { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaIngreso { get; set; }

    }
}