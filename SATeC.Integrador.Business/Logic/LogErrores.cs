using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATeC.Integrador.Business.Logic
{
    public class LogErrores
    {
        public static void Write(string message, Exception ex)
        {
            try
            {
                string appPath = AppDomain.CurrentDomain.BaseDirectory;

                using (StreamWriter writer = new StreamWriter(appPath + "\\LogErrores_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt", true))
                {
                    writer.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | " + message + " | " + "Error: [" + ex.ToString() + "]");
                    writer.WriteLine(Environment.NewLine);
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}