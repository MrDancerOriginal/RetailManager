﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using TRMApi.Models;

namespace TRMApi.Controllers
{
  public class HomeController : Controller
  {
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;

    public HomeController(RoleManager<IdentityRole> roleManager,
        UserManager<IdentityUser> userManager)
    {
      _roleManager = roleManager;
      _userManager = userManager;
    }

    public IActionResult Index()
    {
      return View();
    }

    public async Task<IActionResult> Privacy()
    {
      string[] roles = { "Admin", "Manager", "Cashier" };
      foreach (var role in roles)
      {
        var roleExist = await _roleManager.RoleExistsAsync(role);
        if (!roleExist)
          await _roleManager.CreateAsync(new IdentityRole(role));
      }

      var user = await _userManager.FindByEmailAsync("mrdanceryoutub@gmail.com");

      if (user != null)
      {
        await _userManager.AddToRoleAsync(user, "Admin");
        await _userManager.AddToRoleAsync(user, "Cashier");
      }

      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
