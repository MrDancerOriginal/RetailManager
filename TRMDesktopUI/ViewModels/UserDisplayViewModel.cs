﻿using Caliburn.Micro;
using System;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Model;

namespace TRMDesktopUI.ViewModels
{
  public class UserDisplayViewModel : Screen
  {
    private readonly IUserEndPoint _userEndPoint;
    private readonly StatusInfoViewModel _status;
    private readonly IWindowManager _window;
    public UserDisplayViewModel(IUserEndPoint userEndPoint,
        StatusInfoViewModel status, IWindowManager window)
    {
      _userEndPoint = userEndPoint;
      _status = status;
      _window = window;
    }

    private BindingList<UserModel> _users;
    public BindingList<UserModel> Users
    {
      get
      {
        return _users;
      }
      set
      {
        _users = value;
        NotifyOfPropertyChange(() => Users);
      }
    }
    private UserModel _selectedUser;

    public UserModel SelectedUser
    {
      get
      {
        return _selectedUser;
      }
      set
      {
        _selectedUser = value;
        SelectedUserName = value.Email;
        UserRoles = new BindingList<string>(value.Roles.Select(x => x.Value).ToList());
        //TODO - Pull this out into a method/event
        LoadRoles();
        NotifyOfPropertyChange(() => SelectedUser);
      }
    }
    private string _selectedUserRole;

    public string SelectedUserRole
    {
      get
      {
        return _selectedUserRole;
      }
      set
      {
        _selectedUserRole = value;
        NotifyOfPropertyChange(() => SelectedUserRole);
        NotifyOfPropertyChange(() => CanRemoveSelectedRole);
      }
    }

    private string _selectedAvailableRole;

    public string SelectedAvailableRole
    {
      get
      {
        return _selectedAvailableRole;
      }
      set
      {
        _selectedAvailableRole = value;
        NotifyOfPropertyChange(() => SelectedAvailableRole);
        NotifyOfPropertyChange(() => CanAddSelectedRole);
      }
    }

    private string _selectedUserName;

    public string SelectedUserName
    {
      get
      {
        return _selectedUserName;
      }
      set
      {
        _selectedUserName = value;
        NotifyOfPropertyChange(() => SelectedUserName);
      }
    }
    private BindingList<string> _userRoles = new();

    public BindingList<string> UserRoles
    {
      get
      {
        return _userRoles;
      }
      set
      {
        _userRoles = value;
        NotifyOfPropertyChange(() => UserRoles);
      }
    }

    private BindingList<string> _availableRoles = new();
    public BindingList<string> AvailableRoles
    {
      get
      {
        return _availableRoles;
      }
      set
      {
        _availableRoles = value;
        NotifyOfPropertyChange(() => AvailableRoles);
      }
    }
    protected override async void OnViewLoaded(object view)
    {
      base.OnViewLoaded(view);
      try
      {
        await LoadUsers();
      }
      catch (Exception ex)
      {
        dynamic settings = new ExpandoObject();
        settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        settings.ResizeMode = ResizeMode.NoResize;
        settings.Title = "System Error";

        _ = IoC.Get<StatusInfoViewModel>();

        if (ex.Message == "Unauthorized")
        {
          _status.UpdateMessage("Unauthorized Access",
          "You don't have a permission to interact with the Sales Form.");
          await _window.ShowDialogAsync(_status, null, settings);
        }
        else
        {
          _status.UpdateMessage("Fatal Exception",
          ex.Message);
          await _window.ShowDialogAsync(_status, null, settings);
        }

        await TryCloseAsync();
      }
    }
    private async Task LoadUsers()
    {
      var userList = await _userEndPoint.GetAll();
      Users = new BindingList<UserModel>(userList);
    }
    private async Task LoadRoles()
    {
      var roles = await _userEndPoint.GetAllRoles();

      AvailableRoles.Clear();

      foreach (var role in roles)
      {
        if (UserRoles.IndexOf(role.Value) < 0)
        {
          AvailableRoles.Add(role.Value);
        }
      }
    }
    public bool CanAddSelectedRole
    {
      get
      {
        return !(SelectedUser is null || SelectedAvailableRole is null);
      }
    }
    public async void AddSelectedRole()
    {
      await _userEndPoint.AddUserToRole(SelectedUser.Id, SelectedAvailableRole);
      UserRoles.Add(SelectedAvailableRole);
      AvailableRoles.Remove(SelectedAvailableRole);
    }
    public bool CanRemoveSelectedRole
    {
      get
      {
        return !(SelectedUser is null || SelectedUserRole is null);
      }
    }
    public async void RemoveSelectedRole()
    {
      await _userEndPoint.RemoveUserFromRole(SelectedUser.Id, SelectedUserRole);
      AvailableRoles.Add(SelectedUserRole);
      UserRoles.Remove(SelectedUserRole);
    }
  }
}
