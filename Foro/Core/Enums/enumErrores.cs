using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
    public enum enumErrores
    {
        errorNoControlado = -2,
        errorDeBaseDeDatos = -1, //No se hace
        nombreFaltante = 1,
        apellidosFaltantes = 2,
        correoFaltante = 3,
        reqNull = 4,
        correoNoValido = 5,
        passwordVacio = 6,
        passwordDebil = 7
    }
}