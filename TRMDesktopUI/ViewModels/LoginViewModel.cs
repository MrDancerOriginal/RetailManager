﻿using Caliburn.Micro;
using System;
using System.Threading;
using System.Threading.Tasks;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Library.Api;

namespace TRMDesktopUI.ViewModels
{
  public class LoginViewModel : Screen
  {
    private string _userName = "mrdanceryoutub@gmail.com";
    private string _password = "Mrdancer1.";
    private readonly IAPIHelper _apiHelper;
    private readonly IEventAggregator _events;

    public LoginViewModel(IAPIHelper apiHelper, IEventAggregator events)
    {
      _apiHelper = apiHelper;
      _events = events;
    }

    public string UserName
    {
      get => _userName; set
      {
        _userName = value;
        NotifyOfPropertyChange(() => UserName);
        NotifyOfPropertyChange(() => CanLogIn);
      }
    }
    public string Password
    {
      get => _password; set
      {
        _password = value;
        NotifyOfPropertyChange(() => Password);
        NotifyOfPropertyChange(() => CanLogIn);
      }
    }


    public bool IsErrorVisible
    {
      get
      {
        return ErrorMessage?.Length > 0;
      }
    }

    private string _errorMessage;
    public string ErrorMessage
    {
      get
      {
        return _errorMessage;
      }
      set
      {
        _errorMessage = value;
        NotifyOfPropertyChange(() => ErrorMessage);
        NotifyOfPropertyChange(() => IsErrorVisible);
      }
    }
    public bool CanLogIn
    {
      get
      {
        return UserName?.Length > 0 && Password?.Length > 0;
      }
    }
    public async Task LogIn()
    {
      try
      {
        ErrorMessage = "";
        var result = await _apiHelper.Authenticate(UserName, Password);

        //Capture more information about the user
        await _apiHelper.GetLogInUserInfo(result.Access_Token);

        await _events.PublishOnUIThreadAsync(new LogOnEvent(), new CancellationToken());
      }
      catch (Exception ex)
      {
        ErrorMessage = ex.Message;
      }
    }
  }
}
