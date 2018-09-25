using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.ViewModels;
using Test.EntityFramework;
using Xamarin.Forms;

namespace Test
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            this.BindingContext = new ViewModels.MainVM();

            MessagingCenter.Subscribe<Test.ViewModels.MainVM>(this, "RequestFailed", (sender) =>
            {
                DisplayAlert("Failed to process request", "", "Ok");
            });
        }

        void RequestCities(object sender, System.EventArgs e)
        {
            ViewModels.MainVM vm = this.BindingContext as ViewModels.MainVM;

            vm.RequestCities();
        }

        async void OnCitySelected(object sender, System.EventArgs e)
        {
            await this.Navigation.PushAsync(new Clock());
        }

        void OnRegionSelected(object sender, System.EventArgs e)
        {
            ViewModels.MainVM vm = this.BindingContext as ViewModels.MainVM;
            EFRegion region = this.ListViewRegions.SelectedItem as EFRegion;

            if (region != null) {

                vm.Cities = new ObservableCollection<EFCity>(region.cities);
            }
        }

        void RequestRegions(object sender, System.EventArgs e)
        {
            ViewModels.MainVM vm = this.BindingContext as ViewModels.MainVM;

            vm.RequestRegions();
        }
    }
}
