using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApi_CheckLock
{
    public class Utilidades
    {
        public static void GrabaLog(string text, string flag)
        {
            string rutaLog = ConfigurationManager.AppSettings["RutaLog"].ToString();
            string nombreArchivo = string.Empty;
            string nombreArchivoCompleto = string.Empty;
            StreamWriter stream;
            DateTime fecha = DateTime.Now;
            StringBuilder messageLog = new StringBuilder();

            try
            {
                nombreArchivo = "CheckLock_API_" + fecha.Year.ToString()
                                          + fecha.Month.ToString().PadLeft(2, '0')
                                          + fecha.Day.ToString().PadLeft(2, '0')
                                          + ".txt";

                nombreArchivoCompleto = rutaLog + @"\" + nombreArchivo;

                messageLog.Append(DateTime.Now.ToString());
                messageLog.Append("\t");
                messageLog.Append(flag);
                messageLog.Append("\t");
                messageLog.Append(text);

                if (!File.Exists(nombreArchivoCompleto))
                {
                    stream = File.CreateText(nombreArchivoCompleto);
                    stream.WriteLine(messageLog);
                    stream.Flush();
                    stream.Close();
                }
                else
                {
                    stream = File.AppendText(nombreArchivoCompleto);
                    stream.WriteLine(messageLog);
                    stream.Flush();
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("AppMessage: Error en la aplicación, favor de verificarlo. " + ex.Message);
                GrabaLog("AppError: Error en la aplicación, favor de verificarlo. " + ex.Message.ToString(), "Err Log");
            }

        }
    }
}