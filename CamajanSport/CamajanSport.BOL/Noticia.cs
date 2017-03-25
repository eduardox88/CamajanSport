using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamajanSport.BOL
{
    public class Noticia
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public bool Activo { get; set; }

        public DateTime FechaIngreso { get; set; }
    }
}
