using ApiCamajan.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilidades;
using System.Threading.Tasks;
using CamajanSport.BOL;

namespace ApiCamajan.Manejadores
{
    public class ManejadorUsuario
    {
        private CamajanSportContext _db;

        public ManejadorUsuario() {
            _db = new CamajanSportContext();
        }

        public async Task<Usuario> Autenticar(string correo, string contraseña) {
            string password = Encrypt.ComputeHash(contraseña, "SHA512", null);

            Usuario usuario = _db.usuarios.Where(m => m.CorreoElec == correo).FirstOrDefault();

            if (usuario != null) {
                bool success = Encrypt.VerifyHash(contraseña, "SHA512", usuario.Contrasena.Trim());

                if (!success) {
                    usuario = null;
                }
                usuario.Contrasena = string.Empty;
            }

            return usuario;
        }
    }
}