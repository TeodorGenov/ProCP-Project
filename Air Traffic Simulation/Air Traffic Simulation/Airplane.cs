using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        public override double CoordinateX { get; set; }
        public override double CoordinateY { get; set; }
        public double speed { get; private set; }
        public List<AbstractCheckpoint> Route { get; private set; }
        public string FlightNumber { get; private set; }
        public override int MaxSpeed { get; }
        public override int MinSpeed { get; }
        public override int MaxAltitude { get; }
        public override int MinAltitude { get; }

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

            //yeah.. reconsider that.
            MaxSpeed = 2;
            MaxAltitude = 6500;
        }

        private bool first = true;
        private double leapx = 0;
        private double leapy = 0;
        private LinkedListNode<AbstractCheckpoint> target;

        public void MoveTowardsNextPoint()
        {
            //if the path is plane -> a -> b -> c -> strip and a is removed, the plane will "get lost" 
            //bc it doesnt have b in its reachable states; b can be added to the reachable states and a - removed,
            //but what if a recalculation needs to be done? -- no what happens; the path just becomes plane -> b - > c -> strip
            //TODO: force a recalculation of route on every airplane upon addition or removal of a checkpoint
            //somehow after passing through checkpoint a, every checkpoint of type b has to be added to the reachable states of the
            //airplane ..which will f things up if the weather disables checkpoint a before reaching it..
            //think about this later..

            if (Math.Abs(CoordinateX - ShortestPath.Last.Value.CoordinateX) < 0.5 &&
                Math.Abs(CoordinateY - ShortestPath.Last.Value.CoordinateY) < 0.5)
            {
                return;
            }

            if (target == null)
            {
                target = ShortestPath.First;
                target = target.Next;
            }

            if (first)
            {
                Console.WriteLine($"x: {CalculateTimeBetweenPoints(target.Value)} Y: {CalculateTimeBetweenPoints(target.Value)}");
                leapx = (target.Value.CoordinateX - CoordinateX) / CalculateTimeBetweenPoints(target.Value);
                leapy = (target.Value.CoordinateY - CoordinateY) / CalculateTimeBetweenPoints(target.Value);
                first = false;
            }

            if (CoordinateX > target.Value.CoordinateX) // && CoordinateY == target.Value.CoordinateY)
            {
                target = target.Next;
                first = true;
            }

            CoordinateX += leapx;
            CoordinateY += leapy;
        }

        public void calculateShortestPath(List<Checkpoint> points)
        {
            this.ReachableNodes.Clear();
            this.ShortestPath.Clear();

            foreach (Checkpoint point in points)
            {
                if (point.ParentCellType == CellType.UPPER ||
                    point.ParentCellType == CellType.UNASSIGNED)
                {
                    this.AddSingleDestination(point, CalculateTimeBetweenPoints(point));
                    point.AddSingleDestination(this, CalculateTimeBetweenPoints(this));
                }
            }

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
                        var shortestPath = CalculateMinDistance(reachableCheckpoint, edgeWeight, currentCheckpnt);
                        if (shortestPath != null && reachableCheckpoint.GetType() == typeof(Airstrip))
                        {
                            shortestPath.AddLast(reachableCheckpoint);
                            this.ShortestPath = shortestPath;
                        }

                        unsettledCheckpoints.Add(reachableCheckpoint);
                    }
                }

                settledCheckpoints.Add(currentCheckpnt);
            }
        }
    }
}