using Cdmx.Scg.Sancionados.Web.Models;
using System;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using Microsoft.AspNet.Identity;



namespace Cdmx.Scg.Sancionados.Web.Controllers
{
    public class CambiarPassController : CuentaController
    {
        // GET: CambiarPass
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CambiaPass(ChangePasswordViewModel model)
        {
            string url = "http://localhost:64381/api/CambiarPass";

            ChangePasswordViewModel oCambiarPass = new ChangePasswordViewModel();
            oCambiarPass.Iduser = Convert.ToInt32(User.Identity.GetUserId());
            //oCambiarPass.Name = User.Identity.Name;
            //oCambiarPass.OldPassword = model.OldPassword;
            string Pass = Encrypt.GetMD5(model.OldPassword);
            oCambiarPass.OldPassword = Pass;
            string cadenaEncriptada = Encrypt.GetMD5(model.NewPassword);
            oCambiarPass.NewPassword = cadenaEncriptada;
            string Recover = Send<ChangePasswordViewModel>(url, oCambiarPass, "POST");
            if (Recover == "1")
            {
                AuthenticationManager.SignOut();
                return RedirectToAction("Index", "Cuenta");
            }
            return View(model);            
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

        public class Encrypt
        {
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

}