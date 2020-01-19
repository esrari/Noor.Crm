using NoorCRM.API.Helpers;
using NoorCRM.API.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoorCRM.Client.ViewModels
{
    public delegate void ProductSelectedEventHandler(SelectedProduct selectedProduct);

    public class SelectedProduct
    {
        public Product Product { get; set; }
        public double SelectedPrice { get; set; }

        public SelectedProduct(Product product, double price)
        {
            Product = product;
            SelectedPrice = price;
        }
    }

    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double ExistedQuantity { get; set; }
        public ProductUnit Unit { get; set; }

        public double Price1 { get; set; }
        public double Price2 { get; set; }
        public double Price3 { get; set; }
        public double Price1ch { get; set; }
        public double Price2ch { get; set; }
        public double Price3ch { get; set; }
        public Product Product { get; set; }

        public ProductViewModel(Product product)
        {
            Product = product;
            Id = product.Id;
            Title = $"{product.Title} ({product.Unit.PersianName()})";
            ExistedQuantity = product.ExistedQuantity;
            Unit = product.Unit;
            Price1 = product.Price1;
            Price2 = product.Price2;
            Price3 = product.Price3;
            Price1ch = product.Price1ch;
            Price2ch = product.Price2ch;
            Price3ch = product.Price3ch;
        }
    }
}
