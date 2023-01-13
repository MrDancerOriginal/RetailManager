using System;
using System.Configuration;

namespace TRMDesktopUI.Library.Helpers
{
    // TODO : Move this from congif to API
    public class ConfigHelper : IConfigHelper
    {
        public decimal GetTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["taxRate"];
            bool isValidTaxRate = decimal.TryParse(rateText, out decimal output);
            if (!isValidTaxRate)
                throw new ConfigurationErrorsException("The tax rate is not set up properly");
            return output;
        }
    }
}
