﻿using Caliburn.Micro;
using System.Threading;
using System.Threading.Tasks;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Model;

namespace TRMDesktopUI.ViewModels
{
  public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
  {
    private readonly IEventAggregator _events;
    private readonly ILoggedInUserModel _user;
    private readonly IAPIHelper _apiHelper;

    public ShellViewModel(IEventAggregator events,
        ILoggedInUserModel user, IAPIHelper apiHelper)
    {
      _events = events;
      _user = user;
      _apiHelper = apiHelper;

      _events.SubscribeOnPublishedThread(this);

      ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
    }

    public bool IsLoggedIn
    {
      get
      {
        return !string.IsNullOrWhiteSpace(_user.Token);
      }
    }
    public bool IsLoggedOut
    {
      get
      {
        return !IsLoggedIn;
      }
    }

    public void ExitApplication()
    {
      TryCloseAsync();
    }
    public async Task UserManagement()
    {
      await ActivateItemAsync(IoC.Get<UserDisplayViewModel>(), new CancellationToken());
    }
    public async Task LogIn()
    {
      await ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
    }
    public async Task LogOut()
    {
      _user.ResetUserModel();
      _apiHelper.LogOffUser();
      await ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
      NotifyOfPropertyChange(() => IsLoggedIn);
      NotifyOfPropertyChange(() => IsLoggedOut);
    }
    public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
    {
      await ActivateItemAsync(IoC.Get<SalesViewModel>(), cancellationToken);
      NotifyOfPropertyChange(() => IsLoggedIn);
      NotifyOfPropertyChange(() => IsLoggedOut);
    }
  }
}
