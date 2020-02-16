using NoorCRM.API.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoorCRM.Client.Sources
{
    public static class Helper
    {
        public static string CreateCustomerTitle(Customer customer)
        {
            if (customer == null)
                return "";

            if (!string.IsNullOrWhiteSpace(customer.StoreName))
                return $"{customer.StoreName.Trim()} ({customer.ManagerName.Trim()})";

            return customer.ManagerName.Trim();
        }
    }
}
