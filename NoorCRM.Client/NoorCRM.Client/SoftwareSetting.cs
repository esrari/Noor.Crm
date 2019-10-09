using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NoorCRM.Client
{
    public static class SoftwareSettings
    {
        // Server Configs
        // The iOS simulator can connect to localhost. However, Android emulators must use the 10.0.2.2 special alias to your host loopback interface.
        public static string BaseAddress = (Device.RuntimePlatform == Device.Android ? "https://10.0.2.2:5001" : "https://localhost:5001") + "/api";
        //public static string BaseAddress = (Device.RuntimePlatform == Device.Android ? "https://192.168.0.131:5001" : "https://localhost:5001") + "/api";
        public static string UsersUrl = BaseAddress + "/Users/{0}";
        public static string UserCoursesUrl = BaseAddress + "/UserCourses/{0}/{1}";
        public static string CoursesUrl = BaseAddress + "/Courses/{0}";
        public static string CategoreisUrl = BaseAddress + "/Categoreis/{0}";
    }
}
