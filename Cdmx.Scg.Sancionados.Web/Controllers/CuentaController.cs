using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Cdmx.Scg.Sancionados.Web.Models;
using Cdmx.Scg.Sancionados.Web.Models.ViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Cdmx.Scg.Sancionados.Web.Controllers
{
    public class CuentaController : AppBase
    {
        // GET: Index
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Index
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(InicioSesionViewModel inicioSesionViewModel, string returnUrl)
        {
            var tokenUrl = string.Format("{0}{1}", ConfigurationManager.AppSettings["UriApiSSO"], "login/authenticate/");
            string clientId = "http://localhost:49915/";
            var jwtProvider = Providers.JwtProvider.Create(tokenUrl);
            string token = null;

            try
            {

                if (inicioSesionViewModel == null)
                {
                    ModelState.AddModelError(string.Empty, "No existen datos de tipo orígen");
                }
                else
                {
                    //Valida el modelo mediante los data annotation
                    if (ModelState.IsValid)
                    {

                        try
                        {
                            token = await jwtProvider.GetTokenAsync(inicioSesionViewModel.Usuario, inicioSesionViewModel.Contrasena, clientId, Environment.MachineName);

                        }
                        catch (AuthenticationException)
                        {
                            ModelState.AddModelError(string.Empty, "El usuario o contraseña son incorrectos");
                            return View(inicioSesionViewModel);
                        }

                        if (token != null)
                        {

                            //decode payload
                            dynamic payload = jwtProvider.DecodePayload(token);
                            //create an Identity Claim
                            ClaimsIdentity claims = jwtProvider.CreateIdentity(true, inicioSesionViewModel.Usuario, payload, token);

                            //sign in
                            var context = System.Web.HttpContext.Current.Request.GetOwinContext();
                            var authenticationManager = context.Authentication;
                            authenticationManager.SignIn(claims);


                            return RedirectToLocal(returnUrl);

                            ////AuthenticationProperties options = new AuthenticationProperties();

                            ////options.AllowRefresh = true;
                            ////options.IsPersistent = true;
                            ////options.ExpiresUtc = DateTime.UtcNow.AddSeconds(int.Parse(token.expires_in));

                            ////var claims = new[]
                            ////{
                            ////    new Claim(ClaimTypes.Name, model.),
                            ////    new Claim("AcessToken", string.Format("Bearer {0}", token.access_token)),
                            ////};

                            ////var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                            ///
                            ////Request.GetOwinContext().Authentication.SignIn(options, identity);

                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Error al intentar validar los datos, contacta al administrador");
                        }

                    }
                    //ModelState.AddModelError(string.Empty, "No existen datos de tipo orígen")

                }

                //regresa la respuesta en un objeto json
                return View(inicioSesionViewModel);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message + ((ex.InnerException != null) ? " " + ex.InnerException.Message : "") + ", contacta al administrador.");
                //Regresa la excepcion al cliente
                return View(inicioSesionViewModel);
            }
        }

        private IAuthenticationManager _authnManager;  // Add this private variable

        public IAuthenticationManager AuthenticationManager // Modified this from private to public and add the setter
        {
            get
            {
                if (_authnManager == null)
                    _authnManager = HttpContext.GetOwinContext().Authentication;
                return _authnManager;
            }
            set { _authnManager = value; }
        }

        // GET: Index
        [Authorize]
        public ActionResult CerrarSesion()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Recuperar()
        {
            Models.ViewModels.RecuperaPassMV model = new Models.ViewModels.RecuperaPassMV();
            return View(model);
        }

        [HttpPost]
        public ActionResult Recuperar(RecuperaPassMV model)
        {
            string url = "http://localhost:64381/api/RecuperaPass";

            RecuperaPassMV oRecuperaPass = new RecuperaPassMV();
            oRecuperaPass.Email = model.Email;
            string Resp = Send<RecuperaPassMV>(url, oRecuperaPass, "POST");
            if (Resp == "1")
            {
                AuthenticationManager.SignOut();
                return RedirectToAction("Index", "Cuenta");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Recovery(string token)
        {
            Models.ViewModels.CambiaPassRecuperadoVM model = new Models.ViewModels.CambiaPassRecuperadoVM();
            model.token = token;
            return View(model);
        }

        [HttpPost]
        public ActionResult Recovery(CambiaPassRecuperadoVM model)
        {
            string url = "http://localhost:64381/api/CambiarPassRecuperado";

            CambiaPassRecuperadoVM oCambiarPassRecuperado = new CambiaPassRecuperadoVM();
            oCambiarPassRecuperado.token = model.token;                                    
            string Pass = GetMD5(model.Password);
            oCambiarPassRecuperado.Password = Pass;
            string Recover = Send<CambiaPassRecuperadoVM>(url, oCambiarPassRecuperado, "POST");
            if (Recover == "1")
            {
                AuthenticationManager.SignOut();
                return RedirectToAction("Index", "Cuenta");
            }
            return View();
        }

        public string Send<T>(string url, T objectRequest, string method = "POST")
        {
            string result = "";

            try
            {

                JavaScriptSerializer js = new JavaScriptSerializer();

                //serializamos el objeto
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(objectRequest);

                //peticion
                WebRequest request = WebRequest.Create(url);
                //headers
                request.Method = method;
                request.PreAuthenticate = false;
                request.ContentType = "application/json;charset=utf-8'";
                request.Timeout = 10000; //esto es opcional

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

            }
            catch (Exception e)
            {
                result = e.Message;

            }

            return result;
        }

        public static string GetMD5(string str)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = md5.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
    }
}
