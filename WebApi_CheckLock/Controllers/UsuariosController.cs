using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using WebApi_CheckLock.Models;
using static WebApi_CheckLock.Models.UsuariosModelo;
using static WebApi_CheckLock.Utilidades;

namespace WebApi_CheckLock.Controllers
{
    public class UsuariosController : ApiController
    {
        public HttpResponseMessage Post([FromBody] AltaUsuarioRequest oParam)
        {
            HttpResponseMessage oResponseMessage = new HttpResponseMessage();
            AltaUsuarioResponse oResponse = new AltaUsuarioResponse();
            UsuariosModelo oProceso = new UsuariosModelo();

            try
            {
                if (oParam != null)
                {
                    GrabaLog("Entrando a Metodo AltaUsuario", "POST");
                    var jsonRequestMessage = JsonConvert.SerializeObject(oParam);
                    GrabaLog("Request Message: " + jsonRequestMessage, "POST");
                    oResponse = oProceso.AltaUsuario(oParam);

                    if (oResponse.CodigoRespuesta == "00")
                    {
                        var jsonResponseMessage = JsonConvert.SerializeObject(oResponse);
                        GrabaLog("Response Message: " + jsonResponseMessage, "POST");
                        oResponseMessage = Request.CreateResponse(HttpStatusCode.OK, oResponse);
                    }
                    else
                    {
                        var jsonResponseMessage = JsonConvert.SerializeObject(oResponse);
                        GrabaLog("Response Message: " + jsonResponseMessage, "POST");
                        oResponseMessage = Request.CreateResponse(HttpStatusCode.OK, oResponse);
                    }
                }
                else
                {
                    oResponse.Mensaje = "No se especificaron parametros en la peticion";
                    oResponse.CodigoRespuesta = "12";
                    oResponseMessage = Request.CreateResponse(HttpStatusCode.BadRequest, oResponse);
                }

                return oResponseMessage;
            }
            catch (Exception ex)
            {
                GrabaLog("Ocurrio un error: " + ex.Message, "AltaUsuario");
                GrabaLog("Stack Trace: " + ex.StackTrace, "AltaUsuario");
                oResponse.Mensaje = "No se especificaron parametros en la peticion";
                oResponse.CodigoRespuesta = "06";
                oResponseMessage = Request.CreateResponse(HttpStatusCode.InternalServerError, oResponse);
                return oResponseMessage;
            }

        }

    }
}
