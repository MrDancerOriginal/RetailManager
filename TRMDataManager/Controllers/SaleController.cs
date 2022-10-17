<<<<<<< HEAD
﻿using Microsoft.AspNet.Identity;
using System;
=======
﻿using System;
>>>>>>> d5db4101f283a302130a58c2c3a978a076fca60d
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Controllers
{
    [Authorize]
    public class SaleController : ApiController
    {
        public void Post(SaleModel sale)
        {
<<<<<<< HEAD
            SaleData data = new SaleData();
            string userId = RequestContext.Principal.Identity.GetUserId();

            data.SaveSale(sale, userId);
=======
            
>>>>>>> d5db4101f283a302130a58c2c3a978a076fca60d
        }

        //public List<ProductModel> Get()
        //{
        //    ProductData data = new ProductData();
        //    return data.GetProducts();
        //}
    }
}
