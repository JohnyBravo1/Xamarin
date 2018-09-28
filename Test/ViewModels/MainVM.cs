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
    public class CitiesList: List<EFCity>
    {
        public string Header { get; set; }
        public string ShortHeader { get; set; }

        public List<EFCity> cities => this;
    }

    public class MainVM: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<CitiesList> GroupedList { get; set; }

        private AppNetwork _network;
        private IList<EFRegion> _regions;

        public MainVM()
        {
            this._regions = new List<EFRegion>();
            this._network = new AppNetwork();

            using (var db = new AppDB())
            {
                foreach (EFRegion region in db.Regions)
                {
                    this._regions.Add(region);
                }
            }
            if (this._regions.Count == 0)
            {
                this.RequestRegions();
            }
            else
            {
                this._constructGroupedList();
            }
        }

        public async void RequestRegions()
        {
            IList<EFRegion> regions = await this._network.requestRegions();

            if (regions == null)
            {
                MessagingCenter.Send<MainVM>(this, "RequestFailed");
                return;
            }

            using (var db = new AppDB())
            {
                db.ClearRegions();
                this._regions.Clear();

                foreach (EFRegion region in regions)
                {
                    db.Add<EFRegion>(region);

                    this._regions.Add(region);
                }
            }

            this._constructGroupedList();

            MessagingCenter.Send<MainVM>(this, "RegionsResponse");
        }

        private void _constructGroupedList()
        {
            if (this.GroupedList == null)
            {
                this.GroupedList = new ObservableCollection<CitiesList>();
            }
            else
            {
                this.GroupedList.Clear();
            }

            CitiesList list;

            foreach (EFRegion region in this._regions)
            {
                list = new CitiesList();

                list.AddRange(region.Cities);
                list.Header = region.Name;
                list.ShortHeader = region.Name.Substring(0, 1).ToUpper();

                this.GroupedList.Add(list);
            }

            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs("GroupedList"));
            }
        }
    }
}
