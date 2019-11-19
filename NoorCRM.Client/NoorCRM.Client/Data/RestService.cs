using Newtonsoft.Json;
using NoorCRM.API.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NoorCRM.Client.Data
{
    public class RestService
    {
        private HttpClient _client;

        public RestService()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            { return true; };//remove if this makes it to production 

            _client = new HttpClient(handler);
        }

        public async Task<User> GetUserAsync(string phoneNo)
        {
            var uri = new Uri(string.Format(SoftwareSettings.UsersUrl, phoneNo));
            try
            {
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<User>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return null;
        }


        public async Task<ICollection<Customer>> GetUserCustomersAsync(int userId)
        {
            var uri = new Uri(string.Format(SoftwareSettings.CustomersUrl, "Visitor/" + userId));
            try
            {
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ICollection<Customer>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return null;
        }

        public async Task<ICollection<CustomerLog>> GetCustomerLogsAync(int customerId)
        {
            var uri = new Uri(string.Format(SoftwareSettings.CustomerLogsUrl, "Customer/" + customerId));
            try
            {
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ICollection<CustomerLog>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return null;
        }

        public async Task<Customer> SaveCustomerAsync(Customer customer, bool isNewCustomer = false)
        {
            var uri = new Uri(string.Format(SoftwareSettings.CustomersUrl, string.Empty));

            try
            {
                var json = JsonConvert.SerializeObject(customer);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                if (isNewCustomer)
                {
                    response = await _client.PostAsync(uri, content);
                }
                else
                {
                    response = await _client.PutAsync(uri, content);
                }

                // In return get the inserted or updated object
                if (response.IsSuccessStatusCode)
                {
                    var newContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Customer>(newContent);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return null;
        }

        //public async Task DeleteTodoItemAsync(string id)
        //{
        //    var uri = new Uri(string.Format(Constants.TodoItemsUrl, id));

        //    try
        //    {
        //        var response = await _client.DeleteAsync(uri);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            Debug.WriteLine(@"\tTodoItem successfully deleted.");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(@"\tERROR {0}", ex.Message);
        //    }
        //}
    }
}
