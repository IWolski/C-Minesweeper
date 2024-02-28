//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Minesweeper Group">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Encapsulation not taught.")]
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Create new checked bool.
        /// </summary>
        public bool EasyChecked;

        /// <summary>
        /// Create new checked bool.
        /// </summary>
        public bool HardChecked;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            // Method call that changes title text color every second.
            this.RotateTextColor();

            // Method call that populates the high score listbox.
            this.GetScore(true);

            // Method call that populates the low score listbox.
            this.GetScore(false);

            // Method call that refreshes the scores in the listbox every second.
            this.RefreshScores();
        }

        /// <summary>
        /// Gets scores from file.
        /// </summary>
        /// <param name="highLow">Determines update of either high or low</param>
        public void GetScore(bool highLow)
        {
            // Streamwriter AppendText called to create a file if one does not exist, then closes it.
            StreamWriter firstCreate = File.AppendText("scores.txt");
            firstCreate.Close();

            // StreamReader variables and list created, adding all scores from the text file to the list.
            StreamReader inScoreFile;

            List<int> scores = new List<int>();

            inScoreFile = File.OpenText("scores.txt");

            int content;

            // Reads file and adds scores to list.
            while (!inScoreFile.EndOfStream)
            {
                content = int.Parse(inScoreFile.ReadLine());
                
                scores.Add(content);
            }

            // Adds high or low scores to list boxes based on called method.
            if (highLow == false)
            {
                // Sorts scores.
                scores.Sort();

                if (scores.Count < 5)
                {
                    for (int i = 0; i < scores.Count; i++)
                    {
                        LowListBox.Items.Add(scores[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        LowListBox.Items.Add(scores[i]);
                    }
                }
            }
            else
            {
                // Sorts scores in opposite direction.
                scores.Sort();
                scores.Reverse();

                if (scores.Count < 5)
                {
                    for (int i = 0; i < scores.Count; i++)
                    {
                        HighListBox.Items.Add(scores[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        HighListBox.Items.Add(scores[i]);
                    }
                }
            }

            // Closes file.
            inScoreFile.Close();
        }

        /// <summary>
        /// Rotates color of title.
        /// </summary>
        private async void RotateTextColor()
        {
            // Create new random variable.
            Random rand = new Random();

            // Infinitely rotates title color.
            while (1 > 0)
            {
                // Delays rotation by 1 second.
                await Task.Delay(1000);

                SolidColorBrush randColor = new SolidColorBrush(Color.FromRgb((byte)rand.Next(255), (byte)rand.Next(255), (byte)rand.Next(255)));
                
                TitleLabel.Foreground = randColor;
            }
        }

        /// <summary>
        /// Refreshes scores every second.
        /// </summary>
        private async void RefreshScores()
        {
            // Infinitely clears text boxes and repopulates them with current info.
            while (1 > 0)
            {
                // Delays re-population by 1 second.
                await Task.Delay(1000);

                HighListBox.Items.Clear();
                LowListBox.Items.Clear();

                this.GetScore(true);

                this.GetScore(false);
            }
        }

        /// <summary>
        /// Click event for StarButton left click.
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="e">Routed Event</param>
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // If one of the computer players is checked then pass board setup to the computer player.
            if (this.EasyChecked == true)
            {
                // Passes easy board settings.
                Game genBoard = new Game(8, 8, 300, 300, 10, false, this.EasyChecked, this.HardChecked, "Red", "Gray", "Black", false, false);

                // Displays board.
                genBoard.ShowDialog();
            }
            else if (this.HardChecked == true)
            {
                Game genBoard = new Game(8, 8, 300, 300, 10, false, this.EasyChecked, this.HardChecked, "Red", "Gray", "Black", false, false);

                // Displays board.
                genBoard.ShowDialog();
            }
            else
            {
                Game genBoard = new Game(8, 8, 300, 300, 10, false, false, false, "Red", "Gray", "Black", false, false);

                // Displays board.
                genBoard.ShowDialog();
            }
        }

        /// <summary>
        /// Click event for ExitButton left click.
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="e">Routed Event</param>
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Closes form.
            this.Close();
        }

        /// <summary>
        /// Click event for option button.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Routed event</param>
        private void OptionButton_Click(object sender, RoutedEventArgs e)
        {
            // Initialize the setup form.
            Setup setupWindow = new Setup(this.EasyChecked, this.HardChecked);

            // Display the setup form.
            setupWindow.ShowDialog();
        }

        /// <summary>
        /// Radio button for easy computer player check event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Routed event</param>
        private void EasyComputerRadio_Checked(object sender, RoutedEventArgs e)
        {
            // Sets the easy checked state to true.
            this.EasyChecked = true;
        }

        /// <summary>
        /// Radio button for easy computer player uncheck event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Routed event</param>
        private void EasyComputerRadio_Unchecked(object sender, RoutedEventArgs e)
        {
            // Sets the easy checked state to false.
            this.EasyChecked = false;
        }

        /// <summary>
        /// Radio button for hard computer player check event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Routed event</param>
        private void HardComputerRadio_Checked(object sender, RoutedEventArgs e)
        {
            // Sets the hard checked state to true.
            this.HardChecked = true;
        }

        /// <summary>
        /// Radio button for hard computer player uncheck event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Routed event</param>
        private void HardComputerRadio_Unchecked(object sender, RoutedEventArgs e)
        {
            // Sets the hard checked state to false.
            this.HardChecked = false;
        }
    }
}