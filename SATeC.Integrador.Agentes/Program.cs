using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SATeC.Integrador.Business.CfdiV32;
using SATeC.Integrador.Business.CfdiV33;
using SATeC.Integrador.Business.Logic;
using System.IO;

namespace SATeC.Integrador.Agentes
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ClientesManagement.ProcesarFacturas();
                ProveedoresManagement.ProcesarFacturas();
                NominaManagement.ProcesarFacturas();
            }
            catch (Exception ex)
            {
                LogErrores.Write("Ocurrio un error al ejecutar la consola SATeC Integrador de Facturas", ex);
            }
        }
    }
}
