using NoorCRM.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoorCRM.Client.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodayView : ContentView
    {
        public MainViewModel MainViewModel
        {
            get { return (MainViewModel)GetValue(MainViewModelProperty); }
            set { SetValue(MainViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Courses.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty MainViewModelProperty =
            BindableProperty.Create(
                nameof(MainViewModel),
                typeof(MainViewModel),
                typeof(TodayView),
                null,
                BindingMode.TwoWay,
                propertyChanged: handleViewModelChanged);

        private static void handleViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var viewModel = newValue as MainViewModel;
            if (viewModel != null)
            {
                var cluc = (TodayView)bindable;
                cluc.BindingContext = viewModel;
            }
        }
        public TodayView()
        {
            InitializeComponent();
        }
    }
}