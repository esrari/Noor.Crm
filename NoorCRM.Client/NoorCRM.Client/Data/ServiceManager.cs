using NoorCRM.API.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NoorCRM.Client.Data
{
    public class ServiceManager
    {
        private RestService _restService;
        private User _onlineUser;

        public event UserFetchedEventHandler OnlineUserFetched;
        public event CustomerLogsFetchedEventHandler CustomerLogsFetched;
        //public event CategoryFetchedEventHandler CategoryFetched;
        //public event UserCourseFetchedEventHandler UserCourseFetched;
        public string ExtractedUserPhoneNo { get; set; }
        //public IList<Course> OnlineUserCourses
        //{
        //    get
        //    {
        //        // Create a list of user courses
        //        var courseList = new List<Course>();
        //        if (_onlineUser != null && _onlineUser.UserCourses != null)
        //        {
        //            foreach (var item in _onlineUser.UserCourses)
        //            {
        //                if (item.Course != null)
        //                    courseList.Add(item.Course);
        //            }
        //        }

        //        return courseList;
        //    }
        //}

        public ServiceManager(RestService service)
        {
            _restService = service;
        }

        #region Get
        public async Task<User> GetOnlineUserAsync()
        {
            if (string.IsNullOrWhiteSpace(ExtractedUserPhoneNo))
                return null;

            // If online user not loaded yet, should get from API
            if (_onlineUser == null)
            {
                _onlineUser = await _restService.GetUserAsync(ExtractedUserPhoneNo);
                OnOnlineUserFetched();
            }

            // Even existed or not return _onlineUser
            return _onlineUser;
        }

        public async Task<ICollection<Customer>> GetUserCustomersAsync()
        {
            if (_onlineUser == null)
                return null;

            return await _restService.GetUserCustomersAsync(_onlineUser.Id);
        }

        public async Task<ICollection<CustomerLog>> GetCustomerLogsAync(int customerId)
        {
            var logs = await _restService.GetCustomerLogsAync(customerId);
            OnCustomerLogsFetched(logs);
            return logs;
        }
        #endregion

        #region Insert & Update
        public async Task<Customer> InsertNewCustomerAsync(Customer customer)
        {
            return await _restService.SaveCustomerAsync(customer, true);
        }

        public async Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            return await _restService.SaveCustomerAsync(customer, false);
        } 
        #endregion

        #region Event Methods
        public void OnOnlineUserFetched()
        {
            if (_onlineUser != null)
                OnlineUserFetched?.Invoke(_onlineUser);
        }
        public void OnCustomerLogsFetched(IEnumerable<CustomerLog> logs)
        {
            CustomerLogsFetched?.Invoke(logs);
        } 
        #endregion
    }

    public delegate void UserFetchedEventHandler(User user);
    public delegate void CustomerLogsFetchedEventHandler(IEnumerable<CustomerLog> logs);
}
