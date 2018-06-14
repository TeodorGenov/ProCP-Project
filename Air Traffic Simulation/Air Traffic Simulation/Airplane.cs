using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Air_Traffic_Simulation
{
    [Serializable]
    public class Airplane : AbstractCheckpoint
    {
        public override string Name { get; }
        [field: NonSerialized]
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
        public int Area { get; set; }
        public Rectangle Rect { get; set; }

        public double Speed
        {
            get { return speed; }
            private set
            {
                ktsPerSecond = value / 360;
                speed = value;
            }
        }

        [field: NonSerialized]
        public event EventHandler OnAirportReached;
        [field: NonSerialized]
        public event EventHandler OnAirspaceExit;
        [field: NonSerialized]
        public delegate void CrashHandler(Object p1, Object p2);
        [field: NonSerialized]
        public event CrashHandler OnCrash;

        /// <summary>
        /// Shows if the plane is on its way to land.
        /// </summary>
        public bool IsLanding { get; set; }

        /// <summary>
        /// The point through which the airplane will leave the airspace.
        /// </summary>
        public Checkpoint exitDestination { get; set; }

        /// <summary>
        /// The airplane's speed for knots per second. Used for calculation of movement.
        /// </summary>
        private double ktsPerSecond;

        public Airplane(string name, double coordinateX, double coordinateY, double speed, string flightNumber)
        {
            IsLanding = true;
            Name = name;
            this.CoordinateX = coordinateX;
            this.CoordinateY = coordinateY;
            this.speed = speed;
            FlightNumber = flightNumber;
            Area = 20;
            Rect = new Rectangle((int) CoordinateX - Area, (int) CoordinateY - Area, Area * 2, Area * 2);

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
        [field: NonSerialized]
        public LinkedListNode<AbstractCheckpoint> target;

        /// <summary>
        /// Checks if the distance between two airplanes is safe or not. If the distance is not safe, the OnCrash event triggers
        /// </summary>
        /// <param name="p"></param>
        public void DangerCheck(Airplane p)
        {
            if (OnCrash != null)
                if (Math.Sqrt(Math.Pow(CoordinateX - p.CoordinateX, 2) + Math.Pow(CoordinateY - p.CoordinateY, 2)) <=
                    Area * 2)
                    OnCrash(this, p);
        }

        public void MoveTowardsNextPoint()
        {
            //if the path is plane -> a -> b -> c -> strip and a is removed, the plane will "get lost" 
            //bc it doesnt have b in its reachable states; b can be added to the reachable states and a - removed,
            //but what if a recalculation needs to be done? -- no what happens; the path just becomes plane -> b - > c -> strip
            //TODO: force a recalculation of route on every airplane upon addition or removal of a checkpoint
            //somehow after passing through checkpoint a, every checkpoint of type b has to be added to the reachable states of the
            //airplane ..which will f things up if the weather disables checkpoint a before reaching it..
            //think about this later..

            if (target == null)
            {
                target = ShortestPath.First;
                target = target.Next;
            }

            if (Math.Abs(CoordinateX - ShortestPath.Last.Value.CoordinateX) < Cell.Width * 3 &&
                Math.Abs(CoordinateY - ShortestPath.Last.Value.CoordinateY) < Cell.Width * 3 &&
                target.Value.GetType() == typeof(Airstrip) &&
                OnAirportReached != null)
            {
                target = null;
                OnAirportReached(this, EventArgs.Empty);
                return;
            }
            else if (Math.Abs(CoordinateX - ShortestPath.Last.Value.CoordinateX) < Cell.Width * 3 &&
                     Math.Abs(CoordinateY - ShortestPath.Last.Value.CoordinateY) < Cell.Width * 3 &&
                     target.Value.GetType() == typeof(Checkpoint) && OnAirspaceExit != null)
            {
                OnAirspaceExit(this, EventArgs.Empty);
                return;
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

            //if the airplane goes through a checkpoint
            if (Math.Abs(CoordinateX - target.Value.CoordinateX) < Cell.Width * 3 &&
                Math.Abs(CoordinateY - target.Value.CoordinateY) < Cell.Width * 3)
            {
                var toRemove = target;
                ShortestPath.Remove(toRemove.Value);
                target = target.Next;

                if (target != null)
                    speed = target.Value.MaxSpeed;

                first = true;
            }

            CoordinateX += leapx;
            CoordinateY += leapy;
            this.Rect = new Rectangle((int) CoordinateX - Area, (int) CoordinateY - Area, Area * 2, Area * 2);
        }


        /// <summary>
        /// Calculates the shortest path between the airplane and its final destination - the airstrip.
        /// </summary>
        /// <param name="points">All the checkpoints in the airspace. No airstrips, no airplanes.</param>
        /// <param name="landingStrip">The landing strip the airplane is going for.</param>
        public void CalculateShortestPathToAirstrip(List<Checkpoint> points, Airstrip landingStrip)
        {
            this.ReachableNodes.Clear();

            this.ShortestPath.Clear();

            foreach (Checkpoint point in points)
            {
                if (target == null && (point.ParentCellType == CellType.UPPER ||
                                       point.ParentCellType == CellType.UNASSIGNED))
                {
                    this.AddSingleDestination(point, CalculateTimeBetweenPoints(point));
                }
                else if (target != null && target.GetType() == typeof(Airstrip))
                {
                    ShortestPath.Clear();
                    ShortestPath.AddFirst(target);
                    return;
                }
                else if (target != null && point.ParentCellType == ((Checkpoint) target.Value).ParentCellType)
                {
                    this.AddSingleDestination(point, CalculateTimeBetweenPoints(point));
                }


                point.ShortestPath.Clear();
                point.DistanceFromSource = int.MaxValue;
            }


            landingStrip.DistanceFromSource = int.MaxValue;
            landingStrip.ShortestPath.Clear();

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

                            this.ShortestPath = new LinkedList<AbstractCheckpoint>();
                            var pathNode = shortestPath.First;

                            while (pathNode != null)
                            {
                                this.ShortestPath.AddLast(pathNode.Value);
                                pathNode = pathNode.Next;
                            }
                        }

                        unsettledCheckpoints.Add(reachableCheckpoint);
                    }
                }

                settledCheckpoints.Add(currentCheckpnt);
            }
        }

        public override string ToString()
        {
            return this.Name + " -- " + this.FlightNumber;
        }

        public override bool Equals(object obj)
        {
            var airplane = obj as Airplane;
            return airplane != null &&
                   Name == airplane.Name &&
                   FlightNumber == airplane.FlightNumber;
        }

        public void RevertPath()
        {
            ShortestPath.Reverse();
        }

        /// <summary>
        /// Calculates the shortest path between the airplane and the point through which it has to leave the airspace.
        /// </summary>
        /// <param name="points">All the checkpoints in the airspace. No airstrips, no airplanes.</param>
        /// <param name="exitCheckpoint">The checkpoint through which the plane has to leave the airspace.</param>
        public void FindShortestPathLeavingAirspace(List<Checkpoint> points)
        {
            this.ReachableNodes.Clear();
            this.ShortestPath.Clear();

            foreach (Checkpoint point in points)
            {
                if (target == null && point.ParentCellType == CellType.FINAL)
                {
                    this.AddSingleDestination(point, CalculateTimeBetweenPoints(point));
                }
                else if (target != null && target.Equals(ShortestPath.Last)) //if this doesn't work: ((Checkpoint)target.Value).ParentCellType == Cell.BORDER
                {
                    ShortestPath.Clear();
                    ShortestPath.AddFirst(target);
                    return;
                }
                else if (target != null && point.ParentCellType == ((Checkpoint)target.Value).ParentCellType)
                {
                    this.AddSingleDestination(point, CalculateTimeBetweenPoints(point));
                }

                point.ShortestPath.Clear();
                point.DistanceFromSource = int.MaxValue;
            }

            exitDestination.DistanceFromSource = int.MaxValue;
            exitDestination.ShortestPath.Clear();

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
                        if (shortestPath != null && reachableCheckpoint.GetType() == typeof(Checkpoint) &&
                            ((Checkpoint) reachableCheckpoint).ParentCellType == CellType.BORDER)
                        {
                            shortestPath.AddLast(reachableCheckpoint);

                            this.ShortestPath = new LinkedList<AbstractCheckpoint>();
                            var pathNode = shortestPath.First;

                            while (pathNode != null)
                            {
                                this.ShortestPath.AddLast(pathNode.Value);
                                pathNode = pathNode.Next;
                            }
                        }

                        unsettledCheckpoints.Add(reachableCheckpoint);
                    }
                }

                settledCheckpoints.Add(currentCheckpnt);
            }
        }
    }
}