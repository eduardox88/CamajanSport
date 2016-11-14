using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamajanSport.BOL
{
    [Table("Paises")]
    public class Pais
    {
        [Key]
        public int idPais { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaIngreso { get { return DateTime.Now; } }
    }
}
