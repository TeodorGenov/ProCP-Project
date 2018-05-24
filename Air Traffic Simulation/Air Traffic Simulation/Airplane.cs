using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Air_Traffic_Simulation
{
    public class Airplane : AbstractCheckpoint
    {
        public override string Name { get; }
        public override LinkedList<AbstractCheckpoint> ShortestPath { get; set; }
        public override double DistanceFromSource { get; set; }
        public override Dictionary<AbstractCheckpoint, double> ReachableNodes { get; set; }
        public override double CoordinateX { get; set; }
        public override double CoordinateY { get; set; }
        public List<AbstractCheckpoint> Route { get; private set; }
        public string FlightNumber { get; private set; }
        public override int MaxSpeed { get; }
        public override int MinSpeed { get; }
        public override int MaxAltitude { get; }
        public override int MinAltitude { get; }

        private double speed;

        public double Speed
        {
            get { return speed; }
            private set
            {
                ktsPerSecond = value / 360;
                speed = value;
            }
        }

        public event EventHandler OnAirportReached;

        /// <summary>
        /// The airplane's speed for knots per second. Used for calculation of movement.
        /// </summary>
        private double ktsPerSecond;

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

            this.ktsPerSecond = speed / 360;
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

            if (Math.Abs(CoordinateX - ShortestPath.Last.Value.CoordinateX) < Cell.Width &&
                Math.Abs(CoordinateY - ShortestPath.Last.Value.CoordinateY) < Cell.Width &&
                OnAirportReached != null)
            {
                OnAirportReached(this, EventArgs.Empty);
                return;
            }

            if (target == null)
            {
                target = ShortestPath.First;
                target = target.Next;
            }

            if (first)
            {
                double a = (target.Value.CoordinateY - CoordinateY) / Grid.PixelsPerMileVertically;
                double b = (target.Value.CoordinateX - CoordinateX) / Grid.PixelsPerMileHorizontally;
                double c = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2)); //the distance the plane has to fly in miles

                double t = c / ktsPerSecond; //the time which the plane will need to fly this distance

                leapx = (b / t) * Grid
                            .PixelsPerMileHorizontally; //the x speed of the airplane in miles times pixels per mile 
                leapy = (a / t) * Grid.PixelsPerMileVertically; //the y speed of the airplane in miles

                first = false;
            }

            if (Math.Abs(CoordinateX - target.Value.CoordinateX) < Cell.Width &&
                Math.Abs(CoordinateY - target.Value.CoordinateY) < Cell.Width)
            {
                target = target.Next;
                if (target != null)
                    speed = target.Value.MaxSpeed;

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