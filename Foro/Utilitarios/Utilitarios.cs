using AccesoDatos;
using Core.Entidades;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Utilitarios
{
    public static class Utilitarios
    {
        public static Error crearError(enumErrores enumErrores)
        {
            Error error = new Error();
            error.codigo = enumErrores;
            error.mensaje = enumErrores.ToString();

            return error;
        }

        public static void crearBitacora(string clase, string metodo, short tipo, int error, string descripcion, string request, string response)
        {
            try
            {
                using (ConexionLinqDataContext linq = new ConexionLinqDataContext())
                {
                    linq.SP_INSERTAR_BITACORA(clase,metodo,tipo,error,descripcion,request,response);
                }
            }
            catch (Exception ex)
            {
                //Bitacorear en .txt
            }
        }

        public static bool EnviarCorreoVerificacion(string nombre, string correo, string codigoVerificacion)
        {
            try
            {
                string asunto = "Confirma tu cuenta";
                string cuerpo = $@"
    <html>
    <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 30px;'>
        <div style='max-width: 500px; margin: auto; background-color: #ffffff; border-radius: 10px; padding: 30px; text-align: center; box-shadow: 0 2px 10px rgba(0,0,0,0.1);'>
            <h2 style='color: #2c3e50;'>¡Bienvenido, {nombre}!</h2>
            <p style='color: #555; font-size: 15px;'>Gracias por registrarte. Usa el siguiente código para verificar tu cuenta:</p>
            <div style='margin: 25px 0; padding: 15px; background-color: #3498db; border-radius: 8px;'>
                <span style='font-size: 28px; font-weight: bold; color: #ffffff; letter-spacing: 6px;'>{codigoVerificacion}</span>
            </div>
            <p style='color: #999; font-size: 13px;'>Si no creaste esta cuenta, ignora este mensaje.</p>
            <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;'/>
            <p style='color: #bbb; font-size: 11px;'>© 2025 MiApp. Todos los derechos reservados.</p>
        </div>
    </body>
    </html>";

                var mensaje = new MailMessage
                {
                    From = new MailAddress("tucorreo@gmail.com", "MiApp"),
                    Subject = asunto,
                    Body = cuerpo,
                    IsBodyHtml = true
                };
                mensaje.To.Add(correo);

                var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("tucorreo@gmail.com", "2132 43434 4343 4343"),
                    EnableSsl = true
                };

                smtp.Send(mensaje);
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}
