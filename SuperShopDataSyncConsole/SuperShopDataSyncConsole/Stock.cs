using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SuperShopDataSyncConsole
{
    class Stock
    {
        public double Quantity { get; set; }

        public static Stock fromDbData(SqlDataReader dataReader)
        {
            Stock stock = new Stock();
            stock.Quantity = Convert.ToDouble(dataReader["anStock"]);
            return stock;
        }
    }
}
