using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entidades
{
    public class Publicacion
    {
        public int id {  get; set; }
        public Guid guid { get; set; }
        public Guid? idTema { get; set; }
        public Guid idUsuario { get; set; }
        public string titulo { get; set; }
        public string mensaje { get; set; }
        public DateTime? fechaPublicacion { get; set; }

    }
}
