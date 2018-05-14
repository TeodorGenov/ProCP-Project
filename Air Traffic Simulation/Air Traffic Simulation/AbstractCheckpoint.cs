using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Air_Traffic_Simulation
{
    [Serializable]
    abstract class AbstractCheckpoint
    {
        public abstract string Name { get; }
        public abstract double CoordinateX { get; }
        public abstract double CoordinateY { get; }

        public abstract LinkedList<AbstractCheckpoint> ShortestPath { get; set; }
        public abstract double DistanceFromSource { get; set; }
        public abstract Dictionary<AbstractCheckpoint, double> ReachableNodes { get; set; }

        //TODO: fix the mess with the inheritance and the min/max speed/altitude you are about to create

        public abstract int MaxSpeed { get; }
        public abstract int MinSpeed { get; }
        public abstract int MaxAltitude { get; }
        public abstract int MinAltitude { get; }

        /// <summary>
        /// Uses Pythagoras' theorem to calculate the distance between two checkpoints.
        /// </summary>
        /// <param name="a">The checkpoint we are looking for the distance from.</param>
        /// <returns>The distance between the two checkpoints given as arguments.</returns>
        protected virtual double CalculateDistanceBetweenPoints(AbstractCheckpoint a)
        {
            return Math.Sqrt(Math.Pow(Math.Abs(this.CoordinateX - a.CoordinateX), 2) +
                             Math.Pow(Math.Abs(this.CoordinateY - a.CoordinateY), 2));
        }

        protected virtual double CalculateTimeBetweenPoints(AbstractCheckpoint a)
        {
            return CalculateDistanceBetweenPoints(a) / this.MaxSpeed;
        }

        public virtual void AddAllPossibleDestinations(List<AbstractCheckpoint> checkpoints)
        {
            foreach (var point in checkpoints)
            {
                ///TODO: update names
                //this.AddSingleDestination(point, CalculateDistanceBetweenPoints(point));

                this.AddSingleDestination(point, CalculateTimeBetweenPoints(point));
            }
        }

        public virtual void AddSingleDestination(AbstractCheckpoint destination, double distance)
        {
            if (this.CoordinateX != destination.CoordinateX || this.CoordinateY != destination.CoordinateY ||
                !this.Name.Equals(destination.Name))
            {
                ReachableNodes.Add(destination, distance);
            }
        }


        protected virtual AbstractCheckpoint GetLowestDistanceNode(HashSet<AbstractCheckpoint> unsettledCheckpoints)
        {
            AbstractCheckpoint lowestDistanceCP = null;
            double lowestDist = Int32.MaxValue;

            foreach (var checkpnt in unsettledCheckpoints)
            {
                double checkpntDistance = checkpnt.DistanceFromSource;

                if (checkpntDistance < lowestDist)
                {
                    lowestDist = checkpntDistance;
                    lowestDistanceCP = checkpnt;
                }
            }

            return lowestDistanceCP;
        }


        protected virtual void CalculateMinDistance(AbstractCheckpoint evaluationCheckpoint, double edgeWeight,
            AbstractCheckpoint sourceCheckpoint)
        {
            double sourceDisntace = sourceCheckpoint.DistanceFromSource;

            if (sourceDisntace + edgeWeight < evaluationCheckpoint.DistanceFromSource)
            {
                evaluationCheckpoint.DistanceFromSource = sourceDisntace + edgeWeight;
                LinkedList<AbstractCheckpoint> shortestPath =
                    new LinkedList<AbstractCheckpoint>(sourceCheckpoint.ShortestPath);
                shortestPath.AddLast(sourceCheckpoint);
                evaluationCheckpoint.ShortestPath = shortestPath;
            }
        }

        public override string ToString()
        {
            return $"{this.Name} ({this.CoordinateX}, {this.CoordinateY})";
        }
    }
}