using SATeC.Integrador.Business.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace SATeC.Integrador.Business.Logic
{
    public class ClientesManagement
    {
        static string pathFacturasClientes = ConfigurationManager.AppSettings["pathFacturasClientes"].ToString();
        static string pathOutClientes = ConfigurationManager.AppSettings["pathOutClientes"].ToString();

        public static void ProcesarFacturas()
        {
            List<FacturaDTO> facturasCliente = new List<FacturaDTO>();
            List<string> facturasProcesadas = new List<string>();

            try
            {
                string[] facturas = Directory.GetFiles(pathFacturasClientes, "*.xml");

                if (facturas.Count() > 0)
                {
                    foreach (string file in facturas)
                    {
                        try
                        {
                            XmlSerializer serielizer = new XmlSerializer(typeof(CfdiV32.Comprobante));
                            XmlTextReader reader = new XmlTextReader(file);
                            CfdiV32.Comprobante comprobanteV32 = (CfdiV32.Comprobante)serielizer.Deserialize(reader);

                            FacturaDTO fac = CommonManagement.GenerarFactura(comprobanteV32, "C");

                            if (!string.IsNullOrEmpty(fac.noFactura))
                            {
                                facturasCliente.Add(fac);
                                facturasProcesadas.Add(file);
                            }
                            
                            reader.Dispose();
                        }
                        catch (Exception e)
                        {
                            LogErrores.Write(string.Format("El archivo {0} no soporta la version 3.2", file), e);

                            try
                            {
                                XmlSerializer serielizer = new XmlSerializer(typeof(CfdiV33.Comprobante));
                                XmlTextReader reader = new XmlTextReader(file);
                                CfdiV33.Comprobante comprobanteV33 = (CfdiV33.Comprobante)serielizer.Deserialize(reader);
                                facturasCliente.Add(CommonManagement.GenerarFactura(comprobanteV33, "C"));
                                facturasProcesadas.Add(file);
                                reader.Dispose();
                            }
                            catch (Exception eV33)
                            {
                                LogErrores.Write(string.Format("El archivo {0} no soporta la version 3.3", file), eV33);
                            }
                        }
                    }

                    CommonManagement.PrintArchivo(pathOutClientes, "InvoiceClientes", facturasCliente);
                    CommonManagement.MoveFacturasXML(pathFacturasClientes, facturasProcesadas);
                }
                else
                {
                    Bitacoras.Write("No se encontraron Facturas de Clientes que procesar");
                }
            }
            catch (Exception ex)
            {
                LogErrores.Write("Ocurrio un error al Procesar las facturas de clientes", ex);
            }
        }
    }
}