using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamajanSport.BOL
{
    [Table("Estados")]
    public class Estado
    {
        [Key]
        public int IdEstado { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaIngreso { get; set; }
        public bool Activo { get; set; }
    }
}
