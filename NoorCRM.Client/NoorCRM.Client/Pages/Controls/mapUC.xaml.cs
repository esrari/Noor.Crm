using NoorCRM.Client.Sources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace NoorCRM.Client.Pages.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class mapUC : ContentView
    {
        public ObservableCollection<Pin> Pins
        {
            get { return (ObservableCollection<Pin>)GetValue(PinsProperty); }
            set { SetValue(PinsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty PinsProperty =
            BindableProperty.Create(
                nameof(Pins),
                typeof(ObservableCollection<Pin>),
                typeof(mapUC),
                null,
                BindingMode.TwoWay,
                propertyChanged: HandleMessagesChanged);

        private static ObservableCollection<Pin> _pins;
        private static mapUC _mapuc;
        private static void HandleMessagesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            _pins = newValue as ObservableCollection<Pin>;
            if (_pins != null)
            {
                //_pins.CollectionChanged += Messages_CollectionChanged;
                _mapuc = bindable as mapUC;
                _mapuc.map.Pins.Clear();
                foreach (var item in _pins)
                {
                    _mapuc.map.Pins.Add(item);
                }
                
                _mapuc.map.ShowAllPins();
            }
        }

        //private static async void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == NotifyCollectionChangedAction.Add)
        //    {
        //        foreach (Message item in e.NewItems)
        //            _messageBoxes.Add(new MessageBoxViewModel(item));

        //        await _mapuc.scvScroller.ScrollToAsync(_mapuc.lstMessages, ScrollToPosition.End, false).ConfigureAwait(false);
        //    }
        //    else if (e.Action == NotifyCollectionChangedAction.Remove)
        //    {
        //        foreach (Message item in e.OldItems)
        //        {
        //            var m = _messageBoxes.Where(c => ReferenceEquals(c.Message, item)).FirstOrDefault();
        //            if (m != null)
        //                _messageBoxes.Remove(m);
        //        }
        //    }
        //    else if (e.Action == NotifyCollectionChangedAction.Replace)
        //    {
        //        foreach (Message item in e.NewItems)
        //        {
        //            foreach (Message oitem in e.OldItems)
        //            {
        //                var m = _messageBoxes.Where(c => ReferenceEquals(c.Message, oitem)).FirstOrDefault();
        //                m.Update(item);
        //            }
        //        }
        //    }
        //}

        public mapUC()
        {
            InitializeComponent();
        }

        public void SetZoomLevel(double zoomLevel)
        {
            double latlongDegrees = 360 / (Math.Pow(2, zoomLevel));
            if (map.VisibleRegion != null)
            {
                map.MoveToRegion(new MapSpan(map.VisibleRegion.Center, latlongDegrees, latlongDegrees));
            }
        }

        private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            SetZoomLevel(e.NewValue);
        }

        private void map_MapClicked(object sender, MapClickedEventArgs e)
        {

        }
    }
}