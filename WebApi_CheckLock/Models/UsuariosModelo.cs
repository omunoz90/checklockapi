using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using static WebApi_CheckLock.Utilidades;


namespace WebApi_CheckLock.Models
{
    public class UsuariosModelo
    {
        #region Clases
        public partial class AltaUsuarioRequest
        {
            public string CorreoElectronico { get; set; }
            public string Password { get; set; }
            public string Nombre { get; set; }
            public string SegundoNombre { get; set; }
            public string ApellidoPaterno { get; set; }
            public string ApellidoMaterno { get; set; }
            public int Edad { get; set; }
            public string Telefono { get; set; }
        }

        public partial class AltaUsuarioResponse
        {
            public string CodigoRespuesta { get; set; }
            public string Mensaje { get; set; }
        }
        #endregion

        #region Metodos_API
        public AltaUsuarioResponse AltaUsuario(AltaUsuarioRequest oParam)
        {
            AltaUsuarioRequest oRequest = new AltaUsuarioRequest();
            AltaUsuarioResponse oResponse = new AltaUsuarioResponse();
            Dictionary<string, string> dRespuesta = new Dictionary<string, string>();
            string respCode = string.Empty;
            string mensaje = string.Empty;

            try
            {
                dRespuesta = AltaUsuarioRegistro(oParam.CorreoElectronico, oParam.Password, oParam.Nombre, oParam.SegundoNombre
                                                , oParam.ApellidoPaterno, oParam.ApellidoMaterno, oParam.Edad, oParam.Telefono);
                if (dRespuesta.Count > 0)
                {
                    respCode = dRespuesta["RespCode"];
                    mensaje = dRespuesta["Mensaje"];
                    oResponse.CodigoRespuesta = respCode;
                    oResponse.Mensaje = mensaje;
                }
                return oResponse;
            }
            catch (Exception ex)
            {
                GrabaLog("Ocurrio un error:" + ex.InnerException.Message, "Err");
                oResponse.CodigoRespuesta = "06";
                oResponse.Mensaje = ex.InnerException.Message;
                return oResponse;
            }

        }
        #endregion

        #region Metodos_Base_Datos
        private Dictionary<string, string> AltaUsuarioRegistro(string correoElectronico, string password, string nombre
                                                            , string segundoNombre, string apellidoPaterno, string apellidoMaterno
                                                            , int edad, string telefono)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand SQLComando = new SqlCommand();
            string stringConn = ConfigurationManager.ConnectionStrings["AppDB"].ToString();
            string respCode = string.Empty;

            Dictionary<string, string> dRespuesta = new Dictionary<string, string>();

            try
            {
                conn.ConnectionString = stringConn;
                SQLComando.Connection = conn;
                SQLComando.Connection.Open();
                SQLComando.CommandType = CommandType.StoredProcedure;
                SQLComando.CommandText = "AltaUsuarioApp";
                SQLComando.Parameters.AddWithValue("@CorreoElectronico", correoElectronico);
                SQLComando.Parameters.AddWithValue("@Pass", password);
                SQLComando.Parameters.AddWithValue("@Nombre", nombre);
                SQLComando.Parameters.AddWithValue("@SegundoNombre", segundoNombre);
                SQLComando.Parameters.AddWithValue("@ApellidoPaterno", apellidoPaterno);
                SQLComando.Parameters.AddWithValue("@ApellidoMaterno", apellidoMaterno);
                SQLComando.Parameters.AddWithValue("@Edad", edad);
                SQLComando.Parameters.AddWithValue("@Telefono", telefono);
                SqlDataReader sqlReader = SQLComando.ExecuteReader();

                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        dRespuesta.Add("RespCode", sqlReader["RespCode"].ToString());
                        dRespuesta.Add("Mensaje", sqlReader["Mensaje"].ToString());
                    }
                }
                else
                {
                    GrabaLog("Ocurrio un error al retornar el valor", "AltaUsuarioRegistro");
                    dRespuesta.Add("RespCode", "14");
                    dRespuesta.Add("Mensaje", "Ocurrio un error al retornar el valor");

                }
                GrabaLog("RespCode: " + dRespuesta["RespCode"], "Respuesta");
                GrabaLog("Mensaje: " + dRespuesta["Mensaje"], "Respuesta");


                return dRespuesta;
            }
            catch (Exception ex)
            {
                respCode = "06";
                GrabaLog("Ocurrio un error: " + ex.Message, "AltaUsuarioRegistro");
                dRespuesta.Add("RespCode", "06");
                dRespuesta.Add("Mensaje", ex.Message);
                GrabaLog("RespCode: " + dRespuesta["RespCode"], "Respuesta");
                GrabaLog("Mensaje: " + dRespuesta["Mensaje"], "Respuesta");
                return dRespuesta;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

    }
}