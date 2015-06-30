using log4net;
using Magento.RestApi;
using Magento.RestApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SuperShopDataSyncConsole
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string UNIKATOY_SERVER = @"192.168.0.5\SIRIUS";
        private const string UNIKATOY_DATABASE = "WEB-PORTAL";

        private const string MAGENTO_SUPERSHOP_URL = "http://www.supershop.si/";

        static void Main(string[] args)
        {
            if (args.Count() != 6)
            {
                Console.WriteLine("Usage: SuperShopDataSyncConsole {magento_username} {magento_password} {magento_consumer_key} {magento_consumer_secret} {unikatoy_username} {unikatoy_password}");
                return;
            }
            log.Info("Synchronizing Magento product data.");
            string magentoUsername = args[0];
            string magentoPassword = args[1];
            string magentoConsumerKey = args[2];
            string magentoConsumerSecret = args[3];
            string unikatoyUsername = args[4];
            string unikatoyPassword = args[5];
            IMagentoApi client = new MagentoApi()
                .Initialize(MAGENTO_SUPERSHOP_URL, magentoConsumerKey, magentoConsumerSecret)
                .AuthenticateAdmin(magentoUsername, magentoPassword);
            string connectionString = string.Format(
                "data source={0};Persist Security Info=false;database={1};user id={2};password={3};Connection Timeout = 0", 
                UNIKATOY_SERVER, UNIKATOY_DATABASE, unikatoyUsername, unikatoyPassword);
            UnikatoyProductDataReader productDataReader = new UnikatoyProductDataReader(connectionString);
            StockDataReader stockDataReader = new StockDataReader(connectionString);
            MagentoProductUpdater productUpdater = new MagentoProductUpdater(client);
            UnikatoyProduct product = null;
            try
            {
                productDataReader.open();
                stockDataReader.open();
                while ((product = productDataReader.read()) != null)
                {
                    Console.WriteLine("Transfering product: " + product.Sku);
                    log.Debug("Product: " + product);
                    product.StockData = stockDataReader.getStockForProduct(product.PantheonId);
                    productUpdater.update(product);
                }
            }
            catch (Exception e)
            {
                log.Error("UnikaToy database to SuperShop data sync failed: product=" + product.ToString(), e);
            }
            finally
            {
                productDataReader.close();
                stockDataReader.close();
            }
        }
    }
}
