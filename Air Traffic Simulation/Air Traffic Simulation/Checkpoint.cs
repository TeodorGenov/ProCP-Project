using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air_Traffic_Simulation
{
    class Checkpoint : ICheckpoint
    {
        private string name;
        private double coordinateX;
        private double coordinateY;

        public double CoordinateX()
        {
            
        }

        public double CoordinateY()
        {
            throw new NotImplementedException();
        }

        public string Name()
        {
            throw new NotImplementedException();
        }
    }
}
