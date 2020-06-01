using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace Cdmx.Scg.Sancionados.Web
{
    public class AppBase : Controller
    {

        private string UriApiSancionados { get; set; }
        private string UriApiSSO { get; set; }

        /// <summary>
        /// Propiedad para obtener el Identificador del Usuario Autenticado
        /// </summary>
        //public int IdUsuario { get { return 1; } }
        public int IdUsuario
        {
            get
            {
                string strIdUsario = ((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                                        .Select(c => c.Value).SingleOrDefault();

                return Convert.ToInt32(strIdUsario ?? "0");
            }
        }

        public enum WebApis
        {
            ApiSancionados = 1,
            UriApiSSO = 2
        }

        public enum TipoPeticion
        { 
            Get
        }

        public enum TipoMantenimiento
        {
            Post,
            Put,
        }

        public enum TipoBaja
        {
            Delete
        }

        public AppBase()
        {
            this.UriApiSancionados = ConfigurationManager.AppSettings["UriApiSancionados"];
            this.UriApiSSO = ConfigurationManager.AppSettings["UriApiSSO"];
        }

        public HttpResponseMessage LLamarApi<T>(WebApis webApi, TipoMantenimiento tipoMantenimiento, string strControlador, T entidadViewModel)
        {
            HttpResponseMessage result = null;
            string strUri = string.Empty;

            switch (webApi)
            {
                case WebApis.ApiSancionados:
                    strUri = this.UriApiSancionados;
                    break;
                case WebApis.UriApiSSO:
                    strUri = this.UriApiSSO;
                    break;
                default:
                    strUri = string.Empty;
                    break;
            }

            
            using (var client = new HttpClient())
            {
                Task<HttpResponseMessage> resultTask = null;
                client.BaseAddress = new Uri(strUri);

                //Valida el tipo de peticion
                switch (tipoMantenimiento)
                {
                    case TipoMantenimiento.Post:
                        resultTask = client.PostAsJsonAsync<T>(strControlador, entidadViewModel);
                        break;
                    case TipoMantenimiento.Put:
                        resultTask = client.PutAsJsonAsync<T>(strControlador, entidadViewModel);
                        break;
                    default:
                        resultTask = client.PostAsJsonAsync<T>(strControlador, entidadViewModel);
                        break;
                }

                //var resultTask = client.PostAsJsonAsync<T>(strControlador, entidadViewModel);                
                resultTask.Wait();
                result = resultTask.Result;                
            }

            return result;
        }

        public HttpResponseMessage LLamarApi(WebApis webApi, TipoPeticion tipoPeticion, string strControlador)
        {
            HttpResponseMessage result = null;
            string strUri = string.Empty;

            switch (webApi)
            {
                case WebApis.ApiSancionados:
                    strUri = this.UriApiSancionados;
                    break;
                case WebApis.UriApiSSO:
                    strUri = this.UriApiSSO;
                    break;
                default:
                    strUri = string.Empty;
                    break;
            }


            using (var client = new HttpClient())
            {
                Task<HttpResponseMessage> resultTask = null;
                client.BaseAddress = new Uri(strUri);

                resultTask = client.GetAsync(strControlador);

                resultTask.Wait();
                result = resultTask.Result;
            }

            return result;
        }

        public HttpResponseMessage LLamarApi(WebApis webApi, TipoBaja tipoBaja, string strControlador)
        {
            HttpResponseMessage result = null;
            string strUri = string.Empty;

            switch (webApi)
            {
                case WebApis.ApiSancionados:
                    strUri = this.UriApiSancionados;
                    break;
                case WebApis.UriApiSSO:
                    strUri = this.UriApiSSO;
                    break;
                default:
                    strUri = string.Empty;
                    break;
            }


            using (var client = new HttpClient())
            {
                Task<HttpResponseMessage> resultTask = null;
                client.BaseAddress = new Uri(strUri);

                resultTask = client.DeleteAsync(strControlador);

                resultTask.Wait();
                result = resultTask.Result;
            }

            return result;
        }

    }

}