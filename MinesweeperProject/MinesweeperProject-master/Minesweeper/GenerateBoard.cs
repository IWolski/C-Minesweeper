//-----------------------------------------------------------------------
// <copyright file="GenerateBoard.cs" company="Minesweeper Group">
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
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenerateBoard" /> class.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Encapsulation not taught.")]
    public class GenerateBoard
    {
        /// <summary>
        /// Create new reset button instance.
        /// </summary>
        public Button ResetButton = new Button();

        /// <summary>
        /// Create a new exit button instance.        
        /// </summary>
        public Button ExitButton = new Button();

        /// <summary>
        /// Create new mines left label instance.
        /// </summary>
        public TextBlock MinesLeft = new TextBlock();

        /// <summary>
        /// Create new timer text block instance.
        /// </summary>
        public TextBlock TimerText = new TextBlock();

        /// <summary>
        /// Create a new grid instance.        
        /// </summary>
        public Grid Grid = new Grid();

        /// <summary>
        /// Generates form.
        /// </summary>
        /// <param name="row">Row count</param>
        /// <param name="col">Column count</param>
        /// <param name="w">Width of form</param>
        /// <param name="h">Height of form</param>
        /// <param name="cellColor">Cell color</param>
        /// <param name="numberColor">Number color</param>
        public void GenerateForm(int row, int col, int w, int h, string cellColor, string numberColor)
        {
            // Creates new color variable.
            SolidColorBrush buttonColor = new SolidColorBrush();

            // Sets color based on selected color.
            if (cellColor == "Gray")
            {
                buttonColor = new SolidColorBrush(Color.FromRgb(221, 221, 221));
            }
            else if (cellColor == "Cyan")
            {
                buttonColor = new SolidColorBrush(Color.FromRgb(2, 221, 221));
            }
            else if (cellColor == "Yellow")
            {
                buttonColor = new SolidColorBrush(Color.FromRgb(255, 255, 175));
            }
            else if (cellColor == "Pink")
            {
                buttonColor = new SolidColorBrush(Color.FromRgb(255, 175, 255));
            }

            // Sets grid settings.
            this.Grid.Name = "GameBoard";
            this.Grid.Width = w;
            this.Grid.Height = h;

            // Creates row and column definitions.
            RowDefinition r = new RowDefinition();
            ColumnDefinition c = new ColumnDefinition();

            // Adds rows to grid.
            for (int rowIndex = 0; rowIndex < row + 2; rowIndex++)
            {
                this.Grid.RowDefinitions.Add(r);

                r = new RowDefinition();
            }

            // Adds columns to grid.
            for (int colIndex = 0; colIndex < col; colIndex++)
            {
                this.Grid.ColumnDefinitions.Add(c);

                c = new ColumnDefinition();
            }

            // Adds buttons to grid based on grid size.
            for (int rowIndex = 0; rowIndex < row; rowIndex++)
            {
                for (int colIndex = 0; colIndex < col; colIndex++)
                {
                    Button buttons = new Button();

                    buttons.Name = "BR" + rowIndex + "C" + colIndex;
                    buttons.Background = buttonColor;
                    if (numberColor == "Black")
                    {
                        buttons.Foreground = Brushes.Black;
                    }
                    else if (numberColor == "Orange")
                    {
                        buttons.Foreground = Brushes.Gold;
                    }
                    else if (numberColor == "Green")
                    {
                        buttons.Foreground = Brushes.LawnGreen;
                    }
                    else if (numberColor == "Purple")
                    {
                        buttons.Foreground = Brushes.MediumVioletRed;
                    }

                    Grid.SetRow(buttons, rowIndex);
                    Grid.SetColumn(buttons, colIndex);
                    this.Grid.Children.Add(buttons);
                }
            }

            // Sets reset button settings.
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Content = "Reset";
            this.ResetButton.Background = buttonColor;
            Grid.SetRow(this.ResetButton, row + 1);
            this.Grid.Children.Add(this.ResetButton);
            if (col % 2 == 0)
            {
                Grid.SetColumn(this.ResetButton, (col / 2) - 2);
            }
            else
            {
                Grid.SetColumn(this.ResetButton, (col / 2) - 1);
            }

            // Sets exit button settings.
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Content = "Exit";
            this.ExitButton.Background = buttonColor;
            Grid.SetRow(this.ExitButton, row + 1);
            Grid.SetColumn(this.ExitButton, (col / 2) + 1);
            this.Grid.Children.Add(this.ExitButton);

            // Sets mines left label settings.
            this.MinesLeft.Name = "MinesLeft";
            this.MinesLeft.HorizontalAlignment = HorizontalAlignment.Center;
            this.MinesLeft.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(this.MinesLeft, row);
            Grid.SetColumnSpan(this.MinesLeft, 3);
            this.Grid.Children.Add(this.MinesLeft);

            // Sets timer text block settings.
            this.TimerText.Name = "Timer";
            this.TimerText.HorizontalAlignment = HorizontalAlignment.Center;
            this.TimerText.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(this.TimerText, row);
            Grid.SetColumnSpan(this.TimerText, 3);
            Grid.SetColumn(this.TimerText, col - 3);
            this.Grid.Children.Add(this.TimerText);

            // Adjusts column span of labels, and reset/exit buttons for larger grid sizes.
            if (col > 40)
            {
                Grid.SetColumnSpan(this.ResetButton, 4);
                Grid.SetColumnSpan(this.ExitButton, 4);
                Grid.SetColumnSpan(this.MinesLeft, 7);
                Grid.SetColumnSpan(this.TimerText, 7);
                Grid.SetColumn(this.ResetButton, (col / 2) - 4);
                Grid.SetColumn(this.ExitButton, (col / 2) + 3);
                Grid.SetColumn(this.TimerText, col - 7);
            }
        }

        /// <summary>
        /// Generate mine method. 
        /// </summary>
        /// <param name="mineCount">Number of mines</param>
        /// <param name="array">Array to hold mines</param>
        /// <param name="x">Number of rows</param>
        /// <param name="y">Number of columns</param>
        /// <returns>Filled array</returns>
        public int[,] GenerateMines(int mineCount, int[,] array, int x, int y)
        {
            // Random number generator formula.
            Random rand = new Random();

            // For loop to step through and display mines to board randomly.
            for (int i = 0; i < mineCount; i++)
            {
                // Declaring the randomNumber.
                int randomNumber = rand.Next(x);
                int randomNumber2 = rand.Next(y);

                // Boardspaces displaying the mine with the randomNumber variable. 
                array[randomNumber, randomNumber2] = 9;
            }

            // Variables for mine.
            int mine = 0;

            // Loop through the array using for loops.
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    // If statement for Boardspaces checking for equivalency.
                    if (array[i, j] == 9)
                    {
                        // If equal, increment mine. 
                        mine++;
                    }
                }
            }
            
            // If statement for mines less than ten.
            if (mine < mineCount)
            {
                // While loop for while mines are less than 10.
                while (mine < mineCount)
                {
                    // Declaring randomNumber to rand.Next.
                    int randomNumber = rand.Next(x);
                    int randomNumber2 = rand.Next(y);

                    // Boardspaces randomly display Mine.
                    array[randomNumber, randomNumber2] = 9;

                    // Mine variable set to 0.
                    mine = 0;

                    for (int i = 0; i < x; i++)
                    {
                        for (int j = 0; j < y; j++)
                        {
                            // If statement for Boardspaces checking for equivalency.
                            if (array[i, j] == 9)
                            {
                                // If equal, increment mine. 
                                mine++;
                            }
                        }
                    }
                }
            }

            // Declare index variables.
            int iR = 0;
            int iC = 0;

            // Generate array to hold completedC board.
            int[,] completeBoard = this.GenerateNumbers(array, x, y);

            // Sets tags to buttons based on array values.
            foreach (Button button in this.Grid.Children.OfType<Button>())
            {
                if (button != this.ExitButton && button != this.ResetButton)
                {
                    button.Tag = completeBoard[iR, iC].ToString();

                    iC++;
                    if (iC == y)
                    {
                        iC = 0;
                        iR++;
                    }
                }
            }

            // Returns complete array.
            return completeBoard;
        }

        /// <summary>
        /// Generate numbers based on where mines are.
        /// </summary>
        /// <param name="array">Array to hold numbers</param>
        /// <param name="x">Number of rows</param>
        /// <param name="y">Number of columns</param>
        /// <returns>Filled array</returns>
        public int[,] GenerateNumbers(int[,] array, int x, int y)
        {
            // Checks for mines and generates 1 around them.
            for (int iR = 0; iR < x; iR++)
            {
                for (int iC = 0; iC < y; iC++)
                {
                    if (array[iR, iC] == 9)
                    {
                        // Generates numbers around mines.
                        this.AdjacentNumbers(array, iR - 1, iC - 1, x, y);
                        this.AdjacentNumbers(array, iR, iC - 1, x, y);
                        this.AdjacentNumbers(array, iR + 1, iC - 1, x, y);
                        this.AdjacentNumbers(array, iR - 1, iC, x, y);
                        this.AdjacentNumbers(array, iR + 1, iC, x, y);
                        this.AdjacentNumbers(array, iR - 1, iC + 1, x, y);
                        this.AdjacentNumbers(array, iR, iC + 1, x, y);
                        this.AdjacentNumbers(array, iR + 1, iC + 1, x, y);
                    }
                }
            }

            // Returns array with numbers.
            return array;
        }

        /// <summary>
        /// Numbers around mine spaces.
        /// </summary>
        /// <param name="array">Array with values</param>
        /// <param name="iR">Row iteration</param>
        /// <param name="iC">Column iteration</param>
        /// <param name="x">Row count</param>
        /// <param name="y">Column count</param>
        public void AdjacentNumbers(int[,] array, int iR, int iC, int x, int y)
        {
            if (iR != -1 && iC != -1 && iR < x && iC < y && array[iR, iC] != 9)
            {
                array[iR, iC] += 1;
            }
        }
    }
}