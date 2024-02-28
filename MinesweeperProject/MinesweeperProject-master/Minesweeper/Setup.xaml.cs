//-----------------------------------------------------------------------
// <copyright file="Setup.xaml.cs" company="Minesweeper Group">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
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
    /// Interaction logic for Setup.xaml.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Encapsulation not taught.")]
    public partial class Setup : Window
    {
        /// <summary>
        /// Create new checked bool.
        /// </summary>
        public bool FirstChecked;

        /// <summary>
        /// Create new easy AI checked bool.
        /// </summary>
        public bool EasyChecked;

        /// <summary>
        /// Create new hard AI checked bool.
        /// </summary>
        public bool HardChecked;

        /// <summary>
        /// Create new flag color checked state.
        /// </summary>
        public string FlagColor = "Red";

        /// <summary>
        /// Create new cell color checked state.
        /// </summary>
        public string CellColor = "Gray";

        /// <summary>
        /// Create new number color checked state.
        /// </summary>
        public string NumberColor = "Black";

        /// <summary>
        /// Create bool to luck checked.
        /// </summary>
        public bool LuckChecked;

        /// <summary>
        /// Create new character checked state.
        /// </summary>
        public bool CharChecked;

        /// <summary>
        /// Initializes a new instance of the <see cref="Setup" /> class.
        /// </summary>
        /// <param name="easy">Parameter for easy AI state</param>
        /// <param name="hard">Parameter for hard AI state</param>
        public Setup(bool easy, bool hard)
        {
            this.InitializeComponent();

            // Assigns variable states to fields.
            this.EasyChecked = easy;

            this.HardChecked = hard;
        }

        /// <summary>
        /// Click event for BackButton.
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="e">Routed Event</param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Closes the form.
            this.Close();
        }

        /// <summary>
        /// Click event for AboutButton.
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="e">Routed Event</param>
        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            // Displays information.
            string aboutMessage = "Version: 5.0 \nVersion Date: 3/8/23\nDevelopers: Indira, Bryce, Alex, Nick\nAudio Credit: https://mixkit.co";
            MessageBox.Show(aboutMessage, "About");
        }

        /// <summary>
        /// Click event for EasyButton.
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="e">Routed Event</param>
        private void EasyButton_Click(object sender, RoutedEventArgs e)
        {
            // If one of the computer players is checked then pass board setup to the computer player.
            if (this.EasyChecked == true)
            {
                // Passes easy board settings.
                Game genBoard = new Game(8, 8, 300, 300, 10, this.FirstChecked, this.EasyChecked, this.HardChecked, this.FlagColor, this.CellColor, this.NumberColor, this.LuckChecked, this.CharChecked);

                // Displays board.
                genBoard.ShowDialog();
            }
            else if (this.HardChecked == true)
            {
                Game genBoard = new Game(8, 8, 300, 300, 10, this.FirstChecked, this.EasyChecked, this.HardChecked, this.FlagColor, this.CellColor, this.NumberColor, this.LuckChecked, this.CharChecked);

                // Displays board.
                genBoard.ShowDialog();
            }
            else
            {
                Game genBoard = new Game(8, 8, 300, 300, 10, this.FirstChecked, false, false, this.FlagColor, this.CellColor, this.NumberColor, this.LuckChecked, this.CharChecked);

                // Displays board.
                genBoard.ShowDialog();
            }
        }

        /// <summary>
        /// Click event for MediumButton.
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="e">Routed Event</param>
        private void MediumButton_Click(object sender, RoutedEventArgs e)
        {
            // If one of the computer players is checked then pass board setup to the computer player.
            if (this.EasyChecked == true)
            {
                // Passes easy board settings.
                Game genBoard = new Game(16, 16, 599, 599, 40, this.FirstChecked, this.EasyChecked, this.HardChecked, this.FlagColor, this.CellColor, this.NumberColor, this.LuckChecked, this.CharChecked);

                // Displays board.
                genBoard.ShowDialog();
            }
            else if (this.HardChecked == true)
            {
                Game genBoard = new Game(16, 16, 599, 599, 40, this.FirstChecked, this.EasyChecked, this.HardChecked, this.FlagColor, this.CellColor, this.NumberColor, this.LuckChecked, this.CharChecked);

                // Displays board.
                genBoard.ShowDialog();
            }
            else
            {
                Game genBoard = new Game(16, 16, 599, 599, 40, this.FirstChecked, false, false, this.FlagColor, this.CellColor, this.NumberColor, this.LuckChecked, this.CharChecked);

                // Displays board.
                genBoard.ShowDialog();
            }
        }

        /// <summary>
        /// Click event for HardButton.
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="e">Routed Event</param>
        private void HardButton_Click(object sender, RoutedEventArgs e)
        {
            // If one of the computer players is checked then pass board setup to the computer player.
            if (this.EasyChecked == true)
            {
                // Passes easy board settings.
                Game genBoard = new Game(16, 30, 1150, 599, 99, this.FirstChecked, this.EasyChecked, this.HardChecked, this.FlagColor, this.CellColor, this.NumberColor, this.LuckChecked, this.CharChecked);

                // Displays board.
                genBoard.ShowDialog();
            }
            else if (this.HardChecked == true)
            {
                Game genBoard = new Game(16, 30, 1150, 599, 99, this.FirstChecked, this.EasyChecked, this.HardChecked, this.FlagColor, this.CellColor, this.NumberColor, this.LuckChecked, this.CharChecked);

                // Displays board.
                genBoard.ShowDialog();
            }
            else
            {
                Game genBoard = new Game(16, 30, 1150, 599, 99, this.FirstChecked, false, false, this.FlagColor, this.CellColor, this.NumberColor, this.LuckChecked, this.CharChecked);

                // Displays board.
                genBoard.ShowDialog();
            }
        }

        /// <summary>
        /// Click event for CustomButton.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Routed Event</param>/
        private void CustomButton_Click(object sender, RoutedEventArgs e)
        {
            // Creates new custom options window.
            CustomInput customInput = new CustomInput(this.FirstChecked, this.EasyChecked, this.HardChecked, this.FlagColor, this.CellColor, this.NumberColor, this.LuckChecked, this.CharChecked);

            // Displays custom options window.
            customInput.ShowDialog();
        }

        /// <summary>
        /// Checkbox FirstChecked Event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Routed Event</param>/
        private void FirstClickCheck_Checked(object sender, RoutedEventArgs e)
        {
            // Sets checked state to true.
            this.FirstChecked = true;
        }

        /// <summary>
        /// Checkbox Unchecked Event.
        /// </summary>
        /// <param name="sender">Object Sender</param>
        /// <param name="e">Routed Event</param>/
        private void FirstClickCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            // Sets checked state to false.
            this.FirstChecked = false;
        }
       
        /// <summary>
        /// Combo box for selected Flag color.
        /// </summary>
        /// <param name="sender">Object Sender</param>
        /// <param name="e">Routed Event</param>/
        private void FlagColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Assigns flag color based on picked index.
            if (FlagColorComboBox.SelectedIndex == 0)
            {
                this.FlagColor = "Red";
            }
            else if (FlagColorComboBox.SelectedIndex == 1)
            {
                this.FlagColor = "Yellow";
            }
            else if (FlagColorComboBox.SelectedIndex == 2)
            {
                this.FlagColor = "Green";
            }
            else if (FlagColorComboBox.SelectedIndex == 3)
            {
                this.FlagColor = "Blue";
            }
        }
       
        /// <summary>
        /// Combo Box for Cell Selected color.
        /// </summary>
        /// <param name="sender">Object Sender</param>
        /// <param name="e">Routed Event</param>
        private void CellColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Assigns cell color based on picked index.
            if (CellColorComboBox.SelectedIndex == 0)
            {
                this.CellColor = "Gray";
            }
            else if (CellColorComboBox.SelectedIndex == 1)
            {
                this.CellColor = "Cyan";
            }
            else if (CellColorComboBox.SelectedIndex == 2)
            {
                this.CellColor = "Yellow";
            }
            else if (CellColorComboBox.SelectedIndex == 3)
            {
                this.CellColor = "Pink";
            }
        }

        /// <summary>
        /// Checkbox for choice of color of number.
        /// </summary>
        /// <param name="sender">Object Sender</param>
        /// <param name="e">Routed Event</param>/
        private void NumberColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If statements for different choices of number color based on selected index.
            if (this.NumberColorComboBox.SelectedIndex == 0)
            {
                this.NumberColor = "Black";
            }
            else if (this.NumberColorComboBox.SelectedIndex == 1)
            {
                this.NumberColor = "Orange";
            }
            else if (this.NumberColorComboBox.SelectedIndex == 2)
            {
                this.NumberColor = "Green";
            }
            else if (this.NumberColorComboBox.SelectedIndex == 3)
            {
                this.NumberColor = "Purple";
            }
        }

        /// <summary>
        /// Boolean for Luck Checkbox, true.
        /// </summary>
        /// <param name="sender">Object Sender</param>
        /// <param name="e">Routed Event</param>/
        private void LuckCheck_Checked(object sender, RoutedEventArgs e)
        {
            // Sets luck check state to true.
            this.LuckChecked = true;
        }

        /// <summary>
        /// Boolean for Luck Checkbox, false.
        /// </summary>
        /// <param name="sender">Object Sender</param>
        /// <param name="e">Routed Event</param>/
        private void LuckCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            // Sets luck check state to false.
            this.LuckChecked = false;
        }

        /// <summary>
        /// Checkbox for special characters, true. Displaying message box of special characters. 
        /// </summary>
        /// <param name="sender">Object Sender</param>
        /// <param name="e">Routed Event</param>/
        private void CharCheck_Checked(object sender, RoutedEventArgs e)
        {
            // Displays key for special characters.
            MessageBox.Show("% = 1\n* = 2\n^ = 3\n! = 4\n# = 5\n& = 6\n$ = 7\n@ = 8");

            // Sets character check state to true.
            this.CharChecked = true;
        }

        /// <summary>
        /// Checkbox for special characters, false.
        /// </summary>
        /// <param name="sender">Object Sender</param>
        /// <param name="e">Routed Event</param>/
        private void CharCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            // Sets character check state to false. 
            this.CharChecked = false;
        }
    }
}