using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CamajanSport.Models
{
    public class Usuario
    {
        
        public int IdUsuario { get; set; }
        [Required, MaxLength(20)]
        public string NombreUsuario { get; set; }

        [Required]
        public string NombreCompleto { get; set; }

        [Required]
        public string Correo { get; set; }

        [Required]
        public string Telefono { get; set; }

        public string Biografia { get; set; }

        [Required]

        public int idPais { get; set; }

        [Required]

        public int idEstado { get; set; }

        [Required]

        public int IdRol { get; set; }

        public bool CambiarPassword { get; set; }

        public DateTime FechaIngreso { get; set; }
    }
}