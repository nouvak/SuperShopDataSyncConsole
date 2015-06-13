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
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public double Weight { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public Stock StockData { get; set; }

        public static UnikatoyProduct fromDbData(SqlDataReader dataReader)
        {
            UnikatoyProduct product = new UnikatoyProduct();
            product.PantheonId = dataReader["acIdent"].ToString();
            product.Sku = dataReader["acCode"].ToString();
            product.Name = dataReader["acName"].ToString();
            product.Price = Convert.ToDouble(dataReader["anSalePrice"].ToString());
            product.ShortDescription = dataReader["_acDescriptionShort"].ToString();
            product.LongDescription = dataReader["_acDescription"].ToString();
            product.Weight = Convert.ToDouble(dataReader["anDimWeightBrutto"]);
            product.Category = dataReader["acClassif"].ToString();
            if (!Convert.IsDBNull(dataReader["acClassif2"]))
            {
                product.SubCategory = dataReader["acClassif2"].ToString();
            }
            return product;
        }
    }
}
