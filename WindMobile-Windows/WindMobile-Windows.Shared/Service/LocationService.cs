using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ch.Tallichet.WindMobile.Service
{
    public class LocationService
    {
        private Windows.Devices.Geolocation.Geolocator locator = new Windows.Devices.Geolocation.Geolocator();

        public async Task<Model.Location> GetCurrentLocation()
        {
            var pos = await locator.GetGeopositionAsync();
            return new Model.Location()
            {
                Latitude = pos.Coordinate.Point.Position.Latitude,
                Longitude = pos.Coordinate.Point.Position.Longitude,
            };
        }
    }
}
