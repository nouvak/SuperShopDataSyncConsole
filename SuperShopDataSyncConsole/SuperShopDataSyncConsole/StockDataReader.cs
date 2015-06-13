using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SuperShopDataSyncConsole
{
    class StockDataReader
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private SqlConnection connection = null;
        private SqlCommand command = null;

        public StockDataReader(string connectionString)
        {
            connection = new SqlConnection(connectionString);
        }

        public void open()
        {
            connection.Open();
            command = new SqlCommand();
            command.Connection = connection;
        }

        public void close()
        {
            if (command != null)
            {
                command.Dispose();
            }
            if (connection != null)
            {
                connection.Close();
            }
        }

        public Stock getStockForProduct(string productId)
        {
            log.Info("Retrieving stock data for product: " + productId);
            command.CommandText = string.Format("SELECT * FROM tHE_Stock WHERE acWarehouse='Skl.Veleprodaja - Postojna    ' AND acIdent='{0}'", productId);
            SqlDataReader dataReader = command.ExecuteReader();
            Stock stock = null;
            if (dataReader.Read())
            {
                stock = Stock.fromDbData(dataReader);
            }
            else
            {
                stock = new Stock();
                stock.Quantity = 0.0;
            }
            dataReader.Close();
            return stock;
        }
    }
}
