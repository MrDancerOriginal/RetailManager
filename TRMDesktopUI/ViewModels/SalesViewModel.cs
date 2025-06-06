﻿using AutoMapper;
using Caliburn.Micro;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Model;
using TRMDesktopUI.Models;

namespace TRMDesktopUI.ViewModels
{
  public class SalesViewModel : Screen
  {
    private readonly IProductEndpoint _productEndPoint;
    private readonly IConfiguration _config;
    private readonly ISaleEndpoint _saleEndpoint;
    private readonly IMapper _mapper;
    private readonly StatusInfoViewModel _status;
    private readonly IWindowManager _window;

    public SalesViewModel(IProductEndpoint productEndpoint, IConfiguration config,
        ISaleEndpoint saleEndpoint, IMapper mapper, StatusInfoViewModel status, IWindowManager window)
    {
      _productEndPoint = productEndpoint;
      _config = config;
      _saleEndpoint = saleEndpoint;
      _mapper = mapper;
      _status = status;
      _window = window;
    }

    protected override async void OnViewLoaded(object view)
    {
      base.OnViewLoaded(view);
      try
      {
        await LoadProducts();
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
    private async Task LoadProducts()
    {
      var productList = await _productEndPoint.GetAll();
      var products = _mapper.Map<List<ProductDisplayModel>>(productList);
      Products = new BindingList<ProductDisplayModel>(products);
    }
    private async Task ResetSalesViewModel()
    {
      Cart = new BindingList<CartItemDisplayModel>();
      //TODO - Add clearing the selectedCartItem if it does not do it itself
      await LoadProducts();
      NotifyOfPropertyChange(() => SubTotal);
      NotifyOfPropertyChange(() => Tax);
      NotifyOfPropertyChange(() => Total);
      NotifyOfPropertyChange(() => CanCheckOut);
    }

    private BindingList<ProductDisplayModel> _products;

    public BindingList<ProductDisplayModel> Products
    {
      get
      {
        return _products;
      }
      set
      {
        _products = value;
        NotifyOfPropertyChange(() => Products);
      }
    }

    private ProductDisplayModel _selectedProduct;

    public ProductDisplayModel SelectedProduct
    {
      get
      {
        return _selectedProduct;
      }
      set
      {
        _selectedProduct = value;
        NotifyOfPropertyChange(() => SelectedProduct);
        NotifyOfPropertyChange(() => CanAddToCart);
      }
    }

    private CartItemDisplayModel _selectedCartItem;

    public CartItemDisplayModel SelectedCartItem
    {
      get
      {
        return _selectedCartItem;
      }
      set
      {
        _selectedCartItem = value;
        NotifyOfPropertyChange(() => SelectedCartItem);
        NotifyOfPropertyChange(() => CanRemoveFromCart);
      }
    }


    private BindingList<CartItemDisplayModel> _cart = new();

    public BindingList<CartItemDisplayModel> Cart
    {
      get
      {
        return _cart;
      }
      set
      {
        _cart = value;
        NotifyOfPropertyChange(() => Cart);
      }
    }


    private int _itemQuantity = 1;

    public int ItemQuantity
    {
      get
      {
        return _itemQuantity;
      }
      set
      {
        _itemQuantity = value;
        NotifyOfPropertyChange(() => ItemQuantity);
        NotifyOfPropertyChange(() => CanAddToCart);
      }
    }
    public string SubTotal
    {
      get
      {
        return CalculateSubTotal().ToString("C");
      }
    }
    private decimal CalculateSubTotal()
    {
      decimal subTotal = 0;
      foreach (var item in Cart)
        subTotal += item.Product.RetailPrice * item.QuantityInCart;
      return subTotal;
    }
    private decimal CalculateTax()
    {
      decimal taxAmount = 0;
      decimal taxRate = _config.GetValue<decimal>("taxRate") / 100;

      taxAmount = Cart
          .Where(x => x.Product.IsTaxable)
          .Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);
      return taxAmount;
    }

    public string Tax
    {
      get
      {
        return CalculateTax().ToString("C");
      }
    }

    public string Total
    {
      get
      {
        decimal total = CalculateSubTotal() + CalculateTax();
        return total.ToString("C");
      }
    }


    public bool CanAddToCart
    {
      get
      {
        return ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity;
      }
    }

    public void AddToCart()
    {
      CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
      if (existingItem != null)
      {
        existingItem.QuantityInCart += ItemQuantity;
      }
      else
      {
        CartItemDisplayModel item = new()
        {
          Product = SelectedProduct,
          QuantityInCart = ItemQuantity,
        };
        Cart.Add(item);
      }
      SelectedProduct.QuantityInStock -= ItemQuantity;
      ItemQuantity = 1;
      NotifyOfPropertyChange(() => SubTotal);
      NotifyOfPropertyChange(() => Tax);
      NotifyOfPropertyChange(() => Total);
      NotifyOfPropertyChange(() => CanCheckOut);
    }

    public bool CanRemoveFromCart
    {
      get
      {
        return SelectedCartItem != null && SelectedCartItem?.QuantityInCart > 0;
      }
    }
    public void RemoveFromCart()
    {
      SelectedCartItem.Product.QuantityInStock++;

      if (SelectedCartItem.QuantityInCart > 1)
      {
        SelectedCartItem.QuantityInCart--;
      }
      else
      {
        Cart.Remove(SelectedCartItem);
      }

      NotifyOfPropertyChange(() => SubTotal);
      NotifyOfPropertyChange(() => Tax);
      NotifyOfPropertyChange(() => Total);
      NotifyOfPropertyChange(() => CanCheckOut);
      NotifyOfPropertyChange(() => CanAddToCart);
    }

    public bool CanCheckOut
    {
      get
      {
        return Cart.Count > 0;
      }
    }

    public async Task CheckOut()
    {
      //Create a SaleModel and post to the API
      SaleModel sale = new();
      foreach (var item in Cart)
      {
        sale.SaleDetails.Add(new SaleDetailModel
        {
          ProductId = item.Product.Id,
          Quantity = item.QuantityInCart
        });
      }
      await _saleEndpoint.PostSale(sale);
      await ResetSalesViewModel();
    }
  }
}
