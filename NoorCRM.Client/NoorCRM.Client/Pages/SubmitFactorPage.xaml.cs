using NoorCRM.API.Models;
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
        public event FactorDeletedEventHandler FactorDeleted;
        bool _inProccess = false;

        public SubmitFactorPage(Customer customer)
        {
            InitializeComponent();
            _customer = customer;
            _viewModel = new FactorViewModel(customer);
            BindingContext = _viewModel;

            if (!_viewModel.EditPossible)
                btnSubmit.IsVisible = false;
            btnDelete.IsVisible = false;
        }

        public SubmitFactorPage(Factor factor)
        {
            InitializeComponent();
            _customer = factor?.Customer;
            _viewModel = new FactorViewModel(factor);
            BindingContext = _viewModel;

            if (!_viewModel.EditPossible)
            {
                btnSubmit.IsVisible = false;
                btnDelete.IsVisible = false;
            }
        }

        private void BtnAddItem_Clicked(object sender, EventArgs e)
        {
            var addItemPage = new AddFactorItemPage();
            addItemPage.Disappearing += addItemPage_Disappearing;
            App.NavigationPage.Navigation.PushModalAsync(addItemPage);
        }

        private async void addItemPage_Disappearing(object sender, EventArgs e)
        {
            var page = sender as AddFactorItemPage;
            if (page != null && page.SelectedProducts != null)
            {
                var insertedList = new List<FactorItemViewModel>();
                foreach (var item in page.SelectedProducts)
                {
                    var inserted = await addItem(item).ConfigureAwait(true);
                    if (inserted != null)
                        insertedList.Add(inserted);
                }
                setPrice(insertedList);
            }
        }

        private async Task<FactorItemViewModel> addItem(Product selectedProduct)
        {
            bool itsNew = true;
            foreach (var fi in _viewModel.FactorItems)
                if (fi.Product.Id == selectedProduct.Id)
                {
                    itsNew = false;
                    break;
                }

            if (itsNew)
            {
                var item = new FactorItemViewModel()
                {
                    Product = selectedProduct
                };
                _viewModel.AddItem(item);
                return item;
            }
            else
            {
                await MaterialDialog.Instance.SnackbarAsync(message: $"کالای '{selectedProduct.Title}' در لیست موجود می باشد.",
                    msDuration: MaterialSnackbar.DurationShort).ConfigureAwait(true);
                return null;
            }
        }

        private void setPrice(IEnumerable<FactorItemViewModel> factorItems = null)
        {
            if (factorItems == null)
                factorItems = _viewModel.FactorItems;

            foreach (var item in factorItems)
            {
                // Cheque payment
                if (swtChequePayment.IsToggled)
                    item.SelectedPrice = item.Product.Price1ch;
                // Cash
                else
                    item.SelectedPrice = item.Product.Price1;
            }
        }

        private async void Submit_Clicked(object sender, EventArgs e)
        {
            if (_inProccess)
                return;

            if (!_viewModel.FactorItems.Any())
                return;

            if (_viewModel.Status != FactorStatus.New)
                return;

            _inProccess = true;
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

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            if (_inProccess)
                return;

            if (_viewModel.Status != FactorStatus.New)
                return;

            var confirm = await MaterialDialog.Instance.ConfirmAsync(message: "آیا از حذف این فاکتور مطمئن هستید؟",
                    confirmingText: "بله",
                    dismissiveText: "خیر").ConfigureAwait(true);

            if (confirm.HasValue && confirm.Value)
            {
                _inProccess = true;
                var factorId = _viewModel.GetFactorId();

                if (factorId != 0)
                {
                    bool result = await App.ApiService.DeleteFactorAndLogAsync(factorId).ConfigureAwait(true);
                    if (result)
                    {
                        var fact = App.MainViewModel.LastFactors.FirstOrDefault(f => f.Id == factorId);
                        if (fact != null)
                            App.MainViewModel.LastFactors.Remove(fact);
                        OnFactorDeleted(fact);
                        await MaterialDialog.Instance.SnackbarAsync(message: "حذف فاکتور با موفقیت انجام شد.",
                            msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(true);
                    }
                    else
                    {
                        await MaterialDialog.Instance.SnackbarAsync(message: "حذف فاکتور با مشکل مواجه شد.",
                            msDuration: MaterialSnackbar.DurationLong).ConfigureAwait(true);
                    }
                }

                await App.NavigationPage.Navigation.PopAsync().ConfigureAwait(false);
            }
        }

        private void OnPageClosed(bool successful, SuccessfulLog log)
        {
            PageClosed?.Invoke(successful, log);
        }

        private void OnFactorDeleted(Factor factor)
        {
            FactorDeleted?.Invoke(factor);
        }

        private void txtQuantity_Focused(object sender, FocusEventArgs e)
        {
            var ent = sender as Entry;
            ent.SelectionLength = ent.Text.Length;
        }

        private void swtChequePayment_Toggled(object sender, ToggledEventArgs e)
        {
            setPrice(_viewModel.FactorItems);
        }
    }
}