using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air_Traffic_Simulation
{
    class Airstrip : AbstractCheckpoint
    {
        private double _takeOffDirection;
        public override string Name { get; }
        public override double CoordinateX { get; }
        public override double CoordinateY { get; }

        public bool IsFree { get; set; }

        public double TakeOffDirection
        {
            get { return _takeOffDirection; }

            private set
            {
                if (value >= 360)
                {
                    _takeOffDirection = value - 360;
                }
                else
                {
                    _takeOffDirection = value;
                }
            }
        }

        public Airstrip(string name, double coordinateX, double coordinateY, bool isFree, double takeOffDirection)
        {
            Name = name;
            CoordinateX = coordinateX;
            CoordinateY = coordinateY;
            this.IsFree = isFree;
            this._takeOffDirection = takeOffDirection;
        }


        /// <summary>
        /// Changes the take off/landing direction to the opposite.
        /// </summary>
        public void SwitchDirections()
        {
            TakeOffDirection += 180;
        }

        //TODO: SetStatus() method in Airstrip class
        public void SetStatus()
        {
            throw new NotImplementedException();
        }
    }
}