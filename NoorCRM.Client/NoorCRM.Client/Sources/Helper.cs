using NoorCRM.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms.Maps;

namespace NoorCRM.Client.Sources
{
    public static class Helper
    {
        public static string CreateCustomerTitle(this Customer customer)
        {
            if (customer == null)
                return "";

            if (!string.IsNullOrWhiteSpace(customer.StoreName))
                return $"{customer.StoreName.Trim()} ({customer.ManagerName.Trim()})";

            return customer.ManagerName.Trim();
        }

        public static void ShowAllPins(this Map map, Pin userPosition = null)
        {
            if (map == null || map.Pins == null)
                return;

            var latitudes = new List<double>();
            var longitudes = new List<double>();
            foreach (var item in map.Pins)
            {
                latitudes.Add(item.Position.Latitude);
                longitudes.Add(item.Position.Longitude);
            }

            if (userPosition != null)
            {
                latitudes.Add(userPosition.Position.Latitude);
                longitudes.Add(userPosition.Position.Longitude);
            }

            if (latitudes.Count == 0)
            {
                map.MoveToRegion(new MapSpan(SoftwareSettings.HomeLocation, 0.01, 0.01));
                return;
            }

            if (latitudes.Count == 1)
            {
                map.MoveToRegion(new MapSpan(new Position(latitudes[0], longitudes[0]), 0.01, 0.01));
                return;
            }

            double lowestLat = latitudes.Min();
            double highestLat = latitudes.Max();
            double lowestLong = longitudes.Min();
            double highestLong = longitudes.Max();
            double finalLat = (lowestLat + highestLat) / 2;
            double finalLong = (lowestLong + highestLong) / 2;
            double distance = DistanceCalculation.GeoCodeCalc.CalcDistance(lowestLat, lowestLong, highestLat, highestLong, DistanceCalculation.GeoCodeCalcMeasurement.Kilometers);

            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(finalLat, finalLong), Distance.FromKilometers(distance/2)));
        }
    }
}
