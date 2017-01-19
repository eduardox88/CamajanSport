using CamajanSport.BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using Utilidades;

namespace CamajanSport.Controllers
{
    public class MenuInitializer
    {
        public Literal Initialize(Usuario user)
        {
            Usuario userLogged = CookieHandler.GetCookieDecrypted<Usuario>(FormsAuthentication.FormsCookieName);
            Literal menuHtml = new Literal();
            if (userLogged != null)
            {   
                if (userLogged.rol.IdRol == 3/*Administrador*/)
                {
                    menuHtml.Text += "<li><a class='active' href='/Admin/Dashboard'><i class='fa fa-dashboard'></i> Dashboard</a></li>";
                    menuHtml.Text += "<li class='panel'>" +
                            "<a href='javascript:;' data-parent='#side' data-toggle='collapse' class='accordion-toggle' data-target='#forms'>" +
                                "<i class='fa fa-edit'></i> Mantenimientos <i class='fa fa-caret-down'></i>" +
                            "</a>" +
                            "<ul class='collapse nav' id='forms'>" +
                                "<li>" +
                                    "<a href='/Deporte/MantDeportes'>" +
                                        "<i class='fa fa-angle-double-right'></i> Deporte" +
                                    "</a>" +
                                "</li>" +
                                "<li>" +
                                    "<a href='/Equipo/MantEquipos'>" +
                                        "<i class='fa fa-angle-double-right'></i> Equipos" +
                                    "</a>" +
                                "</li>" +
                                "<li>" +
                                    "<a href='/Usuario/MantUsuarios'>" +
                                        "<i class='fa fa-angle-double-right'></i> Usuarios" +
                                    "</a>" +
                                "</li>" +
                                "<li>" +
                                    "<a href='/Membresia/MantMembresias'>" +
                                        "<i class='fa fa-angle-double-right'></i> Membresias" +
                                    "</a>" +
                                "</li>" +
                                "<li>" +
                                    "<a href='/Publicacion/MantPublicaciones'>" +
                                        "<i class='fa fa-angle-double-right'></i> Publicaciones" +
                                    "</a>" +
                                "</li>" +
                            "</ul>" +
                        "</li>";
                }
                else if (userLogged.rol.IdRol == 2/*Camajan*/)
                {
                    menuHtml.Text +=
                                "<li>" +
                                    "<a href='/Publicacion/MantPublicaciones'>" +
                                        "<i class='fa fa-angle-double-right'></i> Mis Publicaciones" +
                                    "</a>" +
                                "</li>";
                }
                else if (userLogged.rol.IdRol == 1/*Regular User*/)
                {
                    menuHtml.Text += "<li>" +
                                    "<a href='/Membresia/GetMisMembresias'>" +
                                        "<i class='fa fa-angle-double-right'></i> Mis Membresías" +
                                    "</a>" +
                                "</li>";
                }
                menuHtml.Text += "<li>" +
                            "<a href='/Usuario/PerfilUsuario'>" +
                                "<i class='fa fa-wrench'></i> Perfil de usuario" +
                            "</a>" +
                        "</li>" +
                        "<li>" +
                            "<a href='/LogIn/Index'>" +
                                "<i class='fa fa-sign-out'></i> Terminar Sesión" +
                            "</a>" +
                        "</li>";
            }
            return menuHtml;//ViewBag.Menu = menuHtml.Text;
        }
    }
}
