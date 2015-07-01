using log4net;
using log4net.Config;
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

        private const string LOG4NET_CONFIG_FILE = "log4net.config";

        private const string UNIKATOY_SERVER = @"192.168.0.5\SIRIUS";
        private const string UNIKATOY_DATABASE = "WEB-PORTAL";

        private const string MAGENTO_SUPERSHOP_URL = "http://www.supershop.si/";

        static void Main(string[] args)
        {
            if (args.Count() != 7)
            {
                Console.WriteLine("Usage: SuperShopDataSyncConsole {magento_username} {magento_password} {magento_consumer_key} {magento_consumer_secret} {unikatoy_username} {unikatoy_password} {image_directory}");
                return;
            }
            XmlConfigurator.Configure(new System.IO.FileInfo(LOG4NET_CONFIG_FILE));
            log.Info("Synchronizing Magento product data.");
            string magentoUsername = args[0];
            string magentoPassword = args[1];
            string magentoConsumerKey = args[2];
            string magentoConsumerSecret = args[3];
            string unikatoyUsername = args[4];
            string unikatoyPassword = args[5];
            string imageDirectory = args[6];
            IMagentoApi client = new MagentoApi()
                .Initialize(MAGENTO_SUPERSHOP_URL, magentoConsumerKey, magentoConsumerSecret)
                .AuthenticateAdmin(magentoUsername, magentoPassword);
            string connectionString = string.Format(
                "data source={0};Persist Security Info=false;database={1};user id={2};password={3};Connection Timeout = 0", 
                UNIKATOY_SERVER, UNIKATOY_DATABASE, unikatoyUsername, unikatoyPassword);
            UnikatoyProductDataReader productDataReader = new UnikatoyProductDataReader(connectionString);
            StockDataReader stockDataReader = new StockDataReader(connectionString);
            MagentoProductUpdater productUpdater = new MagentoProductUpdater(client, imageDirectory);
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
