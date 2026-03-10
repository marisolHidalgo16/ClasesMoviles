using AccesoDatos;
using Core.Entidades;
using Core.Enums;
using Core.Request;
using Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class LogPublicacion
    {
        public ResObtenerPublicaciones obtener (ReqObtenerPublicaciones req)
        {
            ResObtenerPublicaciones res = new ResObtenerPublicaciones();
            try
            {
                using (ConexionLinqDataContext linq = new ConexionLinqDataContext())
                {
                    res.listaDePublicaciones = this.factoriaListaPublicaciones(linq.SP_OBTENER_PUBLICACIONES().ToList());
                }//Aqui se muere Linq
            }
            catch (Exception e) {
                res.resultado = false;
                res.error.Add(Utilitarios.Utilitarios.crearError(enumErrores.errorNoControlado));
            }
            return res;
        }

        //La factoriiiiíaa!
        private List<Publicacion> factoriaListaPublicaciones(List<SP_OBTENER_PUBLICACIONESResult> listaTipoComplejo)
        {
            List<Publicacion> listaPublicaciones = new List<Publicacion>();

            foreach(SP_OBTENER_PUBLICACIONESResult cadaTipoComplejo in listaTipoComplejo)
            {
                Publicacion publicacion = new Publicacion();
                publicacion.guid = cadaTipoComplejo.GUID_PUBLICACION;
                publicacion.idTema = cadaTipoComplejo.GUID_TEMA;
                publicacion.idUsuario = cadaTipoComplejo.GUID_USUARIO;
                publicacion.titulo = cadaTipoComplejo.TITULO;
                publicacion.mensaje = cadaTipoComplejo.MENSAJE;
                publicacion.fechaPublicacion = cadaTipoComplejo.FECHA_REGISTRO;

                listaPublicaciones.Add(publicacion);
            }
            return listaPublicaciones;
        }

    }
}
