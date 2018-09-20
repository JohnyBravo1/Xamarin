using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Test.EntityFramework;
using Xamarin.Forms;

namespace Test.ViewModels
{
    public class MainVM: INotifyPropertyChanged
    {
        public ObservableCollection<City> Items
        {
            get
            {
                return (this._items);
            }
            set
            {
                if (this._items != value)
                {
                    this._items = value;

                    PropertyChangedEventHandler handler = this.PropertyChanged;

                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("Items"));
                    }
                }
            }
        }

        public ObservableCollection<string> Sorted
        {
            get
            {
                return (this._sorted);
            }
            set
            {
                if (this._sorted != value)
                {
                    this._sorted = value;

                    PropertyChangedEventHandler handler = this.PropertyChanged;

                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("Sorted"));
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<City> _items;
        private ObservableCollection<string> _sorted;
        private Network _network;

        public MainVM()
        {
            this._items = new ObservableCollection<City>();
            this._sorted = new ObservableCollection<string>();
            this._network = new Network();

            using (var db = new AppDB())
            {
                foreach (City city in db.Cities)
                {
                    this.Items.Add(city);
                }
            }

            if (this.Items.Count == 0)
            {
                this.requestItems();
            }
        }

        public async void requestItems()
        {
            ArrayList list = await this._network.GETAsync("http://192.168.33.10/Test/index.php/json");

            if (list == null) {

                return;
            }

            using (var db = new AppDB())
            {
                db.ClearCities();
                this.Items.Clear();

                for (int itemIndex = 0; itemIndex < list.Count; itemIndex++)
                {
                    object o = list[itemIndex];

                    var city = new City { Name = o.ToString() };
                    db.Add(city);
                    db.ClearCities();

                    this.Items.Add(city);
                }
            }
        }

        public void sort()
        {
            if (this._items == null || this._items.Count == 0)
            {
                return;
            }

            City[] arr = new City[this._items.Count];
            this._items.CopyTo(arr, 0);

            Array.Sort<City>(arr);

            this.Sorted.Clear();
            foreach (var city in arr)
            {
                this.Sorted.Add(city.Name);
            }
        }
    }
}
