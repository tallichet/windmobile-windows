using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ch.Tallichet.WindMobile.Viewmodel
{
    public interface IMainViewModel
    {
        Model.Station CurrentStation { get; set; }

        List<Model.StationData> CurrentStationData { get; }
        Model.StationData CurrentStationLatestData { get; }

        void Init();

        /// <summary>
        /// List of closest stations
        /// </summary>
        ObservableCollection<Model.Station> CloseStations { get; }

        bool InProgress { get; }
    }
}
