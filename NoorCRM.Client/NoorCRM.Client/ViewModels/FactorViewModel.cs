using NoorCRM.API.Helpers;
using NoorCRM.API.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NoorCRM.Client.ViewModels
{
    public class FactorViewModel : INotifyPropertyChanged
    {
        private string _description;

        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Description
        {
            get => _description;
            set
            {
                if (_description == value)
                    return;
                _description = value;
                OnPropertyChanged();
            }
        }
        public double TotalPrice
        {
            get
            {
                double total = 0;
                foreach (var item in FactorItems)
                {
                    total += item.SumPrice;
                }

                return total;
            }
        }
        public FactorStatus Status { get; set; }
        public string CustomerTitle => Customer?.GetTitle();

        public Customer Customer { get; set; }

        public ObservableCollection<FactorItemViewModel> FactorItems { get; set; }


        public FactorViewModel(Customer customer)
        {
            CreateDate = DateTime.Now;
            Status = FactorStatus.New;
            FactorItems = new ObservableCollection<FactorItemViewModel>();
            Customer = customer;
        }

        public void AddItem(FactorItemViewModel item)
        {
            item.DeleteCommandRaise += Item_DeleteCommandRaise;
            item.PriceChanged += Item_PriceChanged;
            FactorItems.Add(item);
            OnPropertyChanged(nameof(TotalPrice));
        }

        private void Item_PriceChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(TotalPrice));
        }

        private void Item_DeleteCommandRaise(FactorItemViewModel item)
        {
            Task.Run(new Action(() =>
            {
                if (FactorItems.Contains(item))
                    FactorItems.Remove(item);
            }));
        }

        public Factor GetSubmitedFactor()
        {
            var itemList = new List<FactorItem>();
            foreach (var item in FactorItems)
            {
                itemList.Add(new FactorItem()
                {
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity,
                    SelectedPrice = item.SelectedPrice
                });
            }

            var fac = new Factor()
            {
                CreateDate = CreateDate,
                Status = Status,
                CreatorUserId = App.MainViewModel.OnlineUser.Id,
                CustomerId = Customer.Id,
                Description = Description,
                FactorItems = itemList
            };

            return fac;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class FactorItemViewModel : INotifyPropertyChanged
    {
        private int _quantity;
        private double _selectedPrice;

        public int Id { get; set; }
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity == value)
                    return;
                _quantity = value;
                OnPropertyChanged();
                OnPriceChanged();
                OnPropertyChanged(nameof(SumPrice));
            }
        }
        public double SelectedPrice
        {
            get => _selectedPrice;
            set
            {
                if (_selectedPrice == value)
                    return;
                _selectedPrice = value;
                OnPropertyChanged();
                OnPriceChanged();
                OnPropertyChanged(nameof(SumPrice));
            }
        }
        public double SumPrice => SelectedPrice * Quantity;
        public String ProductName => $"{Product?.Title} ({ProductUnitName})";
        public String ProductUnitName => Product?.Unit.PersianName();

        public Product Product { get; set; }
        public ICommand DeleteCommand => new Command<FactorItemViewModel>(
                new Action<FactorItemViewModel>(fi => OnDeleteCommandRaise(fi)));

        public FactorItemViewModel(FactorItem factorItem)
        {
            Id = factorItem.Id;
            Quantity = factorItem.Quantity;
            Product = factorItem.Product;
        }

        public FactorItemViewModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandler PriceChanged;
        public event DeleteCommandRaiseEventHandler DeleteCommandRaise;
        public void OnDeleteCommandRaise(FactorItemViewModel item)
        {
            DeleteCommandRaise?.Invoke(item);
        }
        public void OnPriceChanged([CallerMemberName] string propertyName = null)
        {
            PriceChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    public delegate void DeleteCommandRaiseEventHandler(FactorItemViewModel item);
    public delegate void ItemsCollectionEmptinessStatusEventHandler(bool isEmpty);
}
