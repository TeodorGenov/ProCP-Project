using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air_Traffic_Simulation
{
    class Airplane
    {
        public string Name { get; private set; }
        public double coordinateX { get; private set; }
        public double coordinateY { get; private set; }
        public  double speed { get; private set; }
        public List<AbstractCheckpoint> route { get; private set; }
        public string FlightNumber { get; private set; }

        public Airplane(string name, double coordinateX, double coordinateY, double speed, string flightNumber)
        {
            Name = name;
            this.coordinateX = coordinateX;
            this.coordinateY = coordinateY;
            this.speed = speed;
            FlightNumber = flightNumber;
        }

        public void setRoute()
        {
            throw new NotImplementedException();
        }
    }
}
