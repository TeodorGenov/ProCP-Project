using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Air_Traffic_Simulation
{
    class Grid
    {
        public List<Cell> listOfCells = new List<Cell>();
        private Cell c;
        private int xs = 0, ys = 0, id = 1;

        public Cell GetCell(int xmouse, int ymouse)
        {
            foreach(Cell c in this.listOfCells)
            {
                if (c.x == xmouse && c.y == ymouse)
                { return c; }
            }
            return null;
        }

        public void MakeGrid()
        {

            for(int i = 1; i <= 920; i++)
            {
                if(i % 40 != 0)
                {
                    c = new Cell(id, xs, ys);
                    listOfCells.Add(c);
                    xs = xs + c.width;
                    id = id + 1;
                }
                else
                {
                    c = new Cell(id, xs, ys);
                    listOfCells.Add(c);
                    xs = 0;
                    ys = ys + c.width;
                    id = id + 1;
                }
            }
        }
    }
}
