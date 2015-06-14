using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShopDataSyncConsole
{
    class UnikatoyProduct
    {
        public string PantheonId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double ManufacturerSuggestedRetailPrice { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public double Weight { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public Stock StockData { get; set; }
    }
}
