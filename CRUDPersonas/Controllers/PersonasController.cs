using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDPersonas.Controllers
{
    public class PersonasController : Controller
    {
        protected dbPersonasEntities GetEsquema()
        {
            return new dbPersonasEntities();
        }

        // GET: PersonasController
        public ActionResult Index()
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var listaPersonas = (from x in esquema.Personas
                                         select x).ToList();
                    return View(listaPersonas);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        // GET: PersonasController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PersonasController/Create
        public ActionResult Create()
        {
            return View("_FormularioPersonas");
        }

        // POST: PersonasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Personas persona)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    int fechaMayor = DateTime.Compare(persona.FechaNacimiento, persona.FechaInscripcion);
                    if (fechaMayor > 0)
                    {
                        TempData["mensaje"] = "La fecha de nacimiento no puede ser mayor a la fecha de incripcion.";
                        return RedirectToAction(nameof(Index));
                    }

                    if (persona.Edad < 18)
                    {
                        TempData["mensaje"] = "La persona debe tener mas de 18 años.";
                        return RedirectToAction(nameof(Index));
                    }

                    DateTime fechaActual = DateTime.Now;

                    // Sacando edad de la fecha nacimiento
                    TimeSpan duracionNac = fechaActual.Subtract(persona.FechaNacimiento);
                    int edad = (int)(duracionNac.TotalDays / 365.25);

                    //tiempo de inscripcion
                    TimeSpan duracionCosto = fechaActual.Subtract(persona.FechaInscripcion);
                    int aniosCosto = (int)(duracionCosto.TotalDays / 365.25);

                    if (persona.Edad != edad)
                    {
                        TempData["mensaje"] = "La edad no coincide con la fecha de nacimiento.";
                        return RedirectToAction(nameof(Index));
                    }

                    if (persona.Costo != aniosCosto * 100)
                    {
                        TempData["mensaje"] = "El costo no coincide con la fecha de inscripcion.";
                        return RedirectToAction(nameof(Index));
                    }

                    if (ValidarNombre(persona.NombreCompleto))
                    {
                        TempData["mensaje"] = "El nombre debe contener al menos dos nombres (nombre apellido) con 4 caracteres cada uno";
                        return RedirectToAction(nameof(Index));
                    }

                    esquema.Personas.Add(persona);
                    esquema.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
        }

        public bool ValidarNombre(string str)
        {
            // Divide el string en palabras separadas por espacios
            string[] palabras = str.Split(' ');

            int contador = 0;

            foreach (string palabra in palabras)
            {
                // Si la palabra tiene más de 4 caracteres, incrementa el contador
                if (palabra.Length > 4)
                {
                    contador++;
                }
            }

            // Retorna true si se encontraron al menos dos palabras con más de 4 caracteres
            return contador >= 2;
        }

        // GET: PersonasController/Edit/5
        public ActionResult Edit(int id)
        {
            using (var esquema = GetEsquema())
            {
                var persona = (from x in esquema.Personas
                                       where x.Id == id
                                       select x).FirstOrDefault();
                return View("_UpdatePersonas", persona);
            }
            
        }

        // POST: PersonasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Personas persona)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    int fechaMayor = DateTime.Compare(persona.FechaNacimiento, persona.FechaInscripcion);
                    if (fechaMayor > 0)
                    {
                        TempData["mensaje"] = "La fecha de nacimiento no puede ser mayor a la fecha de incripcion.";
                        return RedirectToAction(nameof(Index));
                    }

                    if (persona.Edad < 18)
                    {
                        TempData["mensaje"] = "La persona debe tener mas de 18 años.";
                        return RedirectToAction(nameof(Index));
                    }

                    DateTime fechaActual = DateTime.Now;

                    // Sacando edad de la fecha nacimiento
                    TimeSpan duracionNac = fechaActual.Subtract(persona.FechaNacimiento);
                    int edad = (int)(duracionNac.TotalDays / 365.25);

                    //tiempo de inscripcion
                    TimeSpan duracionCosto = fechaActual.Subtract(persona.FechaInscripcion);
                    int aniosCosto = (int)(duracionCosto.TotalDays / 365.25);

                    if (persona.Edad != edad)
                    {
                        TempData["mensaje"] = "La edad no coincide con la fecha de nacimiento.";
                        return RedirectToAction(nameof(Index));
                    }

                    if (persona.Costo != aniosCosto * 100)
                    {
                        TempData["mensaje"] = "El costo no coincide con la fecha de inscripcion.";
                        return RedirectToAction(nameof(Index));
                    }

                    if (ValidarNombre(persona.NombreCompleto))
                    {
                        TempData["mensaje"] = "El nombre debe contener al menos dos nombres (nombre apellido) con 4 caracteres cada uno";
                        return RedirectToAction(nameof(Index));
                    }

                    var personaAnterior = (from x in esquema.Personas
                                           where x.Id == id
                                           select x).FirstOrDefault();
                    personaAnterior.NombreCompleto = persona.NombreCompleto;
                    personaAnterior.Edad = persona.Edad;
                    personaAnterior.FechaNacimiento = persona.FechaNacimiento;
                    personaAnterior.FechaInscripcion = persona.FechaInscripcion;
                    personaAnterior.Costo = persona.Costo;
                    esquema.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: PersonasController/Delete/5
        public ActionResult Delete(int id)
        {
            using (var esquema = GetEsquema())
            {
                var persona = (from x in esquema.Personas
                                       where x.Id == id
                                       select x).FirstOrDefault();
                esquema.Personas.Remove(persona);
                esquema.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: PersonasController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Personas persona)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    esquema.Personas.Remove(persona);
                    esquema.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
