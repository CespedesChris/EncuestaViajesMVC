using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EncuestaViajesMVC.Models
{
    public class DestinoResumen
    {
        public string Nombre { get; set; }
        public double Puntos { get; set; }
        public double Porcentaje { get; set; }
        public double Diferencia { get; set; }
    }
}