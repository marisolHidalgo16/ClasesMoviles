using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entidades
{
    public class Usuario
    {
        public string nombre {  get; set; }
        public string apellidos { get; set; }
        public string correoElectronico { get; set; }
        public string password { get; set; }

    }
}
