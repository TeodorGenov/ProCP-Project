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

        /// <summary>
        /// The number of columns the grid is going to have.
        /// </summary>
        private int columnsOfCells;

        /// <summary>
        /// The number of rows the grid is going to have.
        /// </summary>
        private int rowsOfCells;

        /// <summary>
        /// The total number of cells the grid is going to have.
        /// </summary>
        private int totalNumberOfCells;

        public Grid(int pictureBoxWidth, int pictureBoxHeight)
        {
            this.columnsOfCells = pictureBoxHeight / Cell.Width;
            this.rowsOfCells = pictureBoxWidth / Cell.Width;
            this.totalNumberOfCells = this.columnsOfCells * this.rowsOfCells;
            Console.WriteLine($"columns: {columnsOfCells}{Environment.NewLine}rows: {rowsOfCells}{Environment.NewLine}total: {totalNumberOfCells}");
        }

        public Cell GetCell(int xmouse, int ymouse)
        {
            foreach (Cell c in this.listOfCells)
            {
                if (c.x == xmouse && c.y == ymouse)
                {
                    return c;
                }
            }

            return null;
        }

        public void MakeGrid()
        {
            for (int i = 1; i <= totalNumberOfCells; i++)
            {
                if (i % columnsOfCells != 0)
                {
                    c = new Cell(id, xs, ys);
                    listOfCells.Add(c);
                    xs = xs + Cell.Width;
                    id = id + 1;
                }
                else
                {
                    c = new Cell(id, xs, ys);
                    listOfCells.Add(c);
                    xs = 0;
                    ys = ys + Cell.Width;
                    id = id + 1;
                }
            }
        }
    }
}