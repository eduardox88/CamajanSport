using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Utilidades;

namespace Utilidades
{
    public class MailHandler
    {
        public static void SendEmail(int idUsuario, string sendTo){

            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("hxf53514@gmail.com");
                    mail.To.Add(sendTo);
                    mail.Subject = "CamajanSport Correo de Confirmación";
                    mail.Body = "<h2>Gracias por registrarte en nuestra página CamajanSport</h2> <p>Para terminar con el proceso de registro, deberas de confirmar tu cuente haciendo click en el siguiente enlace: http://localhost:1721/Usuario/ConfirmacionCorreo?id=" + Encrypt.RijndaelEncrypt(idUsuario.ToString().Trim()) + "</p>";
                    mail.IsBodyHtml = true;
                    //mail.Attachments.Add(new Attachment("C:\\file.zip"));

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("hxf53514@gmail.com", "fullpower");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }

                
            }
            catch (Exception)
            {
                
            }
        }
    }
}
