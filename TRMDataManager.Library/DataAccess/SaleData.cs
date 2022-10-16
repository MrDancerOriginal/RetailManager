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

        //public List<ProductModel> GetProducts()
        //{
        //    SqlDataAccess sql = new SqlDataAccess();

        //    var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "RMData");

        //    return output;
        //}
        public void SaveSale(SaleModel sale)
        {
            //TODO : Make this SOLID/DRY/Better
            // Start filling in the detail models we will save to the database
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();

            foreach (var item in sale.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                };

                // Get the information about product

                details.Add(detail);
            }

            // Fill in the available information
            // Create the Sale model
            // Save the sale model
            // Get the ID from the sale model
            // Finish filling in the sale detail models
            // Save the sale detail models
        }
    }
}
