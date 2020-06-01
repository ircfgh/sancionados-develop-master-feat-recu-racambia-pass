using Cdmx.Scg.Sancionados.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Cdmx.Scg.Sancionados.Web.Areas.Catalogos.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR")]
    public class TipoOrigenController : AppBase
    {
        /// <summary>
        /// Responde con la vista principal de busqueda
        /// </summary>
        /// <returns>Vista solicitada</returns>
        /// <example>GET: Catalogos/TipoOrigen</example>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Responde con la vista para crear un registro
        /// </summary>
        /// <returns>Vista solicitada</returns>
        /// <example>GET: Catalogos/TipoOrigen/Crear</example>
        [AjaxOnly]
        [HttpGet]
        public ActionResult Crear()
        {
            return PartialView();
        }

        /// <summary>
        /// Crea un tipo de origen
        /// </summary>
        /// <param name="tipoOrigenViewModel">Entidad del tipo de origen</param>
        /// <param name="formCollection">Valores del formulario enviado desde el cliente</param>
        /// <returns>Objeto JSON que indica el estatus del resultado y su mensaje correspondiente</returns>
        /// <example>POST: Catalogos/TipoOrigen/Crear</example>
        [AjaxOnly]
        [HttpPost]
        public JsonResult Crear(TipoOrigenViewModel tipoOrigenViewModel, FormCollection formCollection)
        {
            try
            {
                JsonResult jsnRespuesta;
                HttpResponseMessage hrmRespuestaApi;
                
                //Valida que se haya enviado la entidad via post
                if (tipoOrigenViewModel == null)
                {
                    jsnRespuesta = Json(Utilerias.GenerarMensaje(Utilerias.TipoRespuesta.Exito, Resources.Mensajes.GeneralErrorInfoFormulario));
                    //ModelState.AddModelError(string.Empty, "No existen datos de tipo orígen");
                }
                else {

                    //Asigna el usuario que esta creando el registro
                    tipoOrigenViewModel.IdUsuario = this.IdUsuario;

                    //Valida el modelo mediante los data annotation
                    if (ModelState.IsValid)
                    {
                        //Hace la llamada a la api enviandole la informacion del modelo
                        hrmRespuestaApi = LLamarApi<TipoOrigenViewModel>(WebApis.ApiSancionados, TipoMantenimiento.Post, "TipoOrigen", tipoOrigenViewModel);
                        
                        //Valida si la accion fue realizada de forma correcta
                        if (hrmRespuestaApi.IsSuccessStatusCode)
                        {
                            jsnRespuesta = Json(Utilerias.GenerarMensaje(Utilerias.TipoRespuesta.Exito, Resources.Mensajes.GeneralGuardar));
                        }
                        else //Si la webapi envio error aqui se valida
                        {
                            //log response status here..
                            //Si existe error, entonces se envia un error generico y el estatus de error que regreseo la API
                            jsnRespuesta = Json(Utilerias.GenerarMensaje(Utilerias.TipoRespuesta.Advertencia, string.Format(Resources.Mensajes.GeneralErrorGuardar, hrmRespuestaApi.StatusCode)));
                        }                        
                           
                    }
                    else
                    {
                        //Si el modelo no es valido, entonces se regresa al cliente los errores del modelo
                        jsnRespuesta = Json(Utilerias.GenerarMensaje(Utilerias.TipoRespuesta.Advertencia, ModelState));
                    }

                }

                //regresa la respuesta en un objeto json
                return jsnRespuesta;
            }
            catch (Exception ex)
            {
                //Regresa la excepcion al cliente
                return Json(Utilerias.GenerarMensaje(Utilerias.TipoRespuesta.Error, ex.Message));
            }
        }

        /// <summary>
        /// Genera la vista y completa los campos con la entidad solicitada
        /// </summary>
        /// <param name="id">Identificador de la entidad</param>
        /// <returns>Vista solicitada</returns>
        /// <example>GET: Catalogos/TipoOrigen/Editar/5</example>
        [AjaxOnly]
        [HttpGet]
        public ActionResult Editar(int id)
        {
            try
            {
                TipoOrigenViewModel tipoOrigenViewModel = null;
                HttpResponseMessage hrmRespuestaApi;

                //Hace la llamada a la api enviandole la informacion del modelo
                hrmRespuestaApi = LLamarApi(WebApis.ApiSancionados, TipoPeticion.Get, "TipoOrigen/" + id.ToString());

                if (hrmRespuestaApi.IsSuccessStatusCode)
                {
                    var readTask = hrmRespuestaApi.Content.ReadAsAsync<TipoOrigenViewModel>();
                    readTask.Wait();

                    tipoOrigenViewModel = readTask.Result;

                    return PartialView(tipoOrigenViewModel);
                }
                else {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(Utilerias.GenerarMensaje(Utilerias.TipoRespuesta.Error, Resources.Mensajes.GeneralConsultarNoExisteId), JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(Utilerias.GenerarMensaje(Utilerias.TipoRespuesta.Error, ex.Message), JsonRequestBehavior.AllowGet); 
            }            
          
        }

        /// <summary>
        /// Modifica la informacion de una entidad
        /// </summary>
        /// <param name="id">Identificador de la entidad</param>
        /// <param name="tipoOrigenViewModel">Informacion de la entidad</param>
        /// <returns>Objeto JSON que indica el estatus del resultado y su mensaje correspondiente</returns>
        /// <example>POST: Catalogos/TipoOrigen/Editar/5</example>
        [AjaxOnly]
        [HttpPost]
        public JsonResult Editar(int id, TipoOrigenViewModel tipoOrigenViewModel, FormCollection formCollection)
        {

            try
            {
                JsonResult jsnRespuesta;
                HttpResponseMessage hrmRespuestaApi;

                //Valida que se haya enviado la entidad via post
                if (tipoOrigenViewModel == null)
                {
                    jsnRespuesta = Json(Utilerias.GenerarMensaje(Utilerias.TipoRespuesta.Exito, Resources.Mensajes.GeneralErrorInfoFormulario));
                }
                else
                {
                    //Asigna el usuario que esta creando el registro
                    tipoOrigenViewModel.IdUsuario = this.IdUsuario;

                    //Valida el modelo mediante los data annotation
                    if (ModelState.IsValid)
                    {
                        //Hace la llamada a la api enviandole la informacion del modelo
                        hrmRespuestaApi = LLamarApi<TipoOrigenViewModel>(WebApis.ApiSancionados, TipoMantenimiento.Put, "TipoOrigen/" + id.ToString(), tipoOrigenViewModel);

                        //Valida si la accion fue realizada de forma correcta
                        if (hrmRespuestaApi.IsSuccessStatusCode)
                        {
                            jsnRespuesta = Json(Utilerias.GenerarMensaje(Utilerias.TipoRespuesta.Exito, Resources.Mensajes.GeneralActualizar));
                        }
                        else //Si la webapi envio error aqui se valida
                        {
                            //log response status here..
                            //Si existe error, entonces se envia un error generico y el estatus de error que regreseo la API
                            jsnRespuesta = Json(Utilerias.GenerarMensaje(Utilerias.TipoRespuesta.Advertencia, string.Format(Resources.Mensajes.GeneralErrorActualizar, hrmRespuestaApi.StatusCode)));
                        }

                    }
                    else
                    {
                        //Si el modelo no es valido, entonces se regresa al cliente los errores del modelo
                        jsnRespuesta = Json(Utilerias.GenerarMensaje(Utilerias.TipoRespuesta.Advertencia, ModelState));
                    }

                }

                //regresa la respuesta en un objeto json
                return jsnRespuesta;
            }
            catch (Exception ex)
            {
                //Regresa la excepcion al cliente
                return Json(Utilerias.GenerarMensaje(Utilerias.TipoRespuesta.Error, ex.Message));
            }
            
        }

        /// <summary>
        /// Elimina un registro de una entidad
        /// </summary>
        /// <param name="id">Identificador del registro</param>
        /// <returns>Objeto JSON que indica el estatus del resultado y su mensaje correspondiente</returns>
        /// <example>GET: Catalogos/TipoOrigen/Eliminar/5</example>
        [AjaxOnly]
        public JsonResult Eliminar(int id)
        {
            
            try
            {
                JsonResult jsnRespuesta;
                HttpResponseMessage hrmRespuestaApi;

                //Hace la llamada a la api enviandole la informacion del modelo
                hrmRespuestaApi = LLamarApi(WebApis.ApiSancionados, TipoBaja.Delete, "TipoOrigen/" + id.ToString());

                if (hrmRespuestaApi.IsSuccessStatusCode)
                {
                    jsnRespuesta = Json(Utilerias.GenerarMensaje(Utilerias.TipoRespuesta.Exito, Resources.Mensajes.GeneralEliminar));
                }
                else //Si la webapi envio error aqui se valida
                {
                    //log response status here..
                    //Si existe error, entonces se envia un error generico y el estatus de error que regreseo la API
                    jsnRespuesta = Json(Utilerias.GenerarMensaje(Utilerias.TipoRespuesta.Advertencia, string.Format(Resources.Mensajes.GeneralErrorEliminar, hrmRespuestaApi.StatusCode)));
                }

                return Json(jsnRespuesta, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(Utilerias.GenerarMensaje(Utilerias.TipoRespuesta.Error, ex.Message), JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AjaxOnly]
        public JsonResult Listar()
        {           

            try
            {
                IEnumerable<TipoOrigenViewModel> iETipoOrigenViewModel = null;
                HttpResponseMessage hrmRespuestaApi;

                //Hace la llamada a la api enviandole la informacion del modelo
                hrmRespuestaApi = LLamarApi(WebApis.ApiSancionados, TipoPeticion.Get, "TipoOrigen/");

                if (hrmRespuestaApi.IsSuccessStatusCode)
                {
                    var readTask = hrmRespuestaApi.Content.ReadAsAsync<IList<TipoOrigenViewModel>>();
                    readTask.Wait();

                    iETipoOrigenViewModel = readTask.Result;

                    return Json(iETipoOrigenViewModel, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(Utilerias.GenerarMensaje(Utilerias.TipoRespuesta.Error, Resources.Mensajes.GeneralConsultarNoDatos), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(Utilerias.GenerarMensaje(Utilerias.TipoRespuesta.Error, ex.Message), JsonRequestBehavior.AllowGet);
            }            
        }               

    }
}