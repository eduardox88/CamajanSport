using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http.Formatting;
using CamajanSport.BOL;
using System.Text;

namespace Utilidades
{
    public class ApiHelper
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
        public static async Task<HttpResponseMessage> POST<T>(string nombreControladorAccion, T objeto, Token token) where T : class
        { 
        
            HttpClient client = new HttpClient();

            if (token != null) {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);
            }
                
            HttpResponseMessage result = await client.PostAsync<T>("http://localhost:14678/api/" + nombreControladorAccion, objeto, new JsonMediaTypeFormatter());

            return result;
        }

        public static async Task<T> GET_By_ID<T>(string nombreControladorAccion, int id, Token token) where T: class
        {
            T objeto = null;

            HttpClient client = new HttpClient();

            if (token != null)
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);
            }
                

            HttpResponseMessage result = await client.GetAsync("http://localhost:14678/api/" + nombreControladorAccion + "/" + id);

            if (result.IsSuccessStatusCode)
            {
                objeto = await result.Content.ReadAsAsync<T>();
            }


            return objeto;
        }

        public static async Task<HttpResponseMessage> GET(string nombreControladorAccion, Token token)
        {

            HttpClient client = new HttpClient();

            if (token != null)
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);
            }

            HttpResponseMessage result = await client.GetAsync("http://localhost:14678/api/" + nombreControladorAccion);

            return result;
        }

        public static async Task<HttpResponseMessage> PUT<T>(string nombreControladorAccion,T objeto, Token token) where T : class
        {

            HttpClient client = new HttpClient();

            if (token != null)
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);
            }

            HttpResponseMessage result = await client.PutAsync<T>("http://localhost:14678/api/" + nombreControladorAccion, objeto, new JsonMediaTypeFormatter());

            return result;
        }

        public static async Task<HttpResponseMessage> DELETE(string nombreControladorAccion, int ID, Token token)
        {

            HttpClient client = new HttpClient();

            if (token != null)
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);
            }

            HttpResponseMessage result = await client.DeleteAsync("http://localhost:14678/api/" + nombreControladorAccion + "/" + ID);

            return result;
        }

        public static async Task<List<T>> GET_List<T>(string nombreControladorAccion, Token token) where T : class
        {
            List<T> lista = null;

            HttpClient client = new HttpClient();

            if (token != null)
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);
            }

            HttpResponseMessage result = await client.GetAsync("http://localhost:14678/api/" + nombreControladorAccion);

            if (result.IsSuccessStatusCode) {

                lista = new List<T>();
                lista = await result.Content.ReadAsAsync<List<T>>();
            }

            return lista;
        }

        public static async Task<List<T>> GET_ListWOAuth<T>(string nombreControladorAccion) where T : class
        {
            List<T> lista = null;

            HttpClient client = new HttpClient();

            HttpResponseMessage result = await client.GetAsync("http://localhost:14678/api/" + nombreControladorAccion);

            if (result.IsSuccessStatusCode)
            {

                lista = new List<T>();
                lista = await result.Content.ReadAsAsync<List<T>>();
            }

            return lista;
        }

        public static async Task<List<T>> GET_List_ByFilter<T>(string nombreControladorAccion,string Filter, Token token) where T : class
        {
            List<T> lista = null;

            HttpClient client = new HttpClient();

            if (token != null)
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);
            }

            HttpResponseMessage result = await client.GetAsync("http://localhost:14678/api/" + nombreControladorAccion+"?"+Filter);

            if (result.IsSuccessStatusCode)
            {

                lista = new List<T>();
                lista = await result.Content.ReadAsAsync<List<T>>();
            }
            return lista;
        }

        public static async Task<List<T>> GET_ListById<T>(string nombreControladorAccion, int ID, Token token) where T : class
        {
            List<T> lista = null;

            HttpClient client = new HttpClient();

            if (token != null)
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);
            }

            HttpResponseMessage result = await client.GetAsync("http://localhost:14678/api/" + nombreControladorAccion + "/" + ID);

            if (result.IsSuccessStatusCode)
            {

                lista = new List<T>();
                lista = await result.Content.ReadAsAsync<List<T>>();
            }

            return lista;
        }
    }
}