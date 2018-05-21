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
        public override int MaxSpeed { get; }
        public override int MinSpeed { get; }
        public override int MaxAltitude { get; }
        public override int MinAltitude { get; }

        public Checkpoint(string name, double coordinateX, double coordinateY, Cell c)
        {
            this.Name = name;
            this.CoordinateX = coordinateX;
            this.CoordinateY = coordinateY;

            ShortestPath = new LinkedList<AbstractCheckpoint>();
            DistanceFromSource = Int32.MaxValue;
            ReachableNodes = new Dictionary<AbstractCheckpoint, double>();

            switch (c.Type)
            {
                case CellType.BORDER:
                case CellType.UNASSIGNED:
                    MinSpeed = 330;
                    MaxSpeed = Int32.MaxValue;
                    MaxAltitude = 8000;
                    MinAltitude = 5800;
                    break;
                case CellType.UPPER:
                    MinSpeed = 310;
                    MaxSpeed = 330;
                    MinAltitude = 5800;
                    MaxAltitude = 6100;
                    break;
                case CellType.MID:
                    MinSpeed = 170;
                    MaxSpeed = 190;
                    MinAltitude = 2800;
                    MaxAltitude = 3100;
                    break;
                case CellType.LOWER:
                    MinSpeed = 150;
                    MaxSpeed = 170;
                    MinAltitude = 1200;
                    MaxAltitude = 1500;
                    break;
                case CellType.FINAL:
                    MinSpeed = 120;
                    MaxSpeed = 150;
                    MinAltitude = 500;
                    MaxAltitude = 300;
                    break;
                default:
                    break;
            }
        }
    }
}