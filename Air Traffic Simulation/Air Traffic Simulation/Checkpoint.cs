using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Air_Traffic_Simulation
{
    [Serializable]
    class Checkpoint : AbstractCheckpoint
    {
        public override string Name { get; }
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

        public Checkpoint(string name, double coordinateX, double coordinateY, Cell c, List<Checkpoint> allCheckpoints, Airstrip strip)
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


            DistanceFromSource = Int32.MaxValue;
            AddReachables(allCheckpoints, strip);
        }

        public void AddReachables(List<Checkpoint> allCheckpoints, Airstrip strip)
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
                    foreach (Checkpoint point in allCheckpoints)
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
                            $"A strip needs to be added before{Environment.NewLine}adding a checkpoint in the final zone.", "No strip found.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                    break;
                default:
                    break;
            }


//            this.AddAllPossibleDestinations(points);
//
//
//            points.Add(strip);
//            foreach (var point in points)
//            {
//                point.DistanceFromSource = Int32.MaxValue;
//                point.AddAllPossibleDestinations(points);
//            }
//
//            points.Remove(strip);
        }
    }
}