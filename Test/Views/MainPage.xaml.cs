using System;
using System.Diagnostics;
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

            MainVM vm = new MainVM();
            this.BindingContext = vm;

            MessagingCenter.Subscribe<MainVM>(this, "RequestFailed", (sender) =>
            {
                DisplayAlert("Failed to process request", "", "Ok");
                vm.RequestRegions();
            });
        }

        public async void OnCitySelected(object sender, EventArgs e)
        {
            Clock cityClock = new Clock();
            cityClock.BindingContext = this.listView.SelectedItem as EFCity;
            await this.Navigation.PushAsync(cityClock);
        }
    }
}
