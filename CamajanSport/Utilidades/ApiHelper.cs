using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http.Formatting;
using CamajanSport.BOL;

namespace Utilidades
{
    public class ApiHelper
    {
        public static async Task<HttpResponseMessage> POST<T>(string nombreControladorAccion, T objeto, Token token) where T : class
        { 
        
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);

            HttpResponseMessage result = await client.PostAsync<T>("http://localhost:14678/api/" + nombreControladorAccion, objeto, new JsonMediaTypeFormatter());

            return result;
        }

        public static async Task<HttpResponseMessage> GET_By_ID(string nombreControladorAccion, int id, Token token)
        {

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);

            HttpResponseMessage result = await client.GetAsync("http://localhost:14678/api/" + nombreControladorAccion + "/" + id);

            return result;
        }

        public static async Task<HttpResponseMessage> PUT<T>(string nombreControladorAccion,T objeto, Token token) where T : class
        {

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);

            HttpResponseMessage result = await client.PutAsync<T>("http://localhost:14678/api/" + nombreControladorAccion, objeto, new JsonMediaTypeFormatter());

            return result;
        }

        public static async Task<HttpResponseMessage> DELETE(string nombreControladorAccion, int ID, Token token)
        {

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);

            HttpResponseMessage result = await client.DeleteAsync("http://localhost:14678/api/" + nombreControladorAccion + "/" + ID);

            return result;
        }

        public static async Task<List<T>> GET_List<T>(string nombreControladorAccion, Token token) where T : class
        {
            List<T> lista = null;

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);

            HttpResponseMessage result = await client.GetAsync("http://localhost:14678/api/" + nombreControladorAccion);

            if (result.IsSuccessStatusCode) {

                lista = new List<T>();
                lista = await result.Content.ReadAsAsync<List<T>>();
            }

            return lista;
        }
    }
}