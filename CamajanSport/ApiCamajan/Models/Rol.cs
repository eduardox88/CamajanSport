using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApiCamajan.Models
{
    [Table("roles")]
    public class Rol
    {
        [Key]
        public int IdRol { get; set; }

        public string Nombre { get; set; }
        public DateTime FechaIngreso { get; set; }
    }
}