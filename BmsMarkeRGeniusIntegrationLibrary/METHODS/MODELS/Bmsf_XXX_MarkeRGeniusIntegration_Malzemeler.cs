using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsMarkeRGeniusIntegrationLibrary.METHODS.MODELS
{
    public class Bmsf_XXX_MarkeRGeniusIntegration_Malzemeler
    {
        public DateTime TARIH { get; set; }
		public int IBMGENIUSAMBAR { get; set; }
		public string CODE { get; set; }
		public string BARCODE { get; set; }
		public string EXPLANATION { get; set; }
		public string UNIT1 { get; set; }
		public string UNIT1IBM { get; set; }
		public double SELLING_PRICE1 { get; set; }
		public double VAT_RATE { get; set; }
		public int VAT_CODE { get; set; }

        public int ALCOHOL { get; set; }
        public int CYPHCODE { get; set; }
        public int ACTIVE { get; set; }
        public int VAT_CODE_N { get; set; }
        public string MARKCODE { get; set; }
		public string SPECODE { get; set; }
		public string SPECODE2 { get; set; }
		public string SPECODE3 { get; set; }
		public string SPECODE4 { get; set; }
		public string SPECODE5 { get; set; }
    }
}
