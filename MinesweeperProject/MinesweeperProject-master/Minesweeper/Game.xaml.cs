//-----------------------------------------------------------------------
// <copyright file="Game.xaml.cs" company="Minesweeper Group">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Media;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Resources;
    using System.Windows.Shapes;
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Encapsulation not taught.")]
    public partial class Game : Window
    {
        /// <summary>
        /// Create new game board instance.
        /// </summary>
        public GenerateBoard GameBoard = new GenerateBoard();

        /// <summary>
        /// Create new array to hold parameter values to make them accessible elsewhere.
        /// </summary>
        public int[] ResetInfo = new int[3];

        /// <summary>
        /// Create new array for board values.
        /// </summary>
        public int[,] FilledBoard;

        /// <summary>
        /// Create new row size.
        /// </summary>
        public int RowSize;

        /// <summary>
        /// Create new column size.
        /// </summary>
        public int ColSize;

        /// <summary>
        /// Create new timer instance.
        /// </summary>
        public DispatcherTimer Timer = new DispatcherTimer();

        /// <summary>
        /// Create new seconds elapsed.
        /// </summary>
        public int SecondsElapsed;

        /// <summary>
        /// Create new minutes elapsed.
        /// </summary>
        public int MinutesElapsed;

        /// <summary>
        /// Create new state of timer.
        /// </summary>
        public bool TimerState = false;

        /// <summary>
        /// Boolean for First Click to true.
        /// </summary>
        public bool IsFirstClick = true;

        /// <summary>
        /// Create new state for check.
        /// </summary>
        public bool CheckState;

        /// <summary>
        /// Create new state for window check.
        /// </summary>
        public bool CheckWindowState;

        /// <summary>
        /// Create new array with buttons in grid.
        /// </summary>
        public Button[,] GridButtons;

        /// <summary>
        /// Create new array with various AI states.
        /// </summary>
        public bool[] AIChecked = new bool[2];

        /// <summary>
        /// Create new array with various color states.
        /// </summary>
        public string[] Colors = new string[3];

        /// <summary>
        /// Create new variable for using extra life.
        /// </summary>
        public bool UsedLife = false;

        /// <summary>
        /// Create new variable for luck check state.
        /// </summary>
        public bool LuckChecked;

        /// <summary>
        /// Create new variable for character check state.
        /// </summary>
        public bool CharChecked;

        /// <summary>
        /// Create new array with special characters.
        /// </summary>
        public string[] SpecialChars = new string[] { "%", "*", "^", "!", "#", "&", "$", "@" };

        /// <summary>
        /// Initializes a new instance of the <see cref="Game" /> class.
        /// </summary>
        /// <param name="x">Number of rows</param>
        /// <param name="y">Number of columns</param>
        /// <param name="w">Width of board</param>
        /// <param name="h">Height of board</param>
        /// <param name="mines">Number of mines</param>
        /// <param name="firstChecked">FirstChecked state</param>
        /// <param name="easyChecked">Easy AI checked state</param>
        /// <param name="hardChecked">Hard AI checked state</param>
        /// <param name="flagColor">Flag color</param>
        /// <param name="cellColor">Cell color</param>
        /// <param name="numberColor">Number color</param>
        /// <param name="luckChecked">Luck checked state</param>
        /// <param name="charChecked">Character checked state</param>
        public Game(int x, int y, int w, int h, int mines, bool firstChecked, bool easyChecked, bool hardChecked, string flagColor, string cellColor, string numberColor, bool luckChecked, bool charChecked)
        {
            this.InitializeComponent();

            // Set CheckWindowState value to true.
            this.CheckWindowState = true;

            // Set timer interval and tick variables.
            this.Timer.Interval = new TimeSpan(0, 0, 1);
            this.Timer.Tick += this.Timer_Tick;
            
            // Set row and column size to the given row and column parameters.
            this.RowSize = x;
            this.ColSize = y;

            // Set reset info to the given parameters.
            this.ResetInfo[0] = x;
            this.ResetInfo[1] = y;
            this.ResetInfo[2] = mines;

            // Set CheckState to the value of firstChecked.
            this.CheckState = firstChecked;

            // Set colors array to appropriate flag, cell, and number colors.
            this.Colors[0] = flagColor;
            this.Colors[1] = cellColor;
            this.Colors[2] = numberColor;

            // Fill the Label with Mine's left.
            this.GameBoard.MinesLeft.Text = "Mines Left: " + this.ResetInfo[2];

            // Fill the text-box with a blank timer.
            this.GameBoard.TimerText.Text = "Time: 00:00";

            // Generate the form and add the grid.
            this.GameBoard.GenerateForm(x, y, w, h, this.Colors[1], this.Colors[2]);
            this.AddChild(this.GameBoard.Grid);

            int[,] boardspaces = new int[x, y];

            this.FilledBoard = this.GameBoard.GenerateMines(mines, boardspaces, x, y);

            this.GameBoard.ExitButton.Click += new RoutedEventHandler(this.ExitButton_Click);

            this.GameBoard.ResetButton.Click += new RoutedEventHandler(this.ResetButton_Click);

            // Add buttons to the left and right click events.
            this.LeftClickButtons();
            this.RightClickButtons();

            int iR = 0;
            int iC = 0;

            this.GridButtons = new Button[x, y];

            // Set array variables to values to see if user chose ai.
            this.AIChecked[0] = easyChecked;
            this.AIChecked[1] = hardChecked;

            // Set value of luck checked to whether the user checked extra chance rule.
            this.LuckChecked = luckChecked;

            // Set value of char checked to whether user checked the special characters rule.
            this.CharChecked = charChecked;

            // Add grid buttons to the array.
            foreach (Button button in this.GameBoard.Grid.Children.OfType<Button>())
            {
                if (button != this.GameBoard.ExitButton && button != this.GameBoard.ResetButton)
                {
                    this.GridButtons[iR, iC] = button;
                }

                iC++;
                if (iC == this.ColSize)
                {
                    iC = 0;
                    iR++;
                }
            }

            this.AICall(this.AIChecked[0], this.AIChecked[1]);
        }

        /// <summary>
        /// Sets window state to false if form is closed.
        /// </summary>
        /// <param name="sender">Object Sender</param>
        /// <param name="e">Event Args</param>
        public void CloseForm(object sender, EventArgs e)
        {
            this.CheckWindowState = false;
        }

        /// <summary>
        /// Timer display.
        /// </summary>
        /// <param name="sender">Object Sender</param>
        /// <param name="e">Event Args</param>
        public void Timer_Tick(object sender, EventArgs e)
        {
            // If the seconds counter hits sixty, add one to the minute counter and reset the seconds counter.
            if (this.SecondsElapsed == 60)
            {
                this.MinutesElapsed++;
                this.SecondsElapsed = 0;
            }

            // Display the time elapsed in the text-box.
            this.GameBoard.TimerText.Text = "Time: " + this.MinutesElapsed.ToString("00") + ":" + this.SecondsElapsed.ToString("00");

            // Increase the timer.
            this.SecondsElapsed++;
        }

        /// <summary>
        /// Add buttons to left click event.
        /// </summary>
        public void LeftClickButtons()
        {
            // For each button in the grid, add the button to the left click event.
            foreach (Button buttons in this.GameBoard.Grid.Children.OfType<Button>())
            {
                if (buttons != this.GameBoard.ResetButton && buttons != this.GameBoard.ExitButton)
                {
                    buttons.Click += new RoutedEventHandler(this.Buttons_LeftClick);
                }
            }
        }

        /// <summary>
        /// Add buttons to right click event.
        /// </summary>
        public void RightClickButtons()
        {
            // For each button in the grid, add the button to the right click event.
            foreach (Button buttons in this.GameBoard.Grid.Children.OfType<Button>())
            {
                if (buttons != this.GameBoard.ResetButton && buttons != this.GameBoard.ExitButton)
                {
                    buttons.MouseRightButtonDown += new MouseButtonEventHandler(this.Buttons_RightClick);
                }
            }
        }

        /// <summary>
        /// Check for the win condition.
        /// </summary>
        public void CheckWin()
        {
            // Set the amount of un-clicked spaces to the total of the multiplied rows and columns.
            int unclicked = this.ResetInfo[0] * this.ResetInfo[1];
            int minesClicked = 0;

            // For each button in the grid, if the button is revealed (dark gray), subtract one from the un-clicked variable.
            foreach (Button buttons in this.GameBoard.Grid.Children.OfType<Button>())
            {
                if (buttons != this.GameBoard.ExitButton && buttons != this.GameBoard.ResetButton)
                {
                    if (buttons.Background != this.GameBoard.ExitButton.Background &&
                        buttons.Background != Brushes.Orange && buttons.Background != Brushes.YellowGreen)
                    {
                        unclicked--;
                    }

                    if (buttons.Background == Brushes.Red)
                    {
                        minesClicked++;
                    }
                }
            }

            // If no mines have been clicked, change button colors and display win message.
            if (minesClicked == 0)
            {
                // If the unclicked spaces equal the number of mines, set the unclicked spaces to be flagged (orange).
                if (unclicked == this.ResetInfo[2])
                {
                    foreach (Button buttons in this.GameBoard.Grid.Children.OfType<Button>())
                    {
                        if (buttons != this.GameBoard.ExitButton && buttons != this.GameBoard.ResetButton)
                        {
                            if (buttons.Background == this.GameBoard.ExitButton.Background)
                            {
                                buttons.Background = Brushes.Orange;
                            }
                        }
                    }

                    // Stop the timer and reset the timestate variable.
                    this.Timer.Stop();
                    this.TimerState = false;

                    // Calculate scores if a human is playing.
                    if (this.AIChecked[0] == false && this.AIChecked[1] == false)
                    {
                        this.CalculateScores();
                    }

                    // Ask user if they want to try again, if so, reset the board, timer, mine counter, and regenerate the board.
                    // If not, close the form.
                    if (MessageBox.Show("Do you want to play again?", "You Win!", MessageBoxButton.YesNo) ==
                        MessageBoxResult.Yes)
                    {
                        // Reset the board.
                        this.Reset();
                    }
                    else
                    {
                        // Closes form.
                        this.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Click event for ExitButton.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Routed Event</param>
        public void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Closes form.
            this.Close();
        }

        /// <summary>
        /// Click event for ResetButton.
        /// </summary>
        /// <param name="sender">Object Sender</param>
        /// <param name="e">Routed Event</param>
        public void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            // Stop the timer.
            this.Timer.Stop();
            this.TimerState = false;

            // Reset the board.
            this.Reset();
        }

        /// <summary>
        /// Click event for revealing a tile.
        /// </summary>
        /// <param name="sender">Object Sender</param>
        /// <param name="e">Routed Event</param>
        public void Buttons_LeftClick(object sender, RoutedEventArgs e)
        {
            // Set button variable to the sender.
            Button space = (Button)sender;

            // Left clicks a space.
            this.LeftClick(space);
        }

        /// <summary>
        /// Click event for cycling right click options
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Mouse event</param>
        public void Buttons_RightClick(object sender, MouseEventArgs e)
        {
            // Creates new button instance.
            Button space = (Button)sender;

            // Right clicks a space.
            this.RightClick(space);
        }

        /// <summary>
        /// Method for running the easy AI.
        /// </summary>
        /// <param name="x">Grid row count</param>
        /// <param name="y">Grid column count</param>
        /// <param name="mines">Mine count</param>
        /// <param name="isChecked">State of easy AI radio button</param>
        /// <param name="array">Array with buttons</param>
        public async void EasyComputer(int x, int y, int mines, bool isChecked, Button[,] array)
        {
            // Random declarations.
            Random xRand = new Random();
            Random yRand = new Random();

            // Click delay.
            await Task.Delay(1500);

            // Makes new random numbers.
            int randomX = xRand.Next(x);
            int randomY = yRand.Next(y);

            // Left clicks a space.
            this.LeftClick(array[randomX, randomY]);

            // Declares variable for unrevealed spaces.
            int unrevealed = 0;

            // Adds to unrevealed for each unrevealed space.
            foreach (Button button in array)
            {
                if (button.Background == this.GameBoard.ExitButton.Background)
                {
                    unrevealed++;
                }
            }

            // While there are unrevealed spaces in the array, generate a space in the array and reveal it, if it is not already revealed.
            while (unrevealed > 0 && array[randomX, randomY].Background != Brushes.Red)
            {
                // Breaks if form is closed.
                if (this.CheckWindowState == false)
                {
                    break;
                }

                await Task.Delay(1500);

                randomX = xRand.Next(x);
                randomY = yRand.Next(y);

                if (array[randomX, randomY].Background == Brushes.DarkGray)
                {
                    while (array[randomX, randomY].Background == Brushes.DarkGray)
                    {
                        randomX = xRand.Next(x);
                        randomY = yRand.Next(y);
                    }
                }

                this.LeftClick(array[randomX, randomY]);

                unrevealed = 0;
                
                foreach (Button test in array)
                {
                    if (test.Background == this.GameBoard.ExitButton.Background)
                    {
                        unrevealed++;
                    }
                }
            }
        }

        /// <summary>
        /// Method for running the hard AI.
        /// </summary>
        /// <param name="x">Grid row count</param>
        /// <param name="y">Grid column count</param>
        /// <param name="mines">Mine count</param>
        /// <param name="isChecked">State of easy AI radio button</param>
        /// <param name="array">Array with buttons</param>
        public async void HardComputer(int x, int y, int mines, bool isChecked, Button[,] array)
        {
            // Random declarations.
            Random xRand = new Random();
            Random yRand = new Random();

            // Click delay.
            await Task.Delay(1500);

            // Makes new random numbers.
            int randomX = xRand.Next(x);
            int randomY = yRand.Next(y);

            // Left clicks a space.
            this.LeftClick(array[randomX, randomY]);

            // Various iteration variables.
            int iR = 0;
            int iC = 0;
            int adjacent = 0;
            int flagged = 0;
            int current = 0;
            int largerNum = 0;

            // Take the larger number and assign it to a variable.
            if (x > y)
            {
                largerNum = x;
            }
            else
            {
                largerNum = y;
            }

            // For each button in the array, check for unrevealed adjacent spaces; if that space has adjacent spaces equivalent to the number of mines it touches, flag the space.
            foreach (Button button in array)
            {
                // Checks adjacent spaces and returns the proper value.
                adjacent = this.UnrevealedFlagged(array, button, iR - 1, iC - 1, adjacent, true);
                adjacent = this.UnrevealedFlagged(array, button, iR, iC - 1, adjacent, true);
                adjacent = this.UnrevealedFlagged(array, button, iR + 1, iC - 1, adjacent, true);
                adjacent = this.UnrevealedFlagged(array, button, iR - 1, iC, adjacent, true);
                adjacent = this.UnrevealedFlagged(array, button, iR + 1, iC, adjacent, true);
                adjacent = this.UnrevealedFlagged(array, button, iR - 1, iC + 1, adjacent, true);
                adjacent = this.UnrevealedFlagged(array, button, iR, iC + 1, adjacent, true);
                adjacent = this.UnrevealedFlagged(array, button, iR + 1, iC + 1, adjacent, true);

                // Set the string content to empty.
                string content = string.Empty;

                // If the content of the array space isn't null, set the variable to the converted character set value.
                if (array[iR, iC].Content != null)
                {
                    content = this.CharSetConverter(array[iR, iC].Content.ToString());
                }

                // If the value of the space equals the adjacent spaces, mark the adjacent spaces as guaranteed mines.
                if (array[iR, iC].Content == adjacent.ToString() || content == adjacent.ToString())
                {
                    if (iR - 1 != -1 && iC - 1 != -1 && array[iR - 1, iC - 1].Background != Brushes.DarkGray && array[iR - 1, iC - 1].Background != Brushes.Orange)
                    {
                        await Task.Delay(1500);

                        this.RightClick(array[iR - 1, iC - 1]);
                    }

                    if (iC - 1 != -1 && array[iR, iC - 1].Background != Brushes.DarkGray && array[iR, iC - 1].Background != Brushes.Orange)
                    {
                        await Task.Delay(1500);

                        this.RightClick(array[iR, iC - 1]);
                    }

                    if (iR + 1 != this.RowSize && iC - 1 != -1 && array[iR + 1, iC - 1].Background != Brushes.DarkGray && array[iR + 1, iC - 1].Background != Brushes.Orange)
                    {
                        await Task.Delay(1500);

                        this.RightClick(array[iR + 1, iC - 1]);
                    }

                    if (iR - 1 != -1 && array[iR - 1, iC].Background != Brushes.DarkGray && array[iR - 1, iC].Background != Brushes.Orange)
                    {
                        await Task.Delay(1500);

                        this.RightClick(array[iR - 1, iC]);
                    }

                    if (iR + 1 != this.RowSize && array[iR + 1, iC].Background != Brushes.DarkGray && array[iR + 1, iC].Background != Brushes.Orange)
                    {
                        await Task.Delay(1500);

                        this.RightClick(array[iR + 1, iC]);
                    }

                    if (iR - 1 != -1 && iC + 1 != this.ColSize && array[iR - 1, iC + 1].Background != Brushes.DarkGray && array[iR - 1, iC + 1].Background != Brushes.Orange)
                    {
                        await Task.Delay(1500);

                        this.RightClick(array[iR - 1, iC + 1]);
                    }

                    if (iC + 1 != this.ColSize && array[iR, iC + 1].Background != Brushes.DarkGray && array[iR, iC + 1].Background != Brushes.Orange)
                    {
                        await Task.Delay(1500);

                        this.RightClick(array[iR, iC + 1]);
                    }

                    if (iR + 1 != this.RowSize && iC + 1 != this.ColSize && array[iR + 1, iC + 1].Background != Brushes.DarkGray && array[iR + 1, iC + 1].Background != Brushes.Orange)
                    {
                        await Task.Delay(1500);

                        this.RightClick(array[iR + 1, iC + 1]);
                    }
                }

                // Set adjacent spaces to 0.
                adjacent = 0;

                // Iterate rows and columns.
                iC++;
                if (iC == this.ColSize)
                {
                    iC = 0;
                    iR++;
                }
            }

            // For each button in the array, check for flagged spaces; if that space has flagged spaces equivalent to the number of mines it touches, reveal the remaining spaces.
            // Additionally, run this code a set number of times, to ensure all necessary spaces have been revealed.
            for (int i = 0; i <= largerNum; i++)
            {
                iR = 0;
                iC = 0;

                foreach (Button button in array)
                {
                    // Checks adjacent spaces and returns the proper value.
                    flagged = this.UnrevealedFlagged(array, button, iR - 1, iC - 1, flagged, false);
                    flagged = this.UnrevealedFlagged(array, button, iR, iC - 1, flagged, false);
                    flagged = this.UnrevealedFlagged(array, button, iR + 1, iC - 1, flagged, false);
                    flagged = this.UnrevealedFlagged(array, button, iR - 1, iC, flagged, false);
                    flagged = this.UnrevealedFlagged(array, button, iR + 1, iC, flagged, false);
                    flagged = this.UnrevealedFlagged(array, button, iR - 1, iC + 1, flagged, false);
                    flagged = this.UnrevealedFlagged(array, button, iR, iC + 1, flagged, false);
                    flagged = this.UnrevealedFlagged(array, button, iR + 1, iC + 1, flagged, false);

                    // Set string content to empty.
                    string content = string.Empty;

                    // If array content is not null, set the string content to the converted character set value.
                    if (array[iR, iC].Content != null)
                    {
                        content = this.CharSetConverter(array[iR, iC].Content.ToString());
                    }

                    // If the content is equivalent to the flagged number and not null, left click the guaranteed safe spaces.
                    if ((array[iR, iC].Content == flagged.ToString() || content == flagged.ToString()) && array[iR, iC].Content != null)
                    {
                        if (iR - 1 != -1 && iC - 1 != -1 && array[iR - 1, iC - 1].Background != Brushes.DarkGray && array[iR - 1, iC - 1].Background != Brushes.Orange)
                        {
                            await Task.Delay(1500);

                            this.LeftClick(array[iR - 1, iC - 1]);
                        }

                        if (iC - 1 != -1 && array[iR, iC - 1].Background != Brushes.DarkGray && array[iR, iC - 1].Background != Brushes.Orange)
                        {
                            await Task.Delay(1500);

                            this.LeftClick(array[iR, iC - 1]);
                        }

                        if (iR + 1 != this.RowSize && iC - 1 != -1 && array[iR + 1, iC - 1].Background != Brushes.DarkGray && array[iR + 1, iC - 1].Background != Brushes.Orange)
                        {
                            await Task.Delay(1500);

                            this.LeftClick(array[iR + 1, iC - 1]);
                        }

                        if (iR - 1 != -1 && array[iR - 1, iC].Background != Brushes.DarkGray && array[iR - 1, iC].Background != Brushes.Orange)
                        {
                            await Task.Delay(1500);

                            this.LeftClick(array[iR - 1, iC]);
                        }

                        if (iR + 1 != this.RowSize && array[iR + 1, iC].Background != Brushes.DarkGray && array[iR + 1, iC].Background != Brushes.Orange)
                        {
                            await Task.Delay(1500);

                            this.LeftClick(array[iR + 1, iC]);
                        }

                        if (iR - 1 != -1 && iC + 1 != this.ColSize && array[iR - 1, iC + 1].Background != Brushes.DarkGray && array[iR - 1, iC + 1].Background != Brushes.Orange)
                        {
                            await Task.Delay(1500);

                            this.LeftClick(array[iR - 1, iC + 1]);
                        }

                        if (iC + 1 != this.ColSize && array[iR, iC + 1].Background != Brushes.DarkGray && array[iR, iC + 1].Background != Brushes.Orange)
                        {
                            await Task.Delay(1500);

                            this.LeftClick(array[iR, iC + 1]);
                        }

                        if (iR + 1 != this.RowSize && iC + 1 != this.ColSize && array[iR + 1, iC + 1].Background != Brushes.DarkGray && array[iR + 1, iC + 1].Background != Brushes.Orange)
                        {
                            await Task.Delay(1500);

                            this.LeftClick(array[iR + 1, iC + 1]);
                        }
                    }

                    // Iterate through the array
                    iC++;
                    if (iC == this.ColSize)
                    {
                        iC = 0;
                        iR++;
                    }

                    // Reset flagged variable to 0.
                    flagged = 0;
                }
            }

            // Reset unrevealed to 0.
            int unrevealed = 0;

            // Check the number of unrevealed spaces in the grid.
            foreach (Button button in array)
            {
                if (button.Background == this.GameBoard.ExitButton.Background || button.Background == Brushes.Orange)
                {
                    unrevealed++;
                }
            }

            // Reset iteration variables to 0.
            iR = 0;
            iC = 0;

            // While there are more unrevealed spaces than mines, and there are still spaces to right click or reveal, check whether mines need to be flagged and flag them.
            // Additionally, check if the number of flagged spaces is equivalent to the number in the button, and if so, reveal all other adjacent spaces of the button.
            while (unrevealed > mines && array[randomX, randomY].Background != Brushes.Red)
            {
                // Breaks if form is closed.
                if (this.CheckWindowState == false)
                {
                    break;
                }

                // Set current variable to 1.
                current = 1;

                // While a new space has been clicked, and a mine has not been, continue right and left clicking to reveal the board as necessary.
                while (current > 0 && array[randomX, randomY].Background != Brushes.Red)
                {
                    // Breaks if form is closed.
                    if (this.CheckWindowState == false)
                    {
                        break;
                    }

                    // Set current to 0.
                    current = 0;

                    // For each button in the array, count the revealed adjacent spaces, and right click guaranteed mine spaces.
                    foreach (Button button in array)
                    {
                        // Checks adjacent spaces and returns the proper value.
                        adjacent = this.UnrevealedFlagged(array, button, iR - 1, iC - 1, adjacent, true);
                        adjacent = this.UnrevealedFlagged(array, button, iR, iC - 1, adjacent, true);
                        adjacent = this.UnrevealedFlagged(array, button, iR + 1, iC - 1, adjacent, true);
                        adjacent = this.UnrevealedFlagged(array, button, iR - 1, iC, adjacent, true);
                        adjacent = this.UnrevealedFlagged(array, button, iR + 1, iC, adjacent, true);
                        adjacent = this.UnrevealedFlagged(array, button, iR - 1, iC + 1, adjacent, true);
                        adjacent = this.UnrevealedFlagged(array, button, iR, iC + 1, adjacent, true);
                        adjacent = this.UnrevealedFlagged(array, button, iR + 1, iC + 1, adjacent, true);

                        // Set string content to empty.
                        string content = string.Empty;

                        // If array content is not null, set the variable content to the converted character value.
                        if (array[iR, iC].Content != null)
                        {
                            content = this.CharSetConverter(array[iR, iC].Content.ToString());
                        }

                        // If the content is equal to the number of adjacent spaces, right click the spaces as guaranteed mines.
                        if (array[iR, iC].Content == adjacent.ToString() || content == adjacent.ToString())
                        {
                            if (iR - 1 != -1 && iC - 1 != -1 && array[iR - 1, iC - 1].Background != Brushes.DarkGray && array[iR - 1, iC - 1].Background != Brushes.Orange)
                            {
                                await Task.Delay(1500);

                                this.RightClick(array[iR - 1, iC - 1]);

                                current++;
                            }

                            if (iC - 1 != -1 && array[iR, iC - 1].Background != Brushes.DarkGray && array[iR, iC - 1].Background != Brushes.Orange)
                            {
                                await Task.Delay(1500);

                                this.RightClick(array[iR, iC - 1]);

                                current++;
                            }

                            if (iR + 1 != this.RowSize && iC - 1 != -1 && array[iR + 1, iC - 1].Background != Brushes.DarkGray && array[iR + 1, iC - 1].Background != Brushes.Orange)
                            {
                                await Task.Delay(1500);

                                this.RightClick(array[iR + 1, iC - 1]);

                                current++;
                            }

                            if (iR - 1 != -1 && array[iR - 1, iC].Background != Brushes.DarkGray && array[iR - 1, iC].Background != Brushes.Orange)
                            {
                                await Task.Delay(1500);

                                this.RightClick(array[iR - 1, iC]);

                                current++;
                            }

                            if (iR + 1 != this.RowSize && array[iR + 1, iC].Background != Brushes.DarkGray && array[iR + 1, iC].Background != Brushes.Orange)
                            {
                                await Task.Delay(1500);

                                this.RightClick(array[iR + 1, iC]);

                                current++;
                            }

                            if (iR - 1 != -1 && iC + 1 != this.ColSize && array[iR - 1, iC + 1].Background != Brushes.DarkGray && array[iR - 1, iC + 1].Background != Brushes.Orange)
                            {
                                await Task.Delay(1500);

                                this.RightClick(array[iR - 1, iC + 1]);

                                current++;
                            }

                            if (iC + 1 != this.ColSize && array[iR, iC + 1].Background != Brushes.DarkGray && array[iR, iC + 1].Background != Brushes.Orange)
                            {
                                await Task.Delay(1500);

                                this.RightClick(array[iR, iC + 1]);

                                current++;
                            }

                            if (iR + 1 != this.RowSize && iC + 1 != this.ColSize && array[iR + 1, iC + 1].Background != Brushes.DarkGray && array[iR + 1, iC + 1].Background != Brushes.Orange)
                            {
                                await Task.Delay(1500);

                                this.RightClick(array[iR + 1, iC + 1]);

                                current++;
                            }
                        }

                        // Reset the adjacent variable to 0.
                        adjacent = 0;

                        // Iterate through the array.
                        iC++;
                        if (iC == this.ColSize)
                        {
                            iC = 0;
                            iR++;
                        }
                    }

                    // Iterate through left clicks to ensure even the largest boards have all appropriate left clicks completed.
                    for (int i = 0; i <= largerNum; i++)
                    {
                        // Reset the iteration variables to 0.
                        iR = 0;
                        iC = 0;

                        // For each button in the array, check the spaces for flags, and if there are as many flags as the value of the space, reveal them as guarantees safe spaces.
                        foreach (Button button in array)
                        {
                            // Checks adjacent spaces and returns the proper value.
                            flagged = this.UnrevealedFlagged(array, button, iR - 1, iC - 1, flagged, false);
                            flagged = this.UnrevealedFlagged(array, button, iR, iC - 1, flagged, false);
                            flagged = this.UnrevealedFlagged(array, button, iR + 1, iC - 1, flagged, false);
                            flagged = this.UnrevealedFlagged(array, button, iR - 1, iC, flagged, false);
                            flagged = this.UnrevealedFlagged(array, button, iR + 1, iC, flagged, false);
                            flagged = this.UnrevealedFlagged(array, button, iR - 1, iC + 1, flagged, false);
                            flagged = this.UnrevealedFlagged(array, button, iR, iC + 1, flagged, false);
                            flagged = this.UnrevealedFlagged(array, button, iR + 1, iC + 1, flagged, false);

                            string content = string.Empty;

                            if (array[iR, iC].Content != null)
                            {
                                content = this.CharSetConverter(array[iR, iC].Content.ToString());
                            }

                            if ((array[iR, iC].Content == flagged.ToString() || content == flagged.ToString()) && array[iR, iC].Content != null)
                            {
                                if (iR - 1 != -1 && iC - 1 != -1 && array[iR - 1, iC - 1].Background != Brushes.DarkGray && array[iR - 1, iC - 1].Background != Brushes.Orange)
                                {
                                    await Task.Delay(1500);

                                    this.LeftClick(array[iR - 1, iC - 1]);

                                    current++;
                                }

                                if (iC - 1 != -1 && array[iR, iC - 1].Background != Brushes.DarkGray && array[iR, iC - 1].Background != Brushes.Orange)
                                {
                                    await Task.Delay(1500);

                                    this.LeftClick(array[iR, iC - 1]);

                                    current++;
                                }

                                if (iR + 1 != this.RowSize && iC - 1 != -1 && array[iR + 1, iC - 1].Background != Brushes.DarkGray && array[iR + 1, iC - 1].Background != Brushes.Orange)
                                {
                                    await Task.Delay(1500);

                                    this.LeftClick(array[iR + 1, iC - 1]);

                                    current++;
                                }

                                if (iR - 1 != -1 && array[iR - 1, iC].Background != Brushes.DarkGray && array[iR - 1, iC].Background != Brushes.Orange)
                                {
                                    await Task.Delay(1500);

                                    this.LeftClick(array[iR - 1, iC]);

                                    current++;
                                }

                                if (iR + 1 != this.RowSize && array[iR + 1, iC].Background != Brushes.DarkGray && array[iR + 1, iC].Background != Brushes.Orange)
                                {
                                    await Task.Delay(1500);

                                    this.LeftClick(array[iR + 1, iC]);

                                    current++;
                                }

                                if (iR - 1 != -1 && iC + 1 != this.ColSize && array[iR - 1, iC + 1].Background != Brushes.DarkGray && array[iR - 1, iC + 1].Background != Brushes.Orange)
                                {
                                    await Task.Delay(1500);

                                    this.LeftClick(array[iR - 1, iC + 1]);

                                    current++;
                                }

                                if (iC + 1 != this.ColSize && array[iR, iC + 1].Background != Brushes.DarkGray && array[iR, iC + 1].Background != Brushes.Orange)
                                {
                                    await Task.Delay(1500);

                                    this.LeftClick(array[iR, iC + 1]);

                                    current++;
                                }

                                if (iR + 1 != this.RowSize && iC + 1 != this.ColSize && array[iR + 1, iC + 1].Background != Brushes.DarkGray && array[iR + 1, iC + 1].Background != Brushes.Orange)
                                {
                                    await Task.Delay(1500);

                                    this.LeftClick(array[iR + 1, iC + 1]);

                                    current++;
                                }
                            }

                            // Iterate through the array.
                            iC++;
                            if (iC == this.ColSize)
                            {
                                iC = 0;
                                iR++;
                            }

                            // Reset flagged variable to 0.
                            flagged = 0;
                        }
                    }

                    // Reset unrevealed to 0.
                    unrevealed = 0;

                    // Check the number of unrevealed spaces in the grid.
                    foreach (Button button in array)
                    {
                        if (button.Background == this.GameBoard.ExitButton.Background || button.Background == Brushes.Orange)
                        {
                            unrevealed++;
                        }
                    }

                    // Reset iteration variables to 0.
                    iR = 0;
                    iC = 0;
                }

                // Reset iteration variables to 0.
                iR = 0;
                iC = 0;

                // If there are still more unrevealed spaces than mines, click a random space.
                if (unrevealed > mines)
                {
                    await Task.Delay(1500);

                    randomX = xRand.Next(x);
                    randomY = yRand.Next(y);

                    while (array[randomX, randomY].Background != this.GameBoard.ExitButton.Background)
                    {
                        randomX = xRand.Next(x);
                        randomY = yRand.Next(y);
                    }

                    this.LeftClick(array[randomX, randomY]);
                }
            }
        }

        /// <summary>
        /// Resets game.
        /// </summary>
        public void Reset()
        {
            // for each button in the grid, reset the buttons to their default color.
            foreach (Button buttons in this.GameBoard.Grid.Children.OfType<Button>())
            {
                if (buttons != this.GameBoard.ExitButton && buttons != this.GameBoard.ResetButton)
                {
                    // Set button background to default color.
                    buttons.Background = this.GameBoard.ExitButton.Background;

                    // Set button to null.
                    buttons.Content = null;
                    buttons.Tag = null;
                }
            }

            // Reset the mine count label.
            this.GameBoard.MinesLeft.Text = "Mines Left: " + this.ResetInfo[2];

            // Reset the timer.
            this.GameBoard.TimerText.Text = "Time: 00:00";
            this.SecondsElapsed = 0;
            this.MinutesElapsed = 0;

            // Reset first click state.
            this.IsFirstClick = true;

            // Create a new appropriately sized array and regenerate mines and numbers.
            int[,] boardspaces = new int[this.ResetInfo[0], this.ResetInfo[1]];
            this.FilledBoard = this.GameBoard.GenerateMines(this.ResetInfo[2], boardspaces, this.ResetInfo[0], this.ResetInfo[1]);

            // Reset window state.
            this.CheckWindowState = true;

            // Reset used life.
            this.UsedLife = false;
        }

        /// <summary>
        /// Method for left click code.
        /// </summary>
        /// <param name="space">Specific button</param>
        public void LeftClick(Button space)
        {
            // Generate spread.
            GenerateSpread spread = new GenerateSpread();

            // Check for first click.
            if (this.IsFirstClick == true && space.Tag == "9" && this.CheckState == true)
            {
                MessageBox.Show("That space is a mine. Try a new space.");
            }
            else
            {
                // If the timer hasn't started yet, start the timer.
                if (this.TimerState == false)
                {
                    this.Timer.Start();
                    this.TimerState = true;
                }

                if (this.IsFirstClick == true)
                {
                    this.IsFirstClick = false;
                }

                // Declare row and col variables.
                int row;
                int col;

                // If the space is unrevealed (exit button background color), display the space(s).
                if (space.Background == this.GameBoard.ExitButton.Background)
                {
                    // Depending on what type of space is clicked on, display spaces appropriately.
                    if (space.Tag == "9")
                    {
                        Random rand = new Random();
                        int num = rand.Next(1, 100);

                        if (num <= 5 && this.UsedLife == false && this.LuckChecked == true)
                        {
                            MessageBox.Show("You got an extra chance!");

                            this.RightClick(space);

                            this.UsedLife = true;
                        }
                        else
                        {
                            Image im = new Image();
                            im.Source = new BitmapImage(new Uri("Assets/Mine.png", UriKind.Relative));

                            StreamResourceInfo sri = Application.GetResourceStream(new Uri("Assets/MineAudio.wav", UriKind.Relative));
                            SoundPlayer sp = new SoundPlayer(sri.Stream);
                            sp.Play();

                            // Display the clicked mine in red.
                            space.Background = Brushes.Red;

                            // Display all other mine spaces.
                            foreach (Button space2 in this.GameBoard.Grid.Children.OfType<Button>())
                            {
                                if (space2.Tag == "9")
                                {
                                    space2.Content = im;

                                    im = new Image();
                                    im.Source = new BitmapImage(new Uri("Assets/Mine.png", UriKind.Relative));
                                }
                            }

                            // Stop the timer and reset the timer state to false.
                            this.Timer.Stop();
                            this.TimerState = false;

                            // Calculate scores if the player is a human.
                            if (this.AIChecked[0] == false && this.AIChecked[1] == false)
                            {
                                this.CalculateScores();
                            }

                            // Ask user if they want to try again, if so, reset the board, timer, mine counter, and regenerate the board.
                            // If not, close the form.
                            if (MessageBox.Show("Do you want to try again?", "Play Again?", MessageBoxButton.YesNo) ==
                                MessageBoxResult.Yes)
                            {
                                // Reset the board.
                                this.Reset();
                            }
                            else
                            {
                                // Closes form.
                                this.Close();
                            }
                        }
                    }
                    else if (space.Tag == "0")
                    {
                        // Sets original blank space to different color.
                        space.Background = Brushes.DarkGray;

                        // Gets position of buttons.
                        if (space.Name[3] == 'C')
                        {
                            if (space.Name.Length < 6)
                            {
                                row = int.Parse(space.Name[2].ToString());
                                col = int.Parse(space.Name[4].ToString());
                            }
                            else
                            {
                                row = int.Parse(space.Name[2].ToString());
                                string colString = space.Name[4].ToString() + space.Name[5];
                                col = int.Parse(colString);
                            }
                        }
                        else
                        {
                            if (space.Name.Length < 7)
                            {
                                string rowString = space.Name[2].ToString() + space.Name[3];
                                row = int.Parse(rowString);
                                col = int.Parse(space.Name[5].ToString());
                            }
                            else
                            {
                                string rowString = space.Name[2].ToString() + space.Name[3];
                                row = int.Parse(rowString);
                                string colString = space.Name[5].ToString() + space.Name[6];
                                col = int.Parse(colString);
                            }
                        }

                        // Do the spread.
                        spread.Spread(row, col, this.FilledBoard, this.RowSize, this.ColSize);

                        // Variable declarations for eventually stopping spread.
                        int previous = -1;
                        int current = 0;

                        // Subsequent checks for surrounding blank spaces and avoids edge of board.
                        while (previous < current)
                        {
                            previous = current;

                            current = 0;

                            for (int r = 0; r < this.RowSize; r++)
                            {
                                for (int c = 0; c < this.ColSize; c++)
                                {
                                    if (this.FilledBoard[r, c] == 10)
                                    {
                                        spread.Spread(r, c, this.FilledBoard, this.RowSize, this.ColSize);
                                    }
                                }
                            }

                            foreach (int filledSpace in this.FilledBoard)
                            {
                                if (filledSpace == 10)
                                {
                                    current++;
                                }
                            }
                        }

                        // Index declarations.
                        int iR = 0;
                        int iC = 0;

                        // Set blank spaces' colors and tags.
                        foreach (Button buttons in this.GameBoard.Grid.Children.OfType<Button>())
                        {
                            if (buttons != this.GameBoard.ExitButton && buttons != this.GameBoard.ResetButton)
                            {
                                if (this.FilledBoard[iR, iC] == 10)
                                {
                                    buttons.Content = string.Empty;

                                    buttons.Background = Brushes.DarkGray;
                                }
                                else if (this.FilledBoard[iR, iC] == 11)
                                {
                                    if (this.CharChecked == true)
                                    {
                                        for (int i = 0; i < 8; i++)
                                        {
                                            if (buttons.Tag == (i + 1).ToString())
                                            {
                                                buttons.Content = this.SpecialChars[i];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        buttons.Content = buttons.Tag;
                                    }

                                    buttons.Background = Brushes.DarkGray;
                                }

                                iC++;
                                if (iC == this.ColSize)
                                {
                                    iC = 0;
                                    iR++;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (this.CharChecked == true)
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                if (space.Tag == (i + 1).ToString())
                                {
                                    space.Content = this.SpecialChars[i];
                                }
                            }
                        }
                        else
                        {
                            space.Content = space.Tag;
                        }

                        space.Background = Brushes.DarkGray;
                    }
                }
            }

            // Checks for win conditions.
            this.CheckWin();
        }
        
        /// <summary>
        /// Method for right click code.
        /// </summary>
        /// <param name="space">Specific button</param>
        public void RightClick(Button space)
        {
            if (space.Background != Brushes.DarkGray)
            {
                // Checks what color is displaying.
                if (space.Background == this.GameBoard.ExitButton.Background)
                {
                    Image im = new Image();

                    if (this.Colors[0] == "Red")
                    {
                        im.Source = new BitmapImage(new Uri("Assets/Flag.png", UriKind.Relative));
                    }
                    else if (this.Colors[0] == "Yellow")
                    {
                        im.Source = new BitmapImage(new Uri("Assets/Flag3.png", UriKind.Relative));
                    }
                    else if (this.Colors[0] == "Green")
                    {
                        im.Source = new BitmapImage(new Uri("Assets/Flag2.png", UriKind.Relative));
                    }
                    else if (this.Colors[0] == "Blue")
                    {
                        im.Source = new BitmapImage(new Uri("Assets/Flag4.png", UriKind.Relative));
                    }

                    space.Content = im;

                    StreamResourceInfo sri = Application.GetResourceStream(new Uri("Assets/FlagAudio.wav", UriKind.Relative));
                    SoundPlayer sp = new SoundPlayer(sri.Stream);
                    sp.Play();

                    // Background changes to orange.
                    space.Background = Brushes.Orange;
                }
                else if (space.Background == Brushes.Orange)
                {
                    Image im = new Image();
                    im.Source = new BitmapImage(new Uri("Assets/Question.png", UriKind.Relative));
                    space.Content = im;

                    StreamResourceInfo sri = Application.GetResourceStream(new Uri("Assets/QuestionAudio.wav", UriKind.Relative));
                    SoundPlayer sp = new SoundPlayer(sri.Stream);
                    sp.Play();

                    // Background changes to yellow-green.
                    space.Background = Brushes.YellowGreen;
                }
                else if (space.Background == Brushes.YellowGreen)
                {
                    // Background changes to default.
                    space.Content = null;
                    space.Background = this.GameBoard.ExitButton.Background;
                }
            }

            // Variable declaration.
            int flagged = 0;

            // Changes variable based on amount of right-clicked spaces.
            foreach (Button buttons in this.GameBoard.Grid.Children.OfType<Button>())
            {
                if (buttons.Background == Brushes.Orange)
                {
                    flagged++;
                }
            }

            // Changes count of mines left based on variable.
            this.GameBoard.MinesLeft.Text = "Mines Left: " + (this.ResetInfo[2] - flagged);
        }

        /// <summary>
        /// Calculates scores for the file based off of correctly flagged mines.
        /// </summary>
        public void CalculateScores()
        {
            // Gets scores in old file and sorts them.
            StreamReader inScoreFile;

            List<int> scores = new List<int>();

            inScoreFile = File.OpenText("scores.txt");

            string content = string.Empty;

            while (!inScoreFile.EndOfStream)
            {
                content = inScoreFile.ReadLine();
                scores.Add(int.Parse(content));
            }

            scores.Sort();
            scores.Reverse();

            // Sets the score variable to 0.
            int score = 0;

            // Calculates the score based on number of mines found correctly.
            foreach (Button button in this.GameBoard.Grid.Children.OfType<Button>())
            {
                if (button.Background == Brushes.Orange && button.Tag == "9")
                {
                    score++;
                }
            }

            // Replaces old scores with new scores.
            if (scores.Count < 10)
            {
                scores.Add(score);
            }
            else
            {
                if (score >= scores[0] || score >= scores[1] || score >= scores[2] || score >= scores[3] || score >= scores[4])
                {
                    scores[4] = score;
                }

                if (score <= scores[5] || score <= scores[6] || score <= scores[7] || score <= scores[8] || score <= scores[9])
                {
                    scores[5] = score;
                }
            }

            // Closes file.
            inScoreFile.Close();

            // Writes new values to new file.
            StreamWriter outScoreFile;
            outScoreFile = File.CreateText("scores.txt");

            foreach (int highLow in scores)
            {
                outScoreFile.WriteLine(highLow);
            }
            
            // Closes file.
            outScoreFile.Close();
        }

        /// <summary>
        /// Converts characters to usable numbers.
        /// </summary>
        /// <param name="content">Gets button content</param>
        /// <returns>Returns usable number</returns>
        public string CharSetConverter(string content)
        {
            // Declares variable to hold number.
            string value = "x";

            // Sets number based off of whichever character is showing.
            if (content == "%")
            {
                value = "1";
            }
            else if (content == "*")
            {
                value = "2";
            }
            else if (content == "^")
            {
                value = "3";
            }
            else if (content == "!")
            {
                value = "4";
            }
            else if (content == "#")
            {
                value = "5";
            }
            else if (content == "&")
            {
                value = "6";
            }
            else if (content == "$")
            {
                value = "7";
            }
            else if (content == "@")
            {
                value = "8";
            }

            // Returns value.
            return value;
        }

        /// <summary>
        /// Iterates adjacent or flagged variable depending on where called.
        /// </summary>
        /// <param name="array">Array with buttons</param>
        /// <param name="button">Button variable</param>
        /// <param name="iR">Index row</param>
        /// <param name="iC">Index column</param>
        /// <param name="value">Adjacent or flagged</param>
        /// <param name="adjOrFlag">Bool for adjacent or flagged</param>
        /// <returns>Returns adjacent or flagged value</returns>
        public int UnrevealedFlagged(Button[,] array, Button button, int iR, int iC, int value, bool adjOrFlag)
        {
            // Iterates variable based on what space it is on.
            if (button.Tag != "9" && button.Tag != "0" && button.Content != null)
            {
                if (iR != -1 && iC != -1 && iR < this.RowSize && iC < this.ColSize)
                {
                    if (adjOrFlag == true && array[iR, iC].Background != Brushes.DarkGray)
                    {
                        value++;
                    }
                    else if (adjOrFlag == false && array[iR, iC].Background == Brushes.Orange)
                    {
                        value++;
                    }
                }
            }

            // Returns value.
            return value;
        }
        
        /// <summary>
        /// Runs either easy or hard AI.
        /// </summary>
        /// <param name="easy">Easy AI bool</param>
        /// <param name="hard">Hard AI bool</param>
        public void AICall(bool easy, bool hard)
        {
            // If user chose to run the easy ai, call the appropriate method and prevent user from interacting with the gameboard.
            if (easy == true)
            {
                this.EasyComputer(this.ResetInfo[0], this.ResetInfo[1], this.ResetInfo[2], this.AIChecked[0], this.GridButtons);

                // Loop that makes it so user can not interact with the game board
                foreach (Button test in this.GameBoard.Grid.Children.OfType<Button>())
                {
                    if (test != this.GameBoard.ExitButton)
                    {
                        test.IsHitTestVisible = false;
                    }
                }
            }

            // If user chose to run the hard ai, call the appropriate method and prevent user from interacting with the gameboard.
            if (hard == true)
            {
                this.HardComputer(this.ResetInfo[0], this.ResetInfo[1], this.ResetInfo[2], this.AIChecked[1], this.GridButtons);

                // Loop that makes it so user can not interact with the game board
                foreach (Button test in this.GameBoard.Grid.Children.OfType<Button>())
                {
                    if (test != this.GameBoard.ExitButton)
                    {
                        test.IsHitTestVisible = false;
                    }
                }
            }
        }
    }
}