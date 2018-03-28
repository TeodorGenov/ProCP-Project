using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air_Traffic_Simulation
{
    [Serializable]
    abstract class AbstractCheckpoint
    {
        public abstract string Name { get; }
        public abstract double CoordinateX { get; }
        public abstract double CoordinateY { get; }

        /// <summary>
        /// Uses Pythagoras' theorem to calculate the distance between two checkpoints.
        /// </summary>
        /// <param name="a">The first checkpoint needed for the comparison.</param>
        /// <param name="b">The second checkpoint needed for the comparison.</param>
        /// <returns>The distance between the two checkpoints given as arguments.</returns>
        static double CalculateDistanceBetweenPoints(AbstractCheckpoint a, AbstractCheckpoint b)
        {
            return Math.Sqrt(Math.Pow(Math.Abs(a.CoordinateX - b.CoordinateX), 2) + Math.Pow(Math.Abs(a.CoordinateY - b.CoordinateY), 2));
        }
    }
}
