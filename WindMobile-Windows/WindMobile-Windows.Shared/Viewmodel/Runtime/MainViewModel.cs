using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;

namespace Ch.Tallichet.WindMobile.Viewmodel.Runtime
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public MainViewModel()
        {
            CloseStations = new System.Collections.ObjectModel.ObservableCollection<Model.Station>();
            Init();
        }

        public System.Collections.ObjectModel.ObservableCollection<Model.Station> CloseStations
        {
            get; private set;
        }

        public async void Init()
        {
            var location = await ServiceLocator.Current.GetInstance<Service.LocationService>().GetCurrentLocation();
            if (location != null)
            {
                this.SetLocation(location);
            }
            else
            {
                var stations = await ServiceLocator.Current.GetInstance<Service.NetworkService>().ListStations(limit: 1);
                if (stations.Count > 0)
                {
                    CurrentStation = stations.Where(s => s.IsValid).First();
                }
            }
        }

        private bool inProgress;
        public bool InProgress
        {
            get { return inProgress; }
            set
            {
                if (value != inProgress) // TODO Change to use a counter
                {
                    inProgress = value;
                    base.RaisePropertyChanged(() => this.InProgress);
                }
            }
        }

        private Model.Station currentStation = null;
        public Model.Station CurrentStation
        {
            get
            {
                return currentStation;   
            }
            set
            {
                currentStation = value;
                RaisePropertyChanged(() => this.CurrentStation);
                refreshCurrentStationData();
            }
        }

        public List<Model.StationData> CurrentStationData
        {
            get; private set;
        }

        public Model.StationData CurrentStationLatestData
        {
            get { return CurrentStationData == null ? null : CurrentStationData.FirstOrDefault(); }
        }

        private async void SetLocation(Model.Location CurrentLocation)
        {
            InProgress = true;
            currentLocation = CurrentLocation;

            await refreshCloseStations();

            InProgress = false;
        }

        #region refreshing data

        private Model.Location currentLocation;
        private long distanceInMetersForGetSearch = 20000;
        private TimeSpan dataDefaultDuration = TimeSpan.FromHours(1);

        private async Task refreshCloseStations()
        {
            CloseStations.Clear();
            CurrentStation = null;
            try
            {
                var stations = await ServiceLocator.Current.GetInstance<Service.NetworkService>().GeoSearchStations(currentLocation, distanceInMetersForGetSearch);
                foreach (var station in stations.Where(s => s.IsValid))
                {
                    CloseStations.Add(station);
                    if (CurrentStation == null)
                    {
                        CurrentStation = station;
                    }
                }
            }
            catch (Exception)
            {
                var resource = ResourceLoader.GetForCurrentView("messages");
                var dlg = new MessageDialog(resource.GetString("NoStationsAvailable"));
#pragma warning disable 4014
                dlg.ShowAsync();
#pragma warning restore 4014
            }
        }

        private async void refreshCurrentStationData()
        {
            InProgress = true;

            if (currentStation == null)
            {
                CurrentStationData = null;
            }
            else
            {
                CurrentStationData = await ServiceLocator.Current.GetInstance<Service.NetworkService>().GetStationData(currentStation.ID, dataDefaultDuration);
            }
            RaisePropertyChanged(() => CurrentStationData);
            RaisePropertyChanged(() => CurrentStationLatestData);

            InProgress = false;
        }
        
        #endregion


    }
}
