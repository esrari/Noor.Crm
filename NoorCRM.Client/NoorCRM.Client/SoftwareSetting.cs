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
        public static string BaseAddress = (Device.RuntimePlatform == Device.Android ? "http://10.0.2.2:5000" : "https://localhost:5001") + "/api";
        //public static string BaseAddress = "http://148.251.104.158:5000" + "/api";
    }
}
