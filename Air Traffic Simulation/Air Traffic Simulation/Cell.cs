using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Air_Traffic_Simulation
{
    class Cell
    {
        public int id;
        public int x, y;

        /// <summary>
        /// The side of the cell, width = height.
        /// </summary>
        public static int Width = 15;

        /// <summary>
        /// Marks the type of cell. Depending on this, min and max speeds and altitudes will be defined.
        /// </summary>
        public CellType Type { get; set; }

        public Cell(int id, int x, int y)
        {
            this.id = id;
            this.x = x;
            this.y = y;
        }

        public bool ContainsPoint(int xmouse, int ymouse)
        {
            //return (this.x - xmouse) * (this.x - xmouse) + (this.y - ymouse) * (this.y - ymouse) <= Width * Width;
            if (xmouse < x+Width && ymouse < y + Width && xmouse >= x && ymouse >= y)
            {
                return true;
            }
            return false;
        }

        public Point GetCenter()
        {
            Point p = new Point(x + (Width/2), y + (Width/2));
            return p;
        }
    }
}
