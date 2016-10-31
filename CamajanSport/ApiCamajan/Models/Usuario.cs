using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApiCamajan.Models
{
    [Table("usuarios")]
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        public string Contrasena { get; set; }

        public string NombreUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string CorreoElec { get; set; }
        public string Telefono { get; set; }
        public string Biografia { get; set; }
        public int? IdPais { get; set; }

        public int? IdEstado { get; set; }
        public int? IdRol { get; set; }
        public bool CambiarContrasena { get; set; }
        public DateTime? FechaIngreso { get; set; }

        public virtual Rol rol { get; set; }

    }
}