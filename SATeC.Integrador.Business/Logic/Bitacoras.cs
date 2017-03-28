using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATeC.Integrador.Business.Logic
{
    public class Bitacoras
    {
        public static void Write(string message)
        {
            try
            {
                string appPath = AppDomain.CurrentDomain.BaseDirectory;

                using (StreamWriter writer = new StreamWriter(appPath + "\\Bitacora_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt", true))
                {
                    writer.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | " + message + " ]");
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