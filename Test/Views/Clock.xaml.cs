using System;
using System.Collections.Generic;

using Test.EntityFramework;

using Xamarin.Forms;

namespace Test
{
    public partial class Clock : ContentPage
    {
        public Clock()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            EFCity context = this.BindingContext as EFCity;
            this.lblCityName.Text = context.Name;
            this.Title = context.region.Name;
        }
    }
}
