using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using PayPal.Api;
using CamajanSport.BOL;
using Utilidades;
using System.Web.Security;

namespace CamajanSport
{
    public class Paypal
    {
        private Usuario GetUserDecrypted
        {

            get
            {
                Usuario token = CookieHandler.GetCookieDecrypted<Usuario>(FormsAuthentication.FormsCookieName);

                return token;
            }
        }
        public static APIContext GetPaypalAPIContext
        {
            get
            {
                var apiContext = new APIContext(GetAccessToken);
                apiContext.Config = GetProperties;

                // Define any HTTP headers to be used in HTTP requests made with this APIContext object
                if (apiContext.HTTPHeaders == null)
                {
                    apiContext.HTTPHeaders = new Dictionary<string, string>();
                }
                //apiContext.HTTPHeaders["some-header-name"] = "some-value";
                return apiContext;
            }
        }

        // Initialize the apiContext's configuration with the default configuration for this application.
        public static Dictionary<string, string> GetProperties
        {
            get 
            {
                return ConfigManager.Instance.GetProperties();
            }
        }
        //Generates and return the PayPal AccessToken
        public static string GetAccessToken 
        {
            get {
                // Get a reference to the config
                var config = ConfigManager.Instance.GetProperties();
                // Use OAuthTokenCredential to request an access token from PayPal
                return new OAuthTokenCredential(config["clientId"], config["clientSecret"]).GetAccessToken();
            }
        }

        public Payment CrearPagoMembresia(string NombreArticulo, string Precio, string DescripcionTransaccion, Details Detalles, string CancelURL, string ReturnURL)
        {
            //similar to credit card create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };

            itemList.items.Add(new Item()
            {
                name = NombreArticulo,
                currency = "USD",
                price = Precio,
                quantity = "1"//,
                //sku = "sku"
            });

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = CancelURL,
                return_url = ReturnURL
            };

            // similar as we did for credit card, do here and create details object

            var details = Detalles;

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount();
            amount.currency = "USD";
            amount.total = Precio;// Total must be equal to sum of shipping, tax and subtotal.
            if (Detalles != null)
            {
                amount.details = Detalles;
            }

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = DescripcionTransaccion,
                //invoice_number = "CD-" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + GetUserDecrypted.IdUsuario,
                amount = amount,
                item_list = itemList
            });
            Payment pago = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            return pago.Create(GetPaypalAPIContext);
        }

        public Payment ProcesarPagoMembresia(string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            Payment pago = new Payment() { id = paymentId };
            return pago.Execute(GetPaypalAPIContext, paymentExecution);
        }

        private Payment GetDetallesPagoById(string PaymentId)
        {
            return Payment.Get(GetPaypalAPIContext, PaymentId);
        }
    }
}