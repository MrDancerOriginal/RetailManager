﻿using System.Collections.Generic;
using System.Linq;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
  public class ProductData : IProductData
  {
    private readonly ISqlDataAccess _sql;

    public ProductData(ISqlDataAccess sql)
    {
      _sql = sql;
    }
    public List<ProductModel> GetProducts()
    {
      var output = _sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "RMData");

      return output;
    }
    public ProductModel GetProductById(int productId)
    {
      var output = _sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetById", new {
        Id = productId
      }, "RMData")
          .FirstOrDefault();

      return output;
    }

  }
}
