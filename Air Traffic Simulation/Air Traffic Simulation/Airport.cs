using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air_Traffic_Simulation
{
    class Airport
    {
        public string Name { get; set; }
        List<Airstrip> airstrips;
        List<Airplane> planeList;

        public Airport(string airportName)
        {
            this.Name = airportName;
            airstrips = new List<Airstrip>();
            planeList = new List<Airplane>();
        }
        public void AddStrip(Airstrip airstrip)
        {
            airstrips.Add(airstrip);
        }
        public void RemoveStrip(string name)
        {
            int airstripIndex = airstrips.FindIndex(item => item.Name == name);
            airstrips.RemoveAt(airstripIndex);
        }
        public void PlaneLand()
        {
            //ToDo
        }
        public void PlaneTakeOff()
        {//ToDo
            //ToDo
        }
    }
}
