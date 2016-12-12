using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CamajanSport.BOL
{
    [Table("EstadoResultado")]
    public class EstadoResultado
    {
        [Key]
        public int IdEstadoResultado { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaIngreso { get; set; }
    }
}
