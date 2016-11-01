using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Utilidades
{
    public class Helper
    {
        public static async Task<HttpResponseMessage> GetBearerToken(string siteUrl, string Username, string Password)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(siteUrl);
            client.DefaultRequestHeaders.Accept.Clear();

            HttpContent requestContent = new StringContent("grant_type=password&username=" + Username + "&password=" + Password, Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage responseMessage = await client.PostAsync("Token", requestContent);

            return responseMessage;
        }
    }
}
