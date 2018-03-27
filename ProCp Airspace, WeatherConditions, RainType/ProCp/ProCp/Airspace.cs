using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCp
{
    class Airspace
    {
        public Airport Airport
        { get; set; }

        public List<Checkpoint> AvailableCheckpoints
        { get; set; }

        public List<Airplane> AirplanesInTheAir
        { get; set; }

        public WeatherConditions WeatherConditions
        { get; set; }

        public Airspace(Airport airport, List<Checkpoint> availableCheckpoints, List<Airplane> airplanesInTheAir, WeatherConditions weatherConditions)
        {
            Airport = airport;
            AvailableCheckpoints = availableCheckpoints;
            AirplanesInTheAir = airplanesInTheAir;
            WeatherConditions = weatherConditions;
        }

        public void AddAirplane(Airplane newAirplane)
        {
            AirplanesInTheAir.Add(newAirplane);
        }

        public void AddCheckpoint(double x, double y)
        {
            Checkpoint temp = new AddCheckpoint(double x, double y);
            AvailableCheckpoints.Add(temp);
        }

        public void RemoveCheckpoint(double x, double y)
        {
            foreach (Checkpoint c in AvailableCheckpoints)
            {
                if (c.x == x && c.y == y)
                {
                    AvailableCheckpoints.Remove(c);
                }
            }
        }

        public void ChangeWeatherConditions(WeatherConditions newWeatherConditions)
        {
            WeatherConditions = newWeatherConditions;
        }




    }
}
