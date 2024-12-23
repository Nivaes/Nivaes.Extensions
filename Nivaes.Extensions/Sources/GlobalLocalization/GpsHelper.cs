namespace Nivaes
{
    using System;

    public static class GpsHelper
    {
        private const double EarthRadioKm = 6371.009;
        private const double PiRad = Math.PI / 180;

        public static double GeoDistance(double latitud1, double longitud1, double latititud2, double longitud2)
        {
            var latRad1 = latitud1 * PiRad;
            var latRad2 = latititud2 * PiRad;

            var longRad1 = longitud1 * PiRad;
            var longRad2 = longitud2 * PiRad;

            double distanceRad = Math.Acos(Math.Cos(latRad1) * Math.Cos(latRad2) * Math.Cos(longRad2 - longRad1) + Math.Sin(latRad1) * Math.Sin(latRad2));

            double distanceKm = EarthRadioKm * distanceRad;

            return distanceKm;
        }
    }
}
