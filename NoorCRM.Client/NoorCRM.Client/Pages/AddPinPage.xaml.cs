using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace NoorCRM.Client.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPinPage : ContentPage
    {
        public Position? Position { get; private set; }
        public bool PageSubmitted { get; private set; }

        public AddPinPage()
        {
            InitializeComponent();
            PageSubmitted = false;
            map.MoveToRegion(new MapSpan(SoftwareSettings.HomeLocation, 0.1, 0.1));
        }

        public AddPinPage(Position position)
        {
            InitializeComponent();
            PageSubmitted = false;
            Position = position;
            btnSave.IsEnabled = true;
            setPin(position);
            map.MoveToRegion(new MapSpan(position, 0.1, 0.1));
        }

        private void map_MapClicked(object sender, MapClickedEventArgs e)
        {
            setPin(e.Position);
        }

        private void setPin(Position position)
        {
            var pin = new Pin()
            {
                Position = position,
                Label = "موقعیت مورد نظر"
            };
            Position = position;
            btnSave.IsEnabled = true;

            map.Pins.Clear();
            map.Pins.Add(pin);
        }

        private void BtnCancel_Clicked(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PopModalAsync(true);
        }

        private void BtnSave_Clicked(object sender, EventArgs e)
        {
            PageSubmitted = true;
            App.NavigationPage.Navigation.PopModalAsync(true);
        }

        private void btnRemove_Clicked(object sender, EventArgs e)
        {
            Position = null;
            PageSubmitted = true;
            App.NavigationPage.Navigation.PopModalAsync(true);
        }
    }
}