using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Test
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            this.BindingContext = new ViewModels.MainVM();
        }

        void apiCallAsync(object sender, System.EventArgs e)
        {
            ViewModels.MainVM vm = this.BindingContext as ViewModels.MainVM;

            vm.requestItems();
        }

        async void onItemSelected(object sender, System.EventArgs e)
        {
            await this.Navigation.PushAsync(new Clock());
        }

        void sort(object sender, System.EventArgs e)
        {
            ViewModels.MainVM vm = this.BindingContext as ViewModels.MainVM;

            vm.sort();
        }
    }
}
