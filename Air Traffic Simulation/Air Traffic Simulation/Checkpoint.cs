using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air_Traffic_Simulation
{
    [Serializable]
    class Checkpoint : AbstractCheckpoint
    {
        public override string Name { get; }
        public override double CoordinateX { get; }
        public override double CoordinateY { get; }
        public override LinkedList<AbstractCheckpoint> ShortestPath { get; set; }
        public override double DistanceFromSource { get; set; }
        public override Dictionary<AbstractCheckpoint, double> ReachableNodes { get; set; }

        public Checkpoint(string name, double coordinateX, double coordinateY)
        {
            this.Name = name;
            this.CoordinateX = coordinateX;
            this.CoordinateY = coordinateY;

            ShortestPath = new LinkedList<AbstractCheckpoint>();
            DistanceFromSource = Int32.MaxValue;
            ReachableNodes = new Dictionary<AbstractCheckpoint, double>();
        }

    }
}
