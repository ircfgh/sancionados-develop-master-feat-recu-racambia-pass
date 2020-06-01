using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace Cdmx.Scg.Sancionados.Web.Models
{
    public class TipoOrigenViewModel
    {

        [HiddenInput]
        [Key]
        [DefaultValue(-1)]
        [ScaffoldColumn(false)]
        [Display(Name = "Id")]
        public int IdTipoOrigen { get; set; }

        [Required]
        [Display(Name = "Tipo de Origen")]
        public string DescOrigen { get; set; }

        [ScaffoldColumn(false)]
        [DefaultValue(-1)]
        public int? IdUsuario { get; set; }

    }
}