﻿using NoorCRM.API.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoorCRM.Client.Pages.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RemiderListUC : ContentView
    {
        public ObservableCollection<Customer> Customers
        {
            get { return (ObservableCollection<Customer>)GetValue(CustomersProperty); }
            set { SetValue(CustomersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Courses.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty CustomersProperty =
            BindableProperty.Create(
                nameof(Customers),
                typeof(ObservableCollection<Customer>),
                typeof(RemiderListUC),
                null,
                BindingMode.TwoWay,
                propertyChanged: HandleCustomersChanged);

        private static ObservableCollection<CustomerCardInfo> cardInfos;
        private static void HandleCustomersChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var customers = newValue as ObservableCollection<Customer>;
            if (customers != null)
            {
                customers.CollectionChanged += Customers_CollectionChanged;
                cardInfos = new ObservableCollection<CustomerCardInfo>();
                foreach (var c in customers)
                    cardInfos.Add(new CustomerCardInfo(c, App.NavigationPage.Navigation));

                var cluc = (RemiderListUC)bindable;
                cluc.stkContainer.ItemsSource = cardInfos;
            }
        }

        private static void Customers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Customer item in e.NewItems)
                {
                    cardInfos.Add(new CustomerCardInfo(item, App.NavigationPage.Navigation));
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Customer item in e.OldItems)
                {
                    var cust = cardInfos.Where(c => ReferenceEquals(c.Customer, item)).FirstOrDefault();
                    if (cust != null)
                        cardInfos.Remove(cust);
                }
            }
        }

        public RemiderListUC()
        {
            InitializeComponent();
        }

        private void stkContainer_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var card = e.Item as CustomerCardInfo;
            if (card != null)
            {
                card.TapCommand.Execute(card.Customer);
            }
        }
    }
}