using SATeC.Integrador.Business.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SATeC.Integrador.Business.Logic
{
    public class CommonManagement
    {
        public static FacturaDTO GenerarFactura(CfdiV32.Comprobante comprobante, string tipo)
        {
            FacturaDTO factura = new FacturaDTO();

            try
            {
                factura = new FacturaDTO()
                {
                    tipo = tipo,
                    rfcEmisor = comprobante.Emisor.rfc,
                    rfcReceptor = comprobante.Receptor.rfc,
                    fechaDocto = comprobante.fecha,
                    fechaTimbrado = comprobante.Complemento.Any[0].Attributes["FechaTimbrado"].Value,
                    noFactura = comprobante.folio,
                    UUID = comprobante.Complemento.Any[0].Attributes["UUID"].Value,
                    importeFactura = comprobante.total,
                    moneda = comprobante.Moneda,    
                    tipoCambio = comprobante.TipoCambio
                };
            }
            catch (Exception ex)
            {
                LogErrores.Write("Ocurrio un error al Generar la factura con la V.3.2 con el folio: " + comprobante.folio, ex);
            }

            return factura;
        }
        public static FacturaDTO GenerarFactura(CfdiV33.Comprobante comprobante, string tipo)
        {
            FacturaDTO factura = new FacturaDTO();

            try
            {
                factura = new FacturaDTO()
                {
                    tipo = tipo,
                    rfcEmisor = comprobante.Emisor.Rfc,
                    rfcReceptor = comprobante.Receptor.Rfc,
                    fechaDocto = comprobante.Fecha,
                    fechaTimbrado = comprobante.Complemento.Any[0].Attributes[1].Value,
                    noFactura = comprobante.Folio,
                    UUID = comprobante.Complemento.Any[0].Attributes[2].Value,
                    importeFactura = comprobante.Total,
                    moneda = comprobante.Moneda.ToString(),
                    tipoCambio = comprobante.TipoCambio.ToString()
                };
            }
            catch (Exception ex)
            {
                LogErrores.Write("Ocurrio un error al Generar la factura con la V3.3 con el folio: " + comprobante.Folio, ex);
            }

            return factura;
        }
        public static void PrintArchivo(string path, string preFile, List<FacturaDTO> facturas)
        {
            StringBuilder sbFacturas = new StringBuilder();

            try
            {
                if (facturas.Count > 0)
                {
                    sbFacturas.AppendLine("Tipo, RFCEmisor, RFCReceptor, FechaDocto, FechaTimbrado, NoFactura, UUID, ImporteFactura, Moneda, TipodeCambio");

                    foreach (FacturaDTO fac in facturas)
                    {
                        sbFacturas.AppendLine(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", fac.tipo, fac.rfcEmisor, fac.rfcReceptor, fac.fechaDocto, fac.fechaTimbrado, fac.noFactura, fac.UUID, fac.importeFactura, fac.moneda, fac.tipoCambio));
                    }

                    using (StreamWriter writer = new StreamWriter(path + "\\" + preFile + "_" + DateTime.Now.ToString("dd-MM-yyyy-HHmmss") + ".csv", true))
                    {
                        writer.WriteLine(sbFacturas.ToString());
                        writer.Close();
                    }
                }
                else
                {
                    Bitacoras.Write("La lista de facturas viene vacía");
                }
            }
            catch (Exception ex)
            {
                LogErrores.Write("Ocurrio un error al Imprimir el archivo de facturas", ex);
            }
        }
        public static void MoveFacturasXML(string path, List<string> facturasXML)
        {
            try
            {
                if (facturasXML.Count > 0)
                {
                    foreach (string file in facturasXML)
                    {
                        File.Move(file, path + "\\Procesadas\\" + Path.GetFileName(file));
                    }
                }
                else
                {
                    Bitacoras.Write("La lista de archivos viene vacía");
                }
            }
            catch (Exception ex)
            {
                LogErrores.Write("Ocurrio un error al mover los archivos de facturas procesados", ex);
            }
        }
    }
}