using log4net;
using Magento.RestApi;
using Magento.RestApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SuperShopDataSyncConsole
{
    class MagentoProductUpdater
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private IMagentoApi magentoClient;

        public MagentoProductUpdater(IMagentoApi magentoClient)
        {
            this.magentoClient = magentoClient;
        }

        public void update(UnikatoyProduct product)
        {
            try
            {
                int categoryId = CategoryMappings.getCategoryId(product.Category, product.SubCategory);
                Product magentoProduct = magentoClient.GetProductBySku(product.Sku).Result.Result;
                if (magentoProduct != null)
                {
                    log.Debug("Updating existing product in magento: " + product);
                    updateExistingProduct(product, magentoProduct);
                    MagentoApiResponse<bool> response = magentoClient.UpdateProduct(magentoProduct).Result;
                    if (response.HasErrors)
                    {
                        log.Warn("Product updating failed: " + response.ErrorString);
                    }
                    else if (!response.Result)
                    {
                        log.Debug("Product updating failed.");
                    }
                    else
                    {
                        log.Debug("Product updating succeeded.");
                    }
                }
                else
                {
                    log.Debug("Adding new product to magento: " + product);
                    magentoProduct = createNewProduct(product);
                    MagentoApiResponse<int> response = magentoClient.CreateNewProduct(magentoProduct).Result;
                    if (response.HasErrors)
                    {
                        log.Warn(string.Format("New product adding failed: product={0}, error={1} ", product, response.ErrorString));
                    }
                    else
                    {
                        log.Debug("New product adding succeeded.");
                        magentoProduct.entity_id = response.Result;
                        setWebsiteToProduct(product, magentoProduct);
                        setCategoryToProduct(product, magentoProduct, categoryId);
                        addImagesToProduct(product, magentoProduct);
                    }
                }
            }
            catch (Exception e)
            {
                log.Error("Product updating failed: " + e);
            }
        }

        private Product createNewProduct(UnikatoyProduct product)
        {
            Product magentoProduct = new Product
            {
                name = product.Name,
                description = product.LongDescription,
                short_description = product.ShortDescription,
                price = product.Price,
                sku = product.Sku,
                visibility = ProductVisibility.CatalogSearch,
                status = ProductStatus.Enabled,
                weight = product.Weight,
                tax_class_id = 2,
                type_id = "simple",
                attribute_set_id = 4, // default
                msrp_enabled = ManufacturerPriceEnablement.UseConfig,
                msrp_display_actual_price_type = PriceTypeDisplay.UseConfig,
                msrp = product.ManufacturerSuggestedRetailPrice


                //country_of_manufacture = "BE",
                //custom_design = "default/default",
                //custom_design_from = DateTime.Now,
                //custom_design_to = DateTime.Now.AddMonths(1),
                //custom_layout_update = "<test></test>",
                //enable_googlecheckout = true,
                //gift_message_available = true,
                //meta_description = product.ShortDescription,
                //meta_keyword = "meta keyword",
                //meta_title = "meta title",
                //news_from_date = DateTime.Now,
                //news_to_date = DateTime.Now.AddDays(7),
                //options_container = "container2",
                //page_layout = "two_columns_left",
                //special_from_date = DateTime.Now,
                //special_to_date = DateTime.Now.AddDays(14),
                //special_price = 110.99,
                //url_key = "a-new-product"
            };
            magentoProduct.stock_data = new StockData
            {
                manage_stock = true,
                is_in_stock = true,
                qty = product.StockData.Quantity,
                backorders = BackOrderStatus.NoBackorders


                //enable_qty_increments = false,
                //is_decimal_divided = false,
                //is_qty_decimal = false,
                //max_sale_qty = 10,
                //min_qty = 2,
                //min_sale_qty = 1,
                //notify_stock_qty = 5,
                //qty_increments = 1,
                //use_config_backorders = false,
                //use_config_enable_qty_inc = false,
                //use_config_manage_stock = false,
                //use_config_max_sale_qty = false,
                //use_config_min_qty = false,
                //use_config_min_sale_qty = false,
                //use_config_notify_stock_qty = false,
                //use_config_qty_increments = false
            };
            return magentoProduct;
        }

        private void updateExistingProduct(UnikatoyProduct product, Product magentoProduct)
        {
            magentoProduct.name = product.Name;
            magentoProduct.description = product.LongDescription;
            magentoProduct.short_description = product.ShortDescription;
            magentoProduct.price = product.Price;
            magentoProduct.sku = product.Sku;
            magentoProduct.weight = product.Weight;
            if (magentoProduct.stock_data == null)
            {
                magentoProduct.stock_data = new StockData();
            }
            magentoProduct.stock_data.qty = product.StockData.Quantity;
        }

        private void setWebsiteToProduct(UnikatoyProduct product, Product magentoProduct)
        {
            const int SUPERSHOP_STORE_ID = 1;
            log.Debug("Setting website to product: " + product);
            MagentoApiResponse<bool> response = magentoClient.AssignWebsiteToProduct(magentoProduct.entity_id, SUPERSHOP_STORE_ID).Result;
            if (response.HasErrors)
            {
                log.Warn("Failed to assign website to product: " + product);
            }
        }

        private void setCategoryToProduct(UnikatoyProduct product, Product magentoProduct, int categoryId)
        {
            log.Debug("Setting category to product: " + product);
            MagentoApiResponse<bool> response = magentoClient.AssignCategoryToProduct(magentoProduct.entity_id, categoryId).Result;
            if (response.HasErrors)
            {
                log.Warn("Failed to assign category to product: " + product);
            }
        }

        private void addImagesToProduct(UnikatoyProduct product, Product magentoProduct)
        {
            ImageFile imageFile = new ImageFile
            {
                file_content = File.ReadAllBytes(@"C:\Users\marko\Downloads\150000.jpg"),
                file_mime_type = "image/jpeg",
                file_name = "150000"
            };
            MagentoApiResponse<int> responseAddImage = magentoClient.AddImageToProduct(magentoProduct.entity_id, imageFile).Result;
            if (responseAddImage.HasErrors)
            {
                log.Warn(string.Format("Failed to add image to product: product={0}, error={1}", product, responseAddImage.ErrorString));
                return;
            }
            int imageId = responseAddImage.Result;
            MagentoApiResponse<ImageInfo> responseImageInfo = magentoClient.GetImageInfoForProduct(magentoProduct.entity_id, imageId).Result;
            if (responseImageInfo.HasErrors)
            {
                log.Warn(string.Format("Failed to retrieve image info of product: product={0}, error={1}", product, responseImageInfo.ErrorString));
                return;
            }
            ImageInfo imageInfo = responseImageInfo.Result;
            imageInfo.types.Add(ImageType.image);
            imageInfo.types.Add(ImageType.small_image);
            imageInfo.types.Add(ImageType.thumbnail);
            MagentoApiResponse<bool> responseUpdateImageInfo = magentoClient.UpdateImageInfoForProduct(magentoProduct.entity_id, imageId, imageInfo).Result;
            if (responseUpdateImageInfo.HasErrors)
            {
                log.Warn(string.Format("Failed to update image info of product: product={0}, error={1}", product, responseUpdateImageInfo.ErrorString));
            }
        }
    }
}
