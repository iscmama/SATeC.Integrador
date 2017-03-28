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
    public class ProveedoresManagement
    {
        static string pathFacturasProveedores = ConfigurationManager.AppSettings["pathFacturasProveedores"].ToString();
        static string pathOutProveedores = ConfigurationManager.AppSettings["pathOutProveedores"].ToString();

        public static void ProcesarFacturas()
        {
            List<FacturaDTO> facturasProveedor = new List<FacturaDTO>();
            List<string> facturasProcesadas = new List<string>();

            try
            {
                string[] facturas = Directory.GetFiles(pathFacturasProveedores, "*.xml");

                if (facturas.Count() > 0)
                {
                    foreach (string file in facturas)
                    {
                        try
                        {
                            XmlSerializer serielizer = new XmlSerializer(typeof(CfdiV32.Comprobante));
                            XmlTextReader reader = new XmlTextReader(file);
                            CfdiV32.Comprobante comprobanteV32 = (CfdiV32.Comprobante)serielizer.Deserialize(reader);

                            FacturaDTO fac = CommonManagement.GenerarFactura(comprobanteV32, "P");

                            if (!string.IsNullOrEmpty(fac.noFactura))
                            {
                                facturasProveedor.Add(fac);
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
                                facturasProveedor.Add(CommonManagement.GenerarFactura(comprobanteV33, "P"));
                                facturasProcesadas.Add(file);
                                reader.Dispose();
                            }
                            catch (Exception eV33)
                            {
                                LogErrores.Write(string.Format("El archivo {0} no soporta la version 3.3", file), eV33);
                            }
                        }
                    }

                    CommonManagement.PrintArchivo(pathOutProveedores, "InvoiceProveedores", facturasProveedor);
                    CommonManagement.MoveFacturasXML(pathFacturasProveedores, facturasProcesadas);
                }
                else
                {
                    Bitacoras.Write("No se encontraron Facturas de Proveedores que procesar");
                }
            }
            catch (Exception ex)
            {
                LogErrores.Write("Ocurrio un error al Procesar las facturas de Proveedores", ex);
            }
        }
    }
}