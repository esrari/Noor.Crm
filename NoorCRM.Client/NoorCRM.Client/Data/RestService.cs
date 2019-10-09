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

        public async Task<Course> GetCourseAsync(int courseId)
        {
            var uri = new Uri(string.Format(SoftwareSettings.CoursesUrl, courseId));
            try
            {
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Course>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return null;
        }

        public async Task<UserCourse> GetUserCourseByCourseAsync(int userId, int courseId)
        {
            var uri = new Uri(string.Format(SoftwareSettings.UserCoursesUrl, userId, courseId));
            try
            {
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<UserCourse>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return null;
        }

        public async Task<ICollection<Course>> GetFreeCoursesAsync()
        {
            var uri = new Uri(string.Format(SoftwareSettings.CoursesUrl, "Free"));
            try
            {
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ICollection<Course>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return null;
        }

        public async Task<ICollection<Course>> GetNewCoursesAsync()
        {
            var uri = new Uri(string.Format(SoftwareSettings.CoursesUrl, "New"));
            try
            {
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ICollection<Course>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return null;
        }

        public async Task<ICollection<Category>> GetRootCategoreisAsync()
        {
            var uri = new Uri(string.Format(SoftwareSettings.CategoreisUrl, string.Empty));
            try
            {
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ICollection<Category>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return null;
        }

        public async Task<Category> GetCategoryAsync(int catId)
        {
            var uri = new Uri(string.Format(SoftwareSettings.CategoreisUrl, catId));
            try
            {
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Category>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return null;
        }
    }
}
