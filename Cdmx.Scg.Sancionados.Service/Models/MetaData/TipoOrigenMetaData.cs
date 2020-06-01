using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Cdmx.Scg.Sancionados.Service.Models.MetaData
{
    //public class TipoOrigenMetaData
    //{
    //}

    [MetadataType(typeof(TipoOrigen.TipoOrigenMetaData))]
    public partial class TipoOrigen
    {
        internal sealed class TipoOrigenMetaData
        {
            //#pragma warning disable 0649
            [Required]
            public int IdTipoOrigen { get; set; }
            
            [Required]
            [Display(Name = "Tipo de Origen")]
            public string DescOrigen { get; set; }

            
            //public int IdUsuario { get; set; }
            //public System.DateTime FechaRegistro { get; set; }
        }
    }

}