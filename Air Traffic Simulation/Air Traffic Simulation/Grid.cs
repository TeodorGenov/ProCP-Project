﻿using System;
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
        private int xs = 0, ys = 0, id = 1;

        /// <summary>
        /// The number of columns the grid is going to have.
        /// </summary>
        private readonly int columnsOfCells;

        /// <summary>
        /// The number of rows the grid is going to have.
        /// </summary>
        private readonly int rowsOfCells;

        /// <summary>
        /// The total number of cells the grid is going to have.
        /// </summary>
        private readonly int totalNumberOfCells;

        /// <summary>
        /// The amount of pixels which corresponds on a nautical mile
        /// per our grid, when we are considering horizontal movement.
        /// </summary>
        public static double PixelsPerMileHorizontally;

        /// <summary>
        /// The amount of pixels which corresponds on a nautical mile
        /// per our grid, when we are considering vertical movement.
        /// </summary>
        public static double PixelsPerMileVertically;

        public Grid(int pictureBoxWidth, int pictureBoxHeight)
        {
            this.columnsOfCells = pictureBoxWidth / Cell.Width;
            this.rowsOfCells = pictureBoxHeight / Cell.Width;
            this.totalNumberOfCells = this.columnsOfCells * this.rowsOfCells;

            //that's the diameter of the space covered between the inner sides
            //of the BORDER cells; our grid is based on real life numbers and in real life
            //this outermost zone starts from 20 miles away from the airport - thus, the total
            //diameter of our airspace is 40 miles
            int diameterOfAirspaceInMiles = 40;

            int diameterOfAirspaceInPixelsHorizontally = (columnsOfCells - 2) * Cell.Width;

            int diameterOfAirspaceInPixelsVertically = (rowsOfCells - 2) * Cell.Width;

            PixelsPerMileHorizontally = (double)diameterOfAirspaceInPixelsHorizontally / diameterOfAirspaceInMiles;
            PixelsPerMileVertically = (double)diameterOfAirspaceInPixelsVertically / diameterOfAirspaceInMiles;

            Console.WriteLine($"diameter: {diameterOfAirspaceInMiles} miles");
            Console.WriteLine($"diameter hor: {diameterOfAirspaceInPixelsHorizontally} pixels");
            Console.WriteLine($"diameter vert: {diameterOfAirspaceInPixelsVertically} pixels");
            Console.WriteLine($"ppm hor: {PixelsPerMileHorizontally} pixels");
            Console.WriteLine($"ppm vert: {PixelsPerMileVertically} pixels");


            //TODO: Remove cw
            Console.WriteLine($"columns: {columnsOfCells} rows: {rowsOfCells}");
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
            Cell c;
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

                assignZoneToCell(c);
            }
        }

        private void assignZoneToCell(Cell c)
        {
            if ((c.id % columnsOfCells >= Math.Floor(0.9 * columnsOfCells / 2)) &&
                (c.id % columnsOfCells <= columnsOfCells - Math.Floor(0.9 * columnsOfCells / 2)) &&
                (c.id > (Math.Floor(0.9 * rowsOfCells / 2) - 1) * columnsOfCells) &&
                (c.id < (rowsOfCells - Math.Floor(0.9 * rowsOfCells / 2)) * columnsOfCells))
            {
                c.Type = CellType.FINAL;
            }
            else if ((c.id % columnsOfCells >= Math.Floor(0.7 * columnsOfCells / 2)) &&
                     (c.id % columnsOfCells <= columnsOfCells - Math.Floor(0.7 * columnsOfCells / 2)) &&
                     (c.id > Math.Floor((0.7 * rowsOfCells / 2) - 1) * columnsOfCells) &&
                     (c.id < (rowsOfCells - Math.Floor(0.7 * rowsOfCells / 2)) * columnsOfCells))
            {
                c.Type = CellType.LOWER;
            }
            else if ((c.id % columnsOfCells >= Math.Floor(0.4 * columnsOfCells / 2)) &&
                     (c.id % columnsOfCells <= columnsOfCells - Math.Floor(0.4 * columnsOfCells / 2)) &&
                     (c.id > Math.Floor((0.4 * rowsOfCells / 2) - 1) * columnsOfCells) &&
                     (c.id < (rowsOfCells - Math.Floor(0.4 * rowsOfCells / 2)) * columnsOfCells))
            {
                c.Type = CellType.MIDDLE;
            }
            else if (c.id <= this.columnsOfCells)
            {
                c.Type = CellType.BORDER;
            }
            else if (c.id % this.columnsOfCells == 1)
            {
                c.Type = CellType.BORDER;
            }
            else if (c.id % this.columnsOfCells == 0)
            {
                c.Type = CellType.BORDER;
            }
            //last row
            else if (c.id > this.columnsOfCells * (this.rowsOfCells - 1))
            {
                c.Type = CellType.BORDER;
            }
            else
            {
                c.Type = CellType.UPPER;
            }

            //                else if ((c.id % columnsOfCells >= Math.Floor(0.1 * columnsOfCells / 2)) &&
            //                         (c.id % columnsOfCells <= columnsOfCells - Math.Floor(0.1 * columnsOfCells / 2)) &&
            //                         (c.id > Math.Floor((0.1 * rowsOfCells / 2) - 1) * columnsOfCells) &&
            //                         (c.id < (rowsOfCells - Math.Floor(0.1 * rowsOfCells / 2)) * columnsOfCells))
            //                {
            //                    c.Type = CellType.UPPER;
            //                }
        }
    }
}