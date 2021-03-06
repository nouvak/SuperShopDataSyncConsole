﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SuperShopDataSyncConsole
{
    class UnikatoyProductDataReader
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private SqlConnection connection = null;
        private SqlCommand command = null;
        private SqlDataReader dataReader = null;

        public UnikatoyProductDataReader(string connectionString)
        {
            connection = new SqlConnection(connectionString);
        }

        public void open()
        {
            connection.Open();
            command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM tHE_SetItem";
            dataReader = command.ExecuteReader();
        }

        public void close()
        {
            if (dataReader != null)
            {
                dataReader.Close();
            }
            if (command != null)
            {
                command.Dispose();
            }
            if (connection != null) {
                connection.Close();
            }
        }

        public UnikatoyProduct read()
        {
            while (dataReader.Read())
            {
                if (isProductActive(dataReader))
                {
                    UnikatoyProduct product = fromDbData(dataReader);
                    log.Debug("Retrieved product from UnikaToy database: " + product);
                    return product;
                }
            }
            return null;
        }

        private static Boolean isProductActive(SqlDataReader dataReader)
        {
            string acActive = dataReader["acActive"].ToString();
            bool anShowInWebCatalogue = false;
            if (!Convert.IsDBNull(dataReader["_anShowInWebCatalogue"])) 
            {
                anShowInWebCatalogue = (bool)dataReader["_anShowInWebCatalogue"];
            }
            return acActive.ToUpper().Equals("T") && anShowInWebCatalogue;
        }

        private UnikatoyProduct fromDbData(SqlDataReader dataReader)
        {
            UnikatoyProduct product = new UnikatoyProduct();
            product.PantheonId = dataReader["acIdent"].ToString();
            product.Sku = dataReader["acCode"].ToString();
            product.Name = dataReader["acName"].ToString();
            product.Price = Convert.ToDouble(dataReader["anSalePrice"].ToString());
            product.ManufacturerSuggestedRetailPrice = Convert.ToDouble(dataReader["anRTPrice"].ToString());
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
