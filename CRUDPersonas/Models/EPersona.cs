using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDPersonas.Models
{
    public class EPersona
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public int Edad { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public double Costo { get; set; }

        public EPersona()
        {

        }

        public EPersona(Personas persona)
        {
            this.NombreCompleto = persona.NombreCompleto;
            this.Edad = persona.Edad;
            this.FechaNacimiento = persona.FechaNacimiento;
            this.FechaInscripcion = persona.FechaInscripcion;
            this.Costo = persona.Costo;
        }
    }
}
