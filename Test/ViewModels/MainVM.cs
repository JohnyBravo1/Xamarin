using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Test.EntityFramework;
using Test.Network;
using Xamarin.Forms;

namespace Test.ViewModels
{
    public class MainVM: INotifyPropertyChanged
    {
        public ObservableCollection<EFCity> Cities
        {
            get
            {
                return (this._cities);
            }
            set
            {
                if (this._cities != value)
                {
                    this._cities = value;

                    PropertyChangedEventHandler handler = this.PropertyChanged;

                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("Items"));
                    }
                }
            }
        }

        public ObservableCollection<EFRegion> Regions
        {
            get
            {
                return (this._regions);
            }
            set
            {
                if (this._regions != value)
                {
                    this._regions = value;

                    PropertyChangedEventHandler handler = this.PropertyChanged;

                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("Sorted"));
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<EFCity> _cities;
        private ObservableCollection<EFRegion> _regions;
        private AppNetwork _network;

        public MainVM()
        {
            this._cities = new ObservableCollection<EFCity>();
            this._regions = new ObservableCollection<EFRegion>();
            this._network = new AppNetwork();

            using (var db = new AppDB())
            {
                foreach (EFCity city in db.Cities)
                {
                    this.Cities.Add(city);
                }
            }

            if (this.Cities.Count == 0)
            {
                this.RequestCities();
            }
        }

        public async void RequestCities()
        {
            IList<EFCity> cities = await this._network.requestCities();

            if (cities == null) {

                MessagingCenter.Send<MainVM>(this, "RequestFailed");
                return;
            }

            using (var db = new AppDB())
            {
                db.ClearCities();
                this.Cities.Clear();

                foreach (EFCity city in cities)
                {
                    db.Add<EFCity>(city);

                    this.Cities.Add(city);
                }
            }
        }

        public async void RequestRegions()
        {
            IList<EFRegion> regions = await this._network.requestRegions();

            if (regions == null) {

                MessagingCenter.Send<MainVM>(this, "RequestFailed");
                return;
            }

            using (var db = new AppDB())
            {
                db.ClearRegions();
                foreach (EFRegion region in regions)
                {
                    db.Add<EFRegion>(region);

                    this.Regions.Add(region);
                }
            }
        }
    }
}
