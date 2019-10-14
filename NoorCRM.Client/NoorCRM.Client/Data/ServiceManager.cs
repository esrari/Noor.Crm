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
        //public event CourseFetchedEventHandler CourseFetched;
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

        //public async Task<Course> GetCompleteCourseAsync(int courseId)
        //{
        //    var course = await _restService.GetCourseAsync(courseId);
        //    OnCourseFetched(course);

        //    return course;
        //}

        //public async Task<UserCourse> GetCourseEquivalentUserCourseAsync(int courseId)
        //{
        //    if (_onlineUser == null)
        //        return null;

        //    var userCourse = await _restService.GetUserCourseByCourseAsync(_onlineUser.Id, courseId);
        //    OnUserCourseFetched(userCourse);

        //    return userCourse;
        //}

        //public async Task<ICollection<Course>> GetFreeCoursesAsync()
        //{
        //    return await _restService.GetFreeCoursesAsync();
        //}

        //public async Task<ICollection<Course>> GetNewCoursesAsync()
        //{
        //    return await _restService.GetNewCoursesAsync();
        //}

        //public async Task<ICollection<Category>> GetRootCategoreisAsync()
        //{
        //    return await _restService.GetRootCategoreisAsync();
        //}

        //public async Task<Category> GetCategoryAsync(int catId)
        //{
        //    var cat = await _restService.GetCategoryAsync(catId);
        //    OnCategoryFetched(cat);

        //    return cat;
        //}

        public void OnOnlineUserFetched()
        {
            if (_onlineUser != null)
                OnlineUserFetched?.Invoke(_onlineUser);
        }
        //public void OnCourseFetched(Course course)
        //{
        //    CourseFetched?.Invoke(course);
        //}
        //public void OnCategoryFetched(Category category)
        //{
        //    CategoryFetched?.Invoke(category);
        //}
        //public void OnUserCourseFetched(UserCourse userCourse)
        //{
        //    UserCourseFetched?.Invoke(userCourse);
        //}
    }

    public delegate void UserFetchedEventHandler(User user);
    //public delegate void CourseFetchedEventHandler(Course course);
    //public delegate void CategoryFetchedEventHandler(Category category);
    //public delegate void UserCourseFetchedEventHandler(UserCourse userCourse);
}
