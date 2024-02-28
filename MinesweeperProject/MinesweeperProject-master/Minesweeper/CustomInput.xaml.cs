//-----------------------------------------------------------------------
// <copyright file="CustomInput.xaml.cs" company="Minesweeper Group">
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
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for CustomInput.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Encapsulation not taught.")]
    public partial class CustomInput : Window
    {
        /// <summary>
        /// Create new check for click variable.
        /// </summary>
        public bool HasBeenClicked;

        /// <summary>
        /// Create new row number.
        /// </summary>
        public int InputRow;

        /// <summary>
        /// Create new column number.
        /// </summary>
        public int InputCol;

        /// <summary>
        /// Create new grid height.
        /// </summary>
        public int GridHeight;

        /// <summary>
        /// Create new grid width.
        /// </summary>
        public int GridWidth;

        /// <summary>
        /// Create new mine count.
        /// </summary>
        public int InputMine;

        /// <summary>
        /// Create new state of first click safe check box.
        /// </summary>
        public bool FirstChecked;

        /// <summary>
        /// Create new state of easy AI button.
        /// </summary>
        public bool EasyChecked;

        /// <summary>
        /// Create new state of hard AI button.
        /// </summary>
        public bool HardChecked;

        /// <summary>
        /// Create new state of flag color check box.
        /// </summary>
        public string FlagColor;

        /// <summary>
        /// Create new state of cell color check box.
        /// </summary>
        public string CellColor;
        
        /// <summary>
        /// Create new state of color of number.
        /// </summary>
        public string NumberColor;

        /// <summary>
        /// Create new state of luck check box.
        /// </summary>
        public bool LuckChecked;

        /// <summary>
        /// Create new state of character check box.
        /// </summary>
        public bool CharChecked;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomInput" /> class.
        /// </summary>
        /// <param name="firstChecked">Parameter determining check state</param>
        /// <param name="easy">Parameter determining easy AI check state</param>
        /// <param name="hard">Parameter determining hard AI check state</param>
        /// <param name="flagColor">Parameter determining flag color check state</param>
        /// <param name="cellColor">Parameter determining cell color check state</param>
        /// <param name="numberColor">Parameter determining number color check state</param>
        /// <param name="luckChecked">Parameter determining luck chance check state</param>
        /// <param name="charChecked">Parameter determining character check state</param>
        public CustomInput(bool firstChecked, bool easy, bool hard, string flagColor, string cellColor, string numberColor, bool luckChecked, bool charChecked)
        {
            this.InitializeComponent();

            // Sets various check states to passed parameter.
            this.FirstChecked = firstChecked;

            this.EasyChecked = easy;

            this.HardChecked = hard;

            this.FlagColor = flagColor;

            this.CellColor = cellColor;

            this.NumberColor = numberColor;

            this.LuckChecked = luckChecked;

            this.CharChecked = charChecked;
        }

        /// <summary>
        /// Click event for the start button
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Routed event</param>
        private void CustomStartButton_Click(object sender, RoutedEventArgs e)
        {
            // Checks for integers.
            if (int.TryParse(CustomRowsText.Text, out this.InputRow) && int.TryParse(CustomColsText.Text, out this.InputCol) && int.TryParse(CustomMinesText.Text, out this.InputMine))
            {
                // Checks for minimum row number.
                if (this.InputRow >= 4)
                {
                    // Checks for minimum column number.
                    if (this.InputCol >= 4)
                    {
                        // Checks for maximum row number.
                        if (this.InputRow <= 35)
                        {
                            // Checks for maximum column number.
                            if (this.InputCol <= 90)
                            {
                                // Checks for maximum mine count
                                if (this.InputMine < this.InputRow * this.InputCol && this.InputMine > 0)
                                {
                                    // Sets grid size based on number of rows and columns.
                                    if (this.InputRow <= 8 && this.InputCol <= 8)
                                    {
                                        this.GridWidth = 300;
                                        this.GridHeight = 300;
                                    }
                                    else if (this.InputRow <= 16 && this.InputCol <= 16)
                                    {
                                        this.GridWidth = 599;
                                        this.GridHeight = 599;
                                    }
                                    else if (this.InputRow <= 16 && this.InputCol <= 30)
                                    {
                                        this.GridWidth = 1150;
                                        this.GridHeight = 599;
                                    }
                                    else
                                    {
                                        this.GridWidth = 1300;
                                        this.GridHeight = 660;
                                    }

                                    // Closes form.
                                    this.Close();

                                    // If one of the computer players is checked then pass board setup to the computer player.
                                    if (this.EasyChecked == true)
                                    {
                                        // Passes easy board settings.
                                        Game genBoard = new Game(this.InputRow, this.InputCol, this.GridWidth, this.GridHeight, this.InputMine, this.FirstChecked, this.EasyChecked, this.HardChecked, this.FlagColor, this.CellColor, this.NumberColor, this.LuckChecked, this.CharChecked);

                                        // Displays board.
                                        genBoard.ShowDialog();
                                    }
                                    else if (this.HardChecked == true)
                                    {
                                        Game genBoard = new Game(this.InputRow, this.InputCol, this.GridWidth, this.GridHeight, this.InputMine, this.FirstChecked, this.EasyChecked, this.HardChecked, this.FlagColor, this.CellColor, this.NumberColor, this.LuckChecked, this.CharChecked);

                                        // Displays board.
                                        genBoard.ShowDialog();
                                    }
                                    else
                                    {
                                        Game genBoard = new Game(this.InputRow, this.InputCol, this.GridWidth, this.GridHeight, this.InputMine, this.FirstChecked, false, false, this.FlagColor, this.CellColor, this.NumberColor, this.LuckChecked, this.CharChecked);

                                        // Displays board.
                                        genBoard.ShowDialog();
                                    }
                                }
                                else
                                {
                                    // Display an error.
                                    MessageBox.Show("Invalid amount of mines");

                                    // Re-focus on text box.
                                    CustomMinesText.Text = string.Empty;
                                    CustomMinesText.Focus();
                                }
                            }
                            else
                            {
                                // Display an error.
                                MessageBox.Show("Maximum board size is 35x90");
                                
                                // Re-focus on text box.
                                CustomColsText.Text = string.Empty;
                                CustomColsText.Focus();
                            }
                        }
                        else
                        {
                            // Display an error.
                            MessageBox.Show("Maximum board size is 35x90");

                            // Re-focus on text box.
                            CustomRowsText.Text = string.Empty;
                            CustomRowsText.Focus();
                        }
                    }
                    else
                    {
                        // Display an error.
                        MessageBox.Show("Minimum board size is 4x4");

                        // Refocus on text box.
                        CustomColsText.Text = string.Empty;
                        CustomColsText.Focus();
                    }
                }
                else
                {
                    // Display an error.
                    MessageBox.Show("Minimum board size is 4x4");

                    // Refocus on text box.
                    CustomRowsText.Text = string.Empty;
                    CustomRowsText.Focus();
                }
            }
            else
            {
                // Display an error.
                MessageBox.Show("Please enter valid integers only");
            }
        }

        /// <summary>
        /// Click event for cancel button.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Routed event</param>
        private void CustomCancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Closes form.
            this.Close();
        }

        /// <summary>
        /// Focus event for height text box
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Routed event</param>
        private void CustomHeight_Focus(object sender, RoutedEventArgs e)
        {
            // Place focus on text box when text is deleted.
            this.HasBeenClicked = false;
            if (!this.HasBeenClicked)
            {
                CustomRowsText.Text = string.Empty;
                this.HasBeenClicked = true;
            }
        }

        /// <summary>
        /// Focus event for width text box
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Routed event</param>
        private void CustomWidth_Focus(object sender, RoutedEventArgs e)
        {
            // Place focus on text box when text is deleted.
            this.HasBeenClicked = false;
            if (!this.HasBeenClicked)
            {
                CustomColsText.Text = string.Empty;
                this.HasBeenClicked = true;
            }
        }

        /// <summary>
        /// Focus event for mine count text box
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Routed event</param>
        private void CustomMine_Focus(object sender, RoutedEventArgs e)
        {
            // Place focus on text box when text is deleted.
            this.HasBeenClicked = false;
            if (!this.HasBeenClicked)
            {
                CustomMinesText.Text = string.Empty;
                this.HasBeenClicked = true;
            }
        }
    }
}