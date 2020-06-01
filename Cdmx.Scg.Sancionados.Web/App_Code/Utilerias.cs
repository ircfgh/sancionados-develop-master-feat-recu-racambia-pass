using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Cdmx.Scg.Sancionados.Web
{
    /// <summary>
    /// Clase que contiene un conjunto de funciones para realizar propositos generales en el aplicativo
    /// como enviar mensajes, encriptar, sanitizar cadenas
    /// </summary>
    public static class Utilerias
    {

        /// <summary>
        /// Define las opciones para los mensajes de regreso al cliente
        /// </summary>
        public enum TipoRespuesta
        {
            Exito = 1,
            Advertencia = 2,
            Error = 3,
            Info = 4
        }

        /// <summary>
        /// Funcion para limpiar los caracteres especiales que no se pueden mostrar en javascript
        /// </summary>
        /// <param name="strCadena">Cadeque que se desea limpiar</param>
        /// <returns>Cadena limpia</returns>
        public static string LimpiarCaracteresEspeciales(string strCadena)
        {
            return strCadena.Replace("'", "").Replace("\"", "").Replace("\n", " ").Replace("\r", " ").Replace(Environment.NewLine, " ");
        }

        /// <summary>
        /// Genera la lista anonima para desplegar los mensajes mediante ajax
        /// </summary>
        /// <param name="tipoRespuesta">Enumerado del tipo de respuesta</param>
        /// <param name="modelState">Estado del modelo donde capturo la información</param>
        /// <returns>Una lista con el tipo de mensaje y la descripcion del mensaje</returns>
        public static object GenerarMensaje(TipoRespuesta tipoRespuesta, ModelStateDictionary modelState)
        {
            //Concatena los distintos errores que se generaron en el estado del modelo
            string messages = string.Join("<br/ > ", modelState.Values
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage));

            return new { estatus = tipoRespuesta.ToString(), mensaje = string.IsNullOrWhiteSpace(messages) ? "Capture los campos requeridos" : LimpiarCaracteresEspeciales(messages) };
        }

        /// <summary>
        /// Genera la lista anonima para desplegar los mensajes mediante ajax
        /// </summary>
        /// <param name="tipoRespuesta">Enumerado del tipo de respuesta</param>
        /// <param name="strMensaje">Descripcion del mensaje que se va desplegar</param>
        /// <returns>Una lista con el tipo de mensaje y la descripcion del mensaje</returns>
        public static object GenerarMensaje(TipoRespuesta tipoRespuesta, string strMensaje)
        {
            return new { estatus = tipoRespuesta.ToString(), mensaje = LimpiarCaracteresEspeciales(strMensaje) };
        }

        /// <summary>
        /// Genera la lista anonima para desplegar los mensajes mediante ajax
        /// </summary>
        /// <param name="tipoRespuesta">Enumerado del tipo de respuesta</param>
        /// <param name="strMensaje">Descripcion del mensaje que se va desplegar</param>
        /// <param name="strUrlDescarga">Url de Descarga de archivos</param>
        /// <returns>Una lista con el tipo de mensaje y la descripcion del mensaje</returns>
        public static object GenerarMensaje(TipoRespuesta tipoRespuesta, string strMensaje, string strUrlDescarga)
        {
            return new { estatus = tipoRespuesta.ToString(), mensaje = LimpiarCaracteresEspeciales(strMensaje), url = strUrlDescarga };
        }

        /////// <summary>
        /////// Remueve las propiedades que no son contenidas en la coleccion del formulario y en el modelo si existen.
        /////// </summary>
        /////// <param name="formColeccion">Coleccion de las propiedades que se capturaron en el formulario</param>
        /////// <param name="modelState">Estatus del modelo capturado</param>
        /////// <remarks>Se usa al momento de tener errores de propiedad que son heredadas y no se necesita que se validen</remarks>
        ////public static void RemoverPropiedad(ModelStateDictionary modelState, FormCollection formColeccion)
        ////{
        ////    //Recorre el estado del modelo
        ////    foreach (string strPropiedad in modelState.Keys)
        ////    {
        ////        //Valida si la coleccion contiene la forma del modelo
        ////        if (!formColeccion.Keys.Contains(strPropiedad))
        ////            //Remueve la propiedad del model estate para que no se valide dicha propiedad
        ////            modelState.Remove(strPropiedad);
        ////    }
        ////}

        /// <summary>
        /// Sanitiza una cadena, los caracteres que limpiara son  ?&^$#@!()+-,:;<>’\'-_*
        /// </summary>
        /// <param name="strIn">Cadena que se revisara y limpiara</param>
        /// <returns>Cadena Limpia</returns>
        public static String SanitizarCadena(string strIn)
        {
            var hashSet = new HashSet<char>(@" ?&^$#@!()+-,:;<>’\'-_*/\");
            StringBuilder sb = new StringBuilder(strIn.Length);

            foreach (char x in strIn.Where(c => !hashSet.Contains(c)))
                sb.Append(x);

            return sb.ToString();
        }

        /// <summary>
        /// Genera la ruta de descarga del sitio para enviala al cliente
        /// </summary>
        /// <param name="strHost">host del aplicativo (this.Request.Host))</param>
        /// <param name="strFolderDescarga">Folder temporal de descarga de documentos</param>
        public static string GenerarRutaDescarga(string strHost, string strFolderDescarga)
        {
            return string.Format("http://{0}{1}", strHost, strFolderDescarga);
        }

    }    

}