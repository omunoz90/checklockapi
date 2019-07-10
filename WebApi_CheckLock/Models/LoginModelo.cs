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
    public class LoginModelo
    {
        #region Clases
        public partial class LoginRequest
        {
            public string Usuario { get; set; }
            public string Password { get; set; }
        }

        public partial class LoginResponse
        {
            public string CodigoRespuesta { get; set; }
            public string Mensaje { get; set; }
        }
        #endregion

        #region Metodos_API 
        public LoginResponse LoginCheckLock(LoginRequest oParam)
        {
            LoginRequest oRequest = new LoginRequest();
            LoginResponse oResponse = new LoginResponse();
            Dictionary<string, string> dRespuesta = new Dictionary<string, string>();
            string respCode = string.Empty;
            string mensaje = string.Empty;

            try
            {
                dRespuesta = LoginRequestValidate(oParam.Usuario, oParam.Password);
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


        #region Metodos_BaseDatos
        private Dictionary<string, string> LoginRequestValidate(string username, string password)
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
                SQLComando.CommandText = "Login_App";
                SQLComando.Parameters.AddWithValue("@Usuario", username);
                SQLComando.Parameters.AddWithValue("@Pass", password);
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
                    GrabaLog("Ocurrio un error al retornar el valor", "LoginRequestValidate");
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
                GrabaLog("Ocurrio un error: " + ex.Message, "LoginRequestValidate");
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