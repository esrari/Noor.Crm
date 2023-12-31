﻿using NoorCRM.API.Models;
using NoorCRM.Client.Pages.Controls;
using NoorCRM.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace NoorCRM.Client.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubmitFactorPage : ContentPage
    {
        private readonly Customer _customer;
        private FactorViewModel _viewModel;
        public event PageClosedEventHandler PageClosed;

        public SubmitFactorPage(Customer customer)
        {
            InitializeComponent();
            App.NavigationPage.BarBackgroundColor = Color.White;
            _customer = customer;
            _viewModel = new FactorViewModel(customer);
            BindingContext = _viewModel;

            if (!_viewModel.EditPossible)
                _viewModel.IconSource = "empty.png";
            else
                _viewModel.IconSource = "submit.png";

            App.AddItemPage.ProductSelected += AddItemPage_ProductSelected;
        }

        public SubmitFactorPage(Factor factor)
        {
            InitializeComponent();
            _customer = factor?.Customer;
            _viewModel = new FactorViewModel(factor);
            BindingContext = _viewModel;
        }

        private void BtnAddItem_Clicked(object sender, EventArgs e)
        {
            //var addItemPage = new AddFactorItemPage();
            //addItemPage.ProductSelected += AddItemPage_ProductSelected;
            //App.NavigationPage.Navigation.PushAsync(addItemPage);

            App.NavigationPage.Navigation.PushAsync(App.AddItemPage);
        }

        private async void AddItemPage_ProductSelected(SelectedProduct selectedProduct)
        {
            bool itsNew = true;
            foreach (var fi in _viewModel.FactorItems)
                if (fi.Product.Id == selectedProduct.Product.Id)
                {
                    itsNew = false;
                    break;
                }

            if (itsNew)
            {
                var item = new FactorItemViewModel()
                {
                    Product = selectedProduct.Product,
                    SelectedPrice = selectedProduct.SelectedPrice
                };
                _viewModel.AddItem(item);
            }
            else
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "کالای انتخاب شده در لیست موجود می باشد.",
                    msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(true);
            }
        }

        private async void Submit_Clicked(object sender, EventArgs e)
        {
            if (!_viewModel.FactorItems.Any())
                return;

            if (_viewModel.Status != FactorStatus.New)
                return;

            var factor = _viewModel.GetSubmitedFactor();

            if (factor.Id != 0)
            {
                await App.ApiService.UpdateFactorAsync(factor).ConfigureAwait(true);
                OnPageClosed(successful: true, null);
                await MaterialDialog.Instance.SnackbarAsync(message: "اصلاح فاکتور با موفقیت انجام شد.",
                    msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(true);
            }
            else
            {
                var successLog = new SuccessfulLog()
                {
                    CreationDate = DateTime.Now,
                    CreatorUserId = App.MainViewModel.OnlineUser.Id,
                    CustomerId = factor.CustomerId,
                    IsVisitorsLog = true
                };

                var sucLog = await App.ApiService.InsertNewFactorAsync(factor, successLog).ConfigureAwait(true);
                if (sucLog != null)
                {
                    sucLog.Factor = factor;
                    OnPageClosed(successful: true, sucLog);
                    await MaterialDialog.Instance.SnackbarAsync(message: "افزودن فاکتور جدید با موفقیت انجام شد.",
                        msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(true);
                }
                else
                {
                    OnPageClosed(successful: false, null);
                    await MaterialDialog.Instance.SnackbarAsync(message: "افزودن فاکتور جدید با مشکل روبرو شد.",
                        msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(true);
                }
            }

            await App.NavigationPage.Navigation.PopAsync().ConfigureAwait(false);
        }

        private void OnPageClosed(bool successful, SuccessfulLog log)
        {
            PageClosed?.Invoke(successful, log);
        }

        private void txtQuantity_Focused(object sender, FocusEventArgs e)
        {
            var ent = sender as Entry;
            ent.SelectionLength = ent.Text.Length;
        }
    }
}