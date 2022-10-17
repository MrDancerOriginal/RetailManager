using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
    public class SaleData
    {
<<<<<<< HEAD
        public void SaveSale(SaleModel saleInfo, string cashierId)
=======

        //public List<ProductModel> GetProducts()
        //{
        //    SqlDataAccess sql = new SqlDataAccess();

        //    var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "RMData");

        //    return output;
        //}
        public void SaveSale(SaleModel sale)
>>>>>>> d5db4101f283a302130a58c2c3a978a076fca60d
        {
            //TODO : Make this SOLID/DRY/Better
            // Start filling in the detail models we will save to the database
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
<<<<<<< HEAD
            ProductData products = new ProductData();
            var taxRate = ConfigHelper.GetTaxRate()/100;

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId, 
=======

            foreach (var item in sale.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
>>>>>>> d5db4101f283a302130a58c2c3a978a076fca60d
                    Quantity = item.Quantity,
                };

                // Get the information about product
<<<<<<< HEAD
                var productInfo = products.GetProductById(detail.ProductId);

                if(productInfo == null)
                    throw new Exception($"The product If of {detail.ProductId} could not be find in database.");

                detail.PurchasePrice = (productInfo.RetailPrice * detail.Quantity);

                if (productInfo.IsTaxable)
                    detail.Tax = (detail.PurchasePrice * taxRate);
=======
>>>>>>> d5db4101f283a302130a58c2c3a978a076fca60d

                details.Add(detail);
            }

<<<<<<< HEAD
            // Create the Sale model
            SaleDBModel sale = new SaleDBModel()
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId
            };

            sale.Total = sale.SubTotal + sale.Tax;
            // Save the sale model
            SqlDataAccess sql = new SqlDataAccess();
            sql.SaveData("dbo.spSale_Insert",sale,"RMData");

            // Get the ID from the sale model
            sale.Id = sql.LoadData<int, dynamic>("spSale_Lookup", new
            {
                sale.CashierId,
                sale.SaleDate
            }, "RMData").FirstOrDefault();

            // Finish filling in the sale detail models
            foreach (var item in details)
            {
                item.SaleId = sale.Id;
                // Save the sale detail models
                sql.SaveData("dbo.spSaleDetail_Insert", item, "RMData");
            }
        }
        //public List<ProductModel> GetProducts()
        //{
        //    SqlDataAccess sql = new SqlDataAccess();

        //    var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "RMData");

        //    return output;
        //}
=======
            // Fill in the available information
            // Create the Sale model
            // Save the sale model
            // Get the ID from the sale model
            // Finish filling in the sale detail models
            // Save the sale detail models
        }
>>>>>>> d5db4101f283a302130a58c2c3a978a076fca60d
    }
}
