using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EncuestaViajesMVC.Models

{
    public class EncuestaModel
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Pais { get; set; }
        public string Rol { get; set; }
        public string DestinoPrimario { get; set; }
        public string DestinoSecundario { get; set; }
    }
}