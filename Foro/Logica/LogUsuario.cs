using AccesoDatos;
using Core.Entidades;
using Core.Enums;
using Core.Request;
using Core.Response;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logica
{
    public class LogUsuario
    {
        public ResIngresarUsuario insertar(ReqIngresarUsuario req)
        {
            ResIngresarUsuario res = new ResIngresarUsuario();

            try
            {
                if (req == null)
                {
                    res.resultado = false;
                    res.error.Add(Utilitarios.Utilitarios.crearError(enumErrores.reqNull));
                }
                if (String.IsNullOrEmpty(req.usuario.nombre))
                {
                    res.resultado = false;
                    res.error.Add(Utilitarios.Utilitarios.crearError(enumErrores.nombreFaltante));
                }
                if (String.IsNullOrEmpty(req.usuario.apellidos))
                {
                    res.resultado = false;
                    res.error.Add(Utilitarios.Utilitarios.crearError(enumErrores.apellidosFaltantes));
                }
                if (String.IsNullOrEmpty(req.usuario.correoElectronico))
                {
                    res.resultado = false;
                    res.error.Add(Utilitarios.Utilitarios.crearError(enumErrores.correoFaltante));
                }
                else if (EsCorreoValido(req.usuario.correoElectronico))
                {
                    res.resultado = false;
                    res.error.Add(Utilitarios.Utilitarios.crearError(enumErrores.correoNoValido));
                }
                if (String.IsNullOrEmpty(req.usuario.password))
                {
                    res.resultado = false;
                    res.error.Add(Utilitarios.Utilitarios.crearError(enumErrores.passwordVacio));
                }
                else if (EsPasswordFuerte(req.usuario.password))
                {
                    res.resultado = false;
                    res.error.Add(Utilitarios.Utilitarios.crearError(enumErrores.passwordDebil));
                }

                //¿hay errores?
                if (res.error.Any())
                {
                    //Hay errores
                    return res;
                }
                else
                {
                    Guid? guidUsuario = Guid.Empty;
                    int? idReturn = 0;
                    int? errorId = 0;
                    string errorDescripcion = "";

                    string codigoVerificacion = GenerarCodigo();

                    //No hay errores ¡enviar a la base de datos!
                    using (ConexionLinqDataContext miLinq = new ConexionLinqDataContext())
                    {
                        miLinq.SP_INGRESAR_USUARIO(
                        req.usuario.nombre,
                        req.usuario.apellidos,
                        req.usuario.correoElectronico,
                        req.usuario.password,
                        codigoVerificacion, //Esto hay que cambiarlo
                        ref guidUsuario,
                        ref idReturn,
                        ref errorId,
                        ref errorDescripcion);
                    }
                    if (idReturn <= 0)
                    {
                        //Hay un error de base de datos
                        res.resultado = false;
                        res.error.Add(Utilitarios.Utilitarios.crearError(enumErrores.errorDeBaseDeDatos));
                    }
                    else
                    {
                        //Todo good
                        if(Utilitarios.Utilitarios.EnviarCorreoVerificacion(req.usuario.nombre, req.usuario.correoElectronico, codigoVerificacion))
                        {
                            //Se envio
                            res.resultado =true;
                        }
                        else
                        {
                            //No se envio
                            res.resultado =false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Error de conexion
                res.resultado = false;
                res.error.Add(Utilitarios.Utilitarios.crearError(enumErrores.errorNoControlado));
            }
            finally
            {
                short tipo = 2;

                if (res.resultado){
                    tipo = 1;//Exito
                }
     

                Utilitarios.Utilitarios.crearBitacora(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, tipo,0,"", JsonConvert.SerializeObject(req), JsonConvert.SerializeObject(res));
            }
            return res;
        }
 

        private static bool EsCorreoValido(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
                return false;

            try
            {
                var mail = new MailAddress(correo);
                return mail.Address == correo;
            }
            catch
            {
                return false;
            }
        }

        private static bool EsPasswordFuerte(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            // Mínimo 8 caracteres, 1 mayúscula, 1 minúscula, 1 número y 1 carácter especial
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$");
        }

        private static string GenerarCodigo()
        {
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Range(0, 6).Select(_ => caracteres[random.Next(caracteres.Length)]).ToArray());
        }
    }
}
