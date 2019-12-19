using NoorCRM.API.Models;
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
            if(logs!=null)
            {
                var lluc = bindable as LogListUC;
                foreach (var item in logs)
                {
                    if(item is CommentLog)
                    {
                        var clog = new CommentLogBox(item as CommentLog);
                        lluc.stkLogs.Children.Add(clog);
                    }

                    // Todo: other types of log here
                }
            }
        }

        public LogListUC()
        {
            InitializeComponent();
        }
    }
}