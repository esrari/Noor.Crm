﻿using NoorCRM.API.Models;
using NoorCRM.Client.Pages.Controls.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoorCRM.Client.Pages.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogListUC : ContentView
    {
        public IEnumerable<CustomerLog> CustomerLogs
        {
            get { return (IEnumerable<CustomerLog>)GetValue(CustomerLogsProperty); }
            set { SetValue(CustomerLogsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty CustomerLogsProperty =
            BindableProperty.Create(
                nameof(CustomerLogs),
                typeof(IEnumerable<CustomerLog>),
                typeof(LogListUC),
                null,
                 BindingMode.TwoWay,
                 propertyChanged: HandleCustomerLogsChanged);

        private static void HandleCustomerLogsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var logs = newValue as IEnumerable<CustomerLog>;
            if (logs != null)
            {
                var lluc = bindable as LogListUC;
                lluc.stkLogs.Children.Clear();
                var revLogs = logs.Reverse();
                foreach (var item in revLogs)
                {
                    var log = new LogBox(item);
                    lluc.stkLogs.Children.Add(log);
                }
                lluc.sclLogs.ScrollToAsync(lluc.sclLogs, ScrollToPosition.End, false);
            }
        }

        public static void AddLog(LogListUC lluc, CustomerLog item)
        {
            if (lluc != null && item != null)
            {
                var log = new LogBox(item);
                lluc.stkLogs.Children.Add(log);
                lluc.sclLogs.ScrollToAsync(lluc.sclLogs, ScrollToPosition.End, false);
            }
        }

        public LogListUC()
        {
            InitializeComponent();
        }
    }
}