using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Air_Traffic_Simulation
{
    /// <summary>
    /// A Checkpoint is a point on our <see cref="Grid"/>, in the vicinity of which
    /// an <see cref="Airplane"/> needs to fly in order to get from its initial location
    /// to its final destination (either an <see cref="Airstrip"/>, or a point at the
    /// border of the <see cref="Grid"/>).
    /// 
    /// These checkpoints are used both for purely showing the path the <see cref="Airplane"/>
    /// will ahve to follow and for assigning the speed and altitude at which this should be done.
    /// </summary>
    [Serializable]
    public class Checkpoint : AbstractCheckpoint
    {
        /// <summary>
        /// The name of the <see cref="T:Air_Traffic_Simulation.Checkpoint" />.
        /// </summary>
        public override string Name { get; }

        /// <summary>
        /// The horizontal coordinates of the <see cref="Checkpoint"/>. (Upper left corner)
        /// </summary>
        public override double CoordinateX { get; set; }
        public override double CoordinateY { get; set; }
        public override LinkedList<AbstractCheckpoint> ShortestPath { get; set; }
        public override double DistanceFromSource { get; set; }
        public override Dictionary<AbstractCheckpoint, double> ReachableNodes { get; set; }
        public override int MaxSpeed { get; }
        public override int MinSpeed { get; }
        public override int MaxAltitude { get; }
        public override int MinAltitude { get; }

        /// <summary>
        /// The type of cell the checkpoint is situated in.
        /// </summary>
        public CellType ParentCellType { get; }

        public event EventHandler OnWeatherPassing;

        public Checkpoint(string name, double coordinateX, double coordinateY, Cell c, List<Checkpoint> allCheckpoints, Airstrip strip, List<Checkpoint> exitCheckpoints)
        {
            this.Name = name;
            this.CoordinateX = coordinateX;
            this.CoordinateY = coordinateY;

            ShortestPath = new LinkedList<AbstractCheckpoint>();
            DistanceFromSource = Int32.MaxValue;
            ReachableNodes = new Dictionary<AbstractCheckpoint, double>();

            ParentCellType = c.Type;

            switch (c.Type)
            {
                case CellType.BORDER:
                    MinSpeed = 500;
                    MaxSpeed = 800;
                    MaxAltitude = 7500;
                    MinAltitude = 6500;
                    break;
                case CellType.UNASSIGNED:
                    MinSpeed = 330;
                    MaxSpeed = Int32.MaxValue;
                    MaxAltitude = 8000;
                    MinAltitude = 6100;
                    break;
                case CellType.UPPER:
                    MinSpeed = 310;
                    MaxSpeed = 330;
                    MinAltitude = 5800;
                    MaxAltitude = 6100;
                    break;
                case CellType.MIDDLE:
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

            AddReachables(allCheckpoints, strip, exitCheckpoints);
        }

        public void AddReachables(List<Checkpoint> allCheckpoints, Airstrip strip, List<Checkpoint> exitCheckpoints)
        {
            switch (ParentCellType)
            {
                case CellType.BORDER:
                case CellType.UNASSIGNED:
                    foreach (Checkpoint point in allCheckpoints)
                    {
                        if (point.ParentCellType == CellType.BORDER ||
                            point.ParentCellType == CellType.UNASSIGNED ||
                            point.ParentCellType == CellType.UPPER)
                        {
                            this.AddSingleDestination(point, CalculateDistanceBetweenPoints(point));
                            point.AddSingleDestination(this, CalculateDistanceBetweenPoints(this));
                        }
                    }

                    break;
                case CellType.UPPER:
                    foreach (Checkpoint point in allCheckpoints.Concat(exitCheckpoints))
                    {
                        if (point.ParentCellType == CellType.BORDER ||
                            point.ParentCellType == CellType.UNASSIGNED ||
                            point.ParentCellType == CellType.UPPER ||
                            point.ParentCellType == CellType.MIDDLE)
                        {
                            this.AddSingleDestination(point, CalculateDistanceBetweenPoints(point));
                            point.AddSingleDestination(this, CalculateDistanceBetweenPoints(this));
                        }
                    }

                    break;
                case CellType.MIDDLE:
                    foreach (Checkpoint point in allCheckpoints)
                    {
                        if (point.ParentCellType == CellType.UPPER ||
                            point.ParentCellType == CellType.MIDDLE ||
                            point.ParentCellType == CellType.LOWER)
                        {
                            this.AddSingleDestination(point, CalculateDistanceBetweenPoints(point));
                            point.AddSingleDestination(this, CalculateDistanceBetweenPoints(this));
                        }
                    }

                    break;
                case CellType.LOWER:
                    foreach (Checkpoint point in allCheckpoints)
                    {
                        if (point.ParentCellType == CellType.MIDDLE ||
                            point.ParentCellType == CellType.LOWER ||
                            point.ParentCellType == CellType.FINAL)
                        {
                            this.AddSingleDestination(point, CalculateDistanceBetweenPoints(point));
                            point.AddSingleDestination(this, CalculateDistanceBetweenPoints(this));
                        }
                    }

                    break;
                case CellType.FINAL:

                    foreach (Checkpoint point in allCheckpoints)
                    {
                        if (point.ParentCellType == CellType.LOWER ||
                            point.ParentCellType == CellType.FINAL)
                        {
                            this.AddSingleDestination(point, CalculateDistanceBetweenPoints(point));
                            point.AddSingleDestination(this, CalculateDistanceBetweenPoints(this));
                        }
                    }

                    try
                    {
                        this.AddSingleDestination(strip, CalculateDistanceBetweenPoints(strip));
                        strip.AddSingleDestination(this, CalculateDistanceBetweenPoints(this));
                    }
                    catch (System.NullReferenceException e)
                    {
                        MessageBox.Show(
                            $"A strip needs to be added before{Environment.NewLine}adding a checkpoint in the final zone.",
                            "No strip found.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                    break;
                default:
                    break;
            }
        }
    }
}