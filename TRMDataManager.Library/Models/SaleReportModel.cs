﻿using System;

namespace TRMDataManager.Library.Models
{
  public class SaleReportModel
  {
    public DateTime SaleDate
    {
      get; set;
    }
    public decimal SubTotal
    {
      get; set;
    }
    public decimal Tax
    {
      get; set;
    }
    public decimal Total
    {
      get; set;
    }
    public string FirstName
    {
      get; set;
    }
    public string LastName
    {
      get; set;
    }
    public string EmailAdress
    {
      get; set;
    }
  }
}
