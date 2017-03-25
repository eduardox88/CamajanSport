using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilidades
{
    public class DynamicObjectHandler
    {
        public static object SwalResponse(string title, TypeResult type, string mensaje)
        {

            var objeto = new
            {
                Title = title,
                Type = type.ToString(),
                Message = mensaje
            };

            return objeto;
        }
    }
}
