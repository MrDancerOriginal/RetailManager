﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Portal.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Portal.Authentication
{
  public class AuthenticationService : IAuthenticationService
  {
    private readonly HttpClient _client;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;
    private readonly IConfiguration _config;
    private readonly string authTokenStorageKey;

    public AuthenticationService(HttpClient client,
        AuthenticationStateProvider authStateProvider,
        ILocalStorageService localStorage,
        IConfiguration config)
    {
      _client = client;
      _authStateProvider = authStateProvider;
      _localStorage = localStorage;
      _config = config;

      authTokenStorageKey = _config["authTokenStorageKey"];
    }

    public async Task<AuthenticatedUserModel> Login(AuthenticationUserModel userForAuthentication)
    {
      var data = new FormUrlEncodedContent(new[]
      {
                new KeyValuePair<string,string>("grant_type","password"),
                new KeyValuePair<string, string>("username", userForAuthentication.Email),
                new KeyValuePair<string, string>("password", userForAuthentication.Password)
            });
      string api = _config["api"] + _config["tokenEndpoint"];
      var authResult = await _client.PostAsync(api, data);
      string authContent = await authResult.Content.ReadAsStringAsync();

      if (authResult.IsSuccessStatusCode == false)
      {
        return null;
      }

      var result = JsonSerializer.Deserialize<AuthenticatedUserModel>(
          authContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

      await _localStorage.SetItemAsync(authTokenStorageKey, result.Access_Token);

      await ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Access_Token);

      _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Access_Token);
      return result;
    }

    public async Task Logout()
    {
      await ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
    }
  }
}
