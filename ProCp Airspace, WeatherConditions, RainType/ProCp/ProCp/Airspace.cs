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
        { get; set}

        public Checkpoint[] AvailableCheckpoints
        { get; set; }

        public Airplane[] AirplanesInTheAir
        { get; set; }

        public WeatherConditions WeatherConditions
        { get; set; }

        public Airspace(Airport airport, Checkpoint[] availableCheckpoints, Airplane[] airplanesInTheAir, WeatherConditions weatherConditions)
        {
            Airport = airport;
            AvailableCheckpoints = availableCheckpoints;
            AirplanesInTheAir = airplanesInTheAir;
            WeatherConditions = weatherConditions;
        }

        public void AddAirplane()
        {

        }

        public void AddCheckpoint(double x, double y)
        {

        }

        public void RemoveCheckpoint(double x, double y)
        {

        }

        public void ChangeWeatherConditions(WeatherConditions newWeatherConditions)
        {
            WeatherConditions = newWeatherConditions;
        }




    }
}
