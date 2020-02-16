using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoorCRM.Client.Pages.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        private readonly MenuPageViewModel _menuPageViewModel;

        public MenuPage(MenuPageViewModel menuPageViewModel)
        {
            //this.IconImageSource = "yourHamburgerIcon.png"; //only neeeded for ios
            InitializeComponent();
            _menuPageViewModel = menuPageViewModel;
            BindingContext = menuPageViewModel;
        }
    }
}