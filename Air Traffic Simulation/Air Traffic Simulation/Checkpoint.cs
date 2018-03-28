using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air_Traffic_Simulation
{
    class Checkpoint : AbstractCheckpoint
    {
        public override string Name { get; }
        public override double CoordinateX { get; }
        public override double CoordinateY { get; }

        public Checkpoint(string name, double coordinateX, double coordinateY)
        {
            this.Name = name;
            this.CoordinateX = coordinateX;
            this.CoordinateY = coordinateY;
        }

    }
}
