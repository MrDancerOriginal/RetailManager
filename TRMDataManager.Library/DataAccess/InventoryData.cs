﻿using System.Collections.Generic;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
  public class InventoryData : IInventoryData
  {
    private readonly ISqlDataAccess _sql;

    public InventoryData(ISqlDataAccess sql)
    {
      _sql = sql;
    }
    public List<InventoryModel> GetInventory()
    {
      var output = _sql.LoadData<InventoryModel, dynamic>(
          "dbo.spInventory_GetAll",
          new {
          },
          "RMData");
      return output;
    }
    public void SaveInventoryRecord(InventoryModel item)
    {
      _sql.SaveData("spInventory_Insert", item, "RMData");
    }
  }
}
