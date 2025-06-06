﻿using System;

namespace TRMDesktopUI.Library.Model
{
  public class LoggedInUserModel : ILoggedInUserModel
  {
    public string Token
    {
      get; set;
    }
    public string Id
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
    public DateTime CreatedDate
    {
      get; set;
    }

    public void ResetUserModel()
    {
      Token = string.Empty;
      Id = string.Empty;
      FirstName = string.Empty;
      LastName = string.Empty;
      EmailAdress = string.Empty;
      CreatedDate = DateTime.MinValue;
    }
  }
}
