using NoorCRM.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace NoorCRM.Client.Pages.Controls
{
    public class SelectProductCardInfo : INotifyPropertyChanged
    {
        private bool _selected;

        public Product Product { get; }
        public string Title => Product.Title;
        public double ExistedQuantity => Product.ExistedQuantity;
        public double Price1 => Product.Price1;
        public double Price1ch => Product.Price1ch;
        public ProductUnit Unit => Product.Unit;
        public bool Selected
        {
            get => _selected;
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    OnPropertyChanged();
                }
            }
        }
        public SelectProductCardInfo(Product product)
        {
            Product = product;
        }

        public void ProductChanged()
        {
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(ExistedQuantity));
            OnPropertyChanged(nameof(Price1));
            OnPropertyChanged(nameof(Price1ch));
            OnPropertyChanged(nameof(Unit));
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
