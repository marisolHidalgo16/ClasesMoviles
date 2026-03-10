using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Response
{
    public class ResBase
    {
        public bool resultado { get; set; }
        public List<Error> error { get; set; }
    }
}
