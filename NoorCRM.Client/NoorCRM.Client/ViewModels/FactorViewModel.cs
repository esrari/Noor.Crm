using NoorCRM.API.Helpers;
using NoorCRM.API.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
        private bool _editMode = false;
        private string _iconSource;
        private string _deleteIconSource;
        private readonly Factor _factorForEdit;

        public int Id { get; set; }
        public string IconSource
        {
            get => _iconSource;
            set
            {
                if (_iconSource == value)
                    return;
                _iconSource = value;
                OnPropertyChanged();
            }
        }        
        public string DeleteIconSource
        {
            get => _deleteIconSource;
            set
            {
                if (_deleteIconSource == value)
                    return;
                _deleteIconSource = value;
                OnPropertyChanged();
            }
        }
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
        public string StatusPersianName => Status.PersianName();
        public string CustomerTitle => Customer?.GetTitle();
        public bool EditPossible
        {
            get
            {
                if (!_editMode)
                    return true;
                if (_editMode && Status == FactorStatus.New)
                    return true;
                return false;
            }
        }

        public Customer Customer { get; set; }

        public ObservableCollection<FactorItemViewModel> FactorItems { get; set; }


        public FactorViewModel(Customer customer)
        {
            CreateDate = DateTime.Now;
            Status = FactorStatus.New;
            FactorItems = new ObservableCollection<FactorItemViewModel>();
            Customer = customer;
        }

        public FactorViewModel(Factor factor)
        {
            _editMode = true;
            CreateDate = factor.CreateDate;
            Status = factor.Status;
            FactorItems = new ObservableCollection<FactorItemViewModel>();
            Customer = factor.Customer;

            if (factor.FactorItems != null && factor.FactorItems.Any())
                foreach (var item in factor.FactorItems)
                    AddItem(new FactorItemViewModel(item));
            _factorForEdit = factor;
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

        public int GetFactorId()
        {
            if (_editMode)
                return _factorForEdit.Id;
            return 0;
        }

        public Factor GetSubmitedFactor()
        {
            var itemList = new List<FactorItem>();
            foreach (var item in FactorItems)
            {
                if (item.Quantity > 0)
                {
                    var fi = new FactorItem()
                    {
                        ProductId = item.Product.Id,
                        Product = item.Product,
                        Quantity = item.Quantity,
                        SelectedPrice = item.SelectedPrice,
                    };
                    itemList.Add(fi);
                    if (_editMode)
                        fi.FactorId = _factorForEdit.Id;
                }
            }

            Factor fac;
            if (_editMode)
            {
                fac = _factorForEdit;
                fac.Description = Description;
                fac.FactorItems = itemList;
            }
            else
                fac = new Factor()
                {
                    CreateDate = CreateDate,
                    Status = Status,
                    CreatorUserId = App.MainViewModel.OnlineUser.Id,
                    CustomerId = Customer.Id,
                    Customer = Customer,
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
        public double _quantity;
        public double _selectedPrice;

        public int Id { get; set; }
        public double Quantity
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
            SelectedPrice = factorItem.SelectedPrice;
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
