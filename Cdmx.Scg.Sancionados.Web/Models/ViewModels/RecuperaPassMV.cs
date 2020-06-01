using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cdmx.Scg.Sancionados.Web.Models.ViewModels
{
    public class RecuperaPassMV
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}