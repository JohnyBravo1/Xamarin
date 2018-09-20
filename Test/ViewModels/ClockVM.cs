using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Test.ViewModels
{
    public class ClockVM: INotifyPropertyChanged
    {
        private DateTime _dateTime = DateTime.Now;
        private int _ticks;
        private String _message = "CLOCK VM";
        public event PropertyChangedEventHandler PropertyChanged;

        public ClockVM()
        {
            this._ticks = 0;
            Device.StartTimer(TimeSpan.FromSeconds(1), OnTimerTick);
        }

        private bool OnTimerTick()
        {
            this._ticks++;

            this.Message = String.Format("Timer active for {0} seconds", this._ticks);

            return (true);
        }

        public DateTime CurrentTime
        {
            get
            {
                return (this._dateTime);
            }
            set
            {
                if (this._dateTime != value)
                {
                    this._dateTime = value;

                    PropertyChangedEventHandler handler = PropertyChanged;

                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("CurrentTime"));
                    }
                }
            }
        }

        public String Message
        {
            get 
            {
                return (this._message);
            }
            set
            {
                if (this._message != value)
                {
                    this._message = value;

                    PropertyChangedEventHandler handler = PropertyChanged;

                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("Message"));
                    }
                }
                this._message = value;
            }
        }
    }
}
