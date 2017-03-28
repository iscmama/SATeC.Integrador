using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATeC.Integrador.Business.Entities
{
    public class FacturaDTO
    {
        public string tipo { get; set; }
        public string rfcEmisor { get; set; }
        public string rfcReceptor { get; set; }
        public DateTime fechaDocto { get; set; }
        public string fechaTimbrado { get; set; }
        public string noFactura { get; set; }
        public string UUID { get; set; }
        public decimal importeFactura { get; set; }
        public string moneda { get; set; }
        public string tipoCambio { get; set; }
    }
}