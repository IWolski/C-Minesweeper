//-----------------------------------------------------------------------
// <copyright file="GenerateSpread.cs" company="Minesweeper Group">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenerateSpread" /> class.
    /// </summary>
    public class GenerateSpread
    {
        /// <summary>
        /// Generates spread.
        /// </summary>
        /// <param name="row">Parameter determining row number.</param>
        /// <param name="col">Parameter determining column number.</param>
        /// <param name="filledBoard">Parameter determining filledBoard array.</param>
        /// <param name="rowSize">Parameter determining row size.</param>
        /// <param name="colSize">Parameter determining column size.</param>
        public void Spread(int row, int col, int[,] filledBoard, int rowSize, int colSize)
        {
            // Method calls to check each adjacent space of the clicked space.
            this.AdjacentCheck(filledBoard, row - 1, col - 1, rowSize, colSize);
            this.AdjacentCheck(filledBoard, row, col - 1, rowSize, colSize);
            this.AdjacentCheck(filledBoard, row + 1, col - 1, rowSize, colSize);
            this.AdjacentCheck(filledBoard, row - 1, col, rowSize, colSize);
            this.AdjacentCheck(filledBoard, row + 1, col, rowSize, colSize);
            this.AdjacentCheck(filledBoard, row - 1, col + 1, rowSize, colSize);
            this.AdjacentCheck(filledBoard, row, col + 1, rowSize, colSize);
            this.AdjacentCheck(filledBoard, row + 1, col + 1, rowSize, colSize);
        }

        /// <summary>
        /// Checks for adjacent spaces in grid.
        /// </summary>
        /// <param name="filledBoard">Parameter determining filledBoard array.</param>
        /// <param name="row">Parameter determining row number.</param>
        /// <param name="col">>Parameter determining column number.</param>
        /// <param name="rowSize">Parameter determining row size.</param>
        /// <param name="colSize">Parameter determining column size.</param>
        public void AdjacentCheck(int[,] filledBoard, int row, int col, int rowSize, int colSize)
        {
            if (row != -1 && col != -1 && row < rowSize && col < colSize)
            {
                if (filledBoard[row, col] == 0)
                {
                    filledBoard[row, col] = 10;
                }
                else if (filledBoard[row, col] != 9 && filledBoard[row, col] != 10)
                {
                    filledBoard[row, col] = 11;
                }
            }
        }
    }
}