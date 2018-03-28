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
        public int width;

        public Cell(int id, int x, int y)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.width = 20;
        }

        public bool ContainsPoint(int xmouse, int ymouse)
        {
            return (this.x - xmouse) * (this.x - xmouse) + (this.y - ymouse) * (this.y - ymouse) <= width * width;
        }
    }
}
