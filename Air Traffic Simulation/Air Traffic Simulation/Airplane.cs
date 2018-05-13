using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air_Traffic_Simulation
{
    class Airplane : AbstractCheckpoint
    {
        public override string Name { get; }
        public override LinkedList<AbstractCheckpoint> ShortestPath { get; set; }
        public override double DistanceFromSource { get; set; }
        public override Dictionary<AbstractCheckpoint, double> ReachableNodes { get; set; }
        public override double CoordinateX { get; }
        public override double CoordinateY { get; }
        public double speed { get; private set; }
        public List<AbstractCheckpoint> Route { get; private set; }
        public string FlightNumber { get; private set; }

        public Airplane(string name, double coordinateX, double coordinateY, double speed, string flightNumber)
        {
            Name = name;
            this.CoordinateX = coordinateX;
            this.CoordinateY = coordinateY;
            this.speed = speed;
            FlightNumber = flightNumber;

            ShortestPath = new LinkedList<AbstractCheckpoint>();
            DistanceFromSource = 0;
            ReachableNodes = new Dictionary<AbstractCheckpoint, double>();
            Route = new List<AbstractCheckpoint>();
        }

        public void setRoute()
        {
            throw new NotImplementedException();
        }

        public void calculateShortestPath(List<AbstractCheckpoint> points, Airstrip strip)
        {
            this.ReachableNodes.Clear();
            this.ShortestPath.Clear();

            this.AddAllPossibleDestinations(points);

            points.Add(strip);
            foreach (var point in points)
            {
                point.ShortestPath.Clear();
                point.ReachableNodes.Clear();
                point.DistanceFromSource = Int32.MaxValue;
                point.AddAllPossibleDestinations(points);
            }

            points.Remove(strip);

            HashSet<AbstractCheckpoint> settledCheckpoints = new HashSet<AbstractCheckpoint>();
            HashSet<AbstractCheckpoint> unsettledCheckpoints = new HashSet<AbstractCheckpoint> {this};


            while (unsettledCheckpoints.Count != 0)
            {
                AbstractCheckpoint currentCheckpnt = this.GetLowestDistanceNode(unsettledCheckpoints);
                unsettledCheckpoints.Remove(currentCheckpnt);
                foreach (var pair in currentCheckpnt.ReachableNodes)
                {
                    AbstractCheckpoint reachableCheckpoint = pair.Key;
                    double edgeWeight = pair.Value;

                    if (!settledCheckpoints.Contains(reachableCheckpoint))
                    {
                        CalculateMinDistance(reachableCheckpoint, edgeWeight, currentCheckpnt);
                        unsettledCheckpoints.Add(reachableCheckpoint);
                    }
                }

                settledCheckpoints.Add(currentCheckpnt);
            }
        }
    }
}