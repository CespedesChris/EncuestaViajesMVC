using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using EncuestaViajesMVC.Models;


namespace EncuestaViajesMVC.Controllers
{
    public class HomeController : Controller
    {
        private static readonly List<string> destinos = new List<string> { "París", "Tokio", "Nueva York", "Roma", "Londres", "Monteverde", "Volcán Irazú", "Volcán Poás", "Playa Panamá", "Montezuma", "Piramides", "Cancún", "Torre Eiffel", "Isla del Coco", "Medellín", "Bogota", "Cristo de Esquipulas", "Tierra Santa", "Egypto","Holanda" };
        private static readonly string datosPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/datos.json");

        //        public ActionResult Index()
        //        {
        //            if (Session["Resultados"] == null)
        //            {
        //                var resumen = destinos.Select(d => new DestinoResumen { Nombre = d, Puntos = 0 }).ToList();
        //                Session["Resultados"] = resumen;
        //            }
        //            return View();
        //        }

        public ActionResult Index()
        {
            if (Session["Resultados"] == null)
            {
                var resumen = destinos.Select(d => new DestinoResumen { Nombre = d, Puntos = 0 }).ToList();

                // Leer archivo JSON y sumar puntos
                if (System.IO.File.Exists(datosPath))
                {
                    var contenido = System.IO.File.ReadAllText(datosPath);
                    var encuestas = JsonConvert.DeserializeObject<List<EncuestaModel>>(contenido);

                    if (encuestas != null)
                    {
                        foreach (var encuesta in encuestas)
                        {
                            if (resumen.Any(r => r.Nombre == encuesta.DestinoPrimario))
                                resumen.First(r => r.Nombre == encuesta.DestinoPrimario).Puntos += 1;

                            if (resumen.Any(r => r.Nombre == encuesta.DestinoSecundario))
                                resumen.First(r => r.Nombre == encuesta.DestinoSecundario).Puntos += 0.5;
                        }
                    }
                }

                Session["Resultados"] = resumen;
            }

            return View();
        }


        public ActionResult Agregar()
        {
            ViewBag.Destinos = destinos;
            ViewBag.Roles = new List<string> { "Turista", "Viajero de negocios", "Estudiante", "Investigador", "Otro" };
            ViewBag.Paises = new List<string> { "Costa Rica", "México", "Honduras", "Canada", "España", "Nicaragua", "Estados Unidos", "Argentina" };
            return View();
        }


        [HttpPost]

        public JsonResult GuardarAjax(EncuestaModel encuesta)
        {
            if (string.IsNullOrEmpty(encuesta.Nombre) ||
    string.IsNullOrEmpty(encuesta.Apellido) ||
    string.IsNullOrEmpty(encuesta.Pais) ||
    string.IsNullOrEmpty(encuesta.Rol) ||
    string.IsNullOrEmpty(encuesta.DestinoPrimario) ||
    string.IsNullOrEmpty(encuesta.DestinoSecundario))
            {
                return Json(new { success = false, message = "Debe completar todos los campos" });
            }

            var resumen = Session["Resultados"] as List<DestinoResumen>;
            if (resumen == null)
            {
                resumen = destinos.Select(d => new DestinoResumen { Nombre = d, Puntos = 0 }).ToList();
            }

            resumen.First(x => x.Nombre == encuesta.DestinoPrimario).Puntos += 1;
            resumen.First(x => x.Nombre == encuesta.DestinoSecundario).Puntos += 0.5;
            Session["Resultados"] = resumen;

            GuardarJSON(encuesta);

            return Json(new { success = true });

        }

        [HttpGet]

        public JsonResult ObtenerResumen()
        {
            var lista = Session["Resultados"] as List<DestinoResumen>;
            CalcularPorcentajes(lista);
            // Ordenamos nuevamente para asegurar que se envíe correctamente
            var ordenada = lista.OrderByDescending(x => x.Porcentaje).Take(20).ToList();
            return Json(ordenada, JsonRequestBehavior.AllowGet);


            //lista = lista.OrderByDescending(x => x.Porcentaje).Take(20).ToList();
           // return Json(lista, JsonRequestBehavior.AllowGet);
        }

        private void GuardarJSON(EncuestaModel encuesta)
        {
            var encuestas = new List<EncuestaModel>();

            if (System.IO.File.Exists(datosPath))
            {
                var contenido = System.IO.File.ReadAllText(datosPath);
                encuestas = JsonConvert.DeserializeObject<List<EncuestaModel>>(contenido) ?? new List<EncuestaModel>();
            }

            encuestas.Add(encuesta);
            System.IO.File.WriteAllText(datosPath, JsonConvert.SerializeObject(encuestas, Formatting.Indented));
        }

        private void CalcularPorcentajes(List<DestinoResumen> lista)
        {
            var total = lista.Sum(x => x.Puntos);
            if (total == 0) return;


            foreach (var destino in lista)
            {
                destino.Porcentaje = (destino.Puntos / total) * 100;
            }
            // Ordenamos por porcentaje antes de calcular la diferencia
            var ordenada = lista.OrderByDescending(x => x.Porcentaje).ToList();



            //for (int i = 0; i < lista.Count; i++)
            for (int i = 0; i< ordenada.Count;i++)
            {
                ordenada[i].Diferencia = i == 0 ? 0 : ordenada[i].Porcentaje - ordenada[i - 1].Porcentaje;
                //       lista[i].Porcentaje = (lista[i].Puntos / total) * 100;
                //       lista[i].Diferencia = i == 0 ? 0 : lista[i].Porcentaje - lista[i - 1].Porcentaje;
            }
        }
    }
}







