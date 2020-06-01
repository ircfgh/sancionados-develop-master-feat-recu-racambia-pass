using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cdmx.Scg.Sancionados.Web.Models.ViewModels
{
    public class CambiaPassRecuperadoVM
    {
        public string token { get; set; }
        [Display(Name ="Contraseña")]
        [Required]
        public string Password { get; set; }
        [Display(Name = "Confirme Contraseña")]
        [Compare("Password")]
        [Required]
        public string Password2 { get; set; }
    }
}