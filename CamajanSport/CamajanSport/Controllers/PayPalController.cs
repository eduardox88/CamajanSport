using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Utilidades;
using CamajanSport.BOL;
using CamajanSport.Properties;
using System.Threading.Tasks;
using PayPal.Api;
using System.Configuration;
using System.Web.Security;

namespace CamajanSport.Controllers
{
    public class PayPalController : Controller
    {
        private Token GetAuthToken
        {

            get
            {
                Token token = CookieHandler.GetCookieDecrypted<Token>(Settings.Default.TokenCookie);

                return token;
            }
        }
        private Usuario GetUserDecrypted
        {

            get
            {
                Usuario token = CookieHandler.GetCookieDecrypted<Usuario>(FormsAuthentication.FormsCookieName);

                return token;
            }
        }
        [Authorize]
        public async Task<JsonResult> PagoConCuentaPayPal(int IdMembresia) 
        {

            try
            {
                Paypal paypal = new Paypal();
                //Membresia mem = await ApiHelper.GET_By_ID<Membresia>("Membresia/GetMembresia", IdMembresia, GetAuthToken);
                Membresia mem = await ApiHelper.GET_By_ID<Membresia>("Membresia/GetMembresia", IdMembresia, GetAuthToken);
                string guid = Convert.ToString((new Random()).Next(100000));
                Session[guid] = null;
                Session[guid + "memId"] = null;
                string CancelURL = Request.Url.Scheme + "://" + Request.Url.Authority + ConfigurationManager.AppSettings["CancelURL"];
                string ReturnURL = Request.Url.Scheme + "://" + Request.Url.Authority + ConfigurationManager.AppSettings["ReturnURL"] + "?guid=" + guid;
                Payment pago = paypal.CrearPagoMembresia(mem.Nombre, (mem.Precio - (mem.Descuento == null ? 0 : mem.Descuento)).ToString(), "Membresía CamajanDeportivo (" + mem.Nombre + ")", null, CancelURL, ReturnURL);
                var links = pago.links.GetEnumerator();

                string paypalRedirectUrl = null;

                while (links.MoveNext())
                {
                    Links lnk = links.Current;

                    if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                    {
                        //saving the payapalredirect URL to which user will be redirected for payment
                        paypalRedirectUrl = lnk.href;
                    }
                }

                // saving the paymentID in the key guid
                Session.Add(guid, pago.id);
                Session.Add(guid + "memId", IdMembresia);
                return Json(new { result="OK",url = paypalRedirectUrl });

                //return Redirect(paypalRedirectUrl);
            }
            catch (Exception)
            {
                return Json(new { result="ERROR",Message="Ha ocurrido un error al realizar el pago utilizando Paypal. Favor intente más tarde. \\n\\nSi el problema persiste contacte el administrador."});
                throw;
            }

        }

        [Authorize]
        public async Task<ActionResult> ProcesarPagoPaypal()
        {
            bool ocurrioError = false;
            Task<MembresiaUsuario> membresia = null;
            if (Request.Params["guid"] != null && Request.Params["guid"] != "" && Request.Params["paymentId"] != null && Request.Params["paymentId"] != ""
                && Request.Params["token"] != null && Request.Params["token"] != "" && Request.Params["PayerID"] != null && Request.Params["PayerID"] != "")
            {                
                try
                {
                    Membresia memActual = await ApiHelper.GET_By_ID<Membresia>("Membresia/GetMembresia", int.Parse(Session[Request.Params["guid"] + "memId"].ToString()), GetAuthToken);
                    MembresiaUsuario memUsuario = new MembresiaUsuario();
                    memUsuario.IdMembresia = memActual.IdMembresia;
                    memUsuario.Nombre = memActual.Nombre;
                    memUsuario.MontoTransaccion = (memActual.Precio - (memActual.Descuento == null ? Convert.ToDecimal(0) : Convert.ToDecimal(memActual.Descuento)));
                    memUsuario.Precio = memActual.Precio;
                    memUsuario.Duracion = memActual.Duracion;
                    memUsuario.Promocion = memActual.Promocion;
                    memUsuario.IdUsuario = GetUserDecrypted.IdUsuario;
                    memUsuario.Descuento = memActual.Descuento;
                    memUsuario.Activa = true;
                    memUsuario.FechaExpiracion = DateTime.Now.AddDays(memActual.Duracion + (memActual.Promocion == null ? 0 : Convert.ToInt32(memActual.Promocion)));
                    memUsuario.IdTransaccionPago = Request.Params["paymentId"].ToString();
                    HttpResponseMessage result = await ApiHelper.POST<MembresiaUsuario>("MembresiaUsuarios/PostMembresiaUsuario", memUsuario, GetAuthToken);
                    membresia = result.Content.ReadAsAsync<MembresiaUsuario>();
                    if (result.IsSuccessStatusCode)
                    {
                        try
                        {
                            Paypal paypal = new Paypal();
                            var pagoEjecutado = paypal.ProcesarPagoMembresia(Request.Params["PayerID"], Request.Params["paymentId"]);
                            if (pagoEjecutado.state.ToLower() == "approved")
                            {
                                ViewBag.GUID = Session[Request.Params["guid"]].ToString();
                                ViewBag.PaymentID = Request.Params["paymentId"].ToString();
                                ViewBag.PaymentSucessful = true;
                            }
                            else
                            {
                                string razon = pagoEjecutado.failure_reason;
                            }

                            Session[Request.Params["guid"]] = null;
                            Session[Request.Params["guid"] + "memId"] = null;

                        }
                        catch (PayPal.PaymentsException pay)
                        {
                            ocurrioError = true;
                            throw;
                        }
                        catch (PayPal.PayPalException ex)
                        {
                            ocurrioError = true;
                            throw;
                        }
                    }
                    else
                    {
                        throw new Exception("Ha ocurrido un error al procesar la transacción. Si el problema persiste contacte su administrador.");
                        //return Json(new { result="ERROR",Message="Ha ocurrido un error al procesar la transacción. Si el problema persiste contacte su administrador."});
                    }
                }
                catch (Exception)
                {
                    ocurrioError = true;
                    ViewBag.GUID = "";
                    ViewBag.PaymentID = "";
                }
            }
            else
            {
                ocurrioError = true;
            }
            if (!ocurrioError)
            {
                return Redirect("Membresia/ListarMisMembresias?paymentId=" + Request.Params["paymentId"]);
            }
            else
            {
                //TODO si ocurrio un error
                if (membresia != null)
                {
                    await ApiHelper.DELETE("MembresiaUsuarios/DeleteMembresiaUsuario", membresia.Result.IdMembresiaUsuario, GetAuthToken);
                }
                return Redirect("../Home/Membresias?error=true");
            }
            
            //var membresias = await ApiHelper.GET_ListById<Membresia>("MembresiaUsuarios/GetMembresiasUsuarioById", GetUserDecrypted.IdUsuario, GetAuthToken);
            //return Json(membresias, JsonRequestBehavior.AllowGet);
        }
    }
}
