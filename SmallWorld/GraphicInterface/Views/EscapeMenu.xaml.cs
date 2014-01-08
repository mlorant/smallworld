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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SmallWorld;

namespace GraphicInterface
{
    /// <summary>
    /// Escape Menu view, displays when the user wants 
    /// to save/load or quit the game.
    /// </summary>
    public partial class EscapeMenu : UserControl
    {

        public EscapeMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Remove the menu and resume the game
        /// </summary>
        private void clickResume(object sender, RoutedEventArgs e)
        {
            ((GameBoard)Window.GetWindow(this)).closeEscapeMenu();
        }

        /// <summary>
        /// Save the current game state in a file
        /// </summary>
        private void saveGame(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();

            // Default filename format: player1_VS_player2_date.sw

            dlg.FileName = Game.Instance.Players[0].Nickname + "_VS_"
                            + Game.Instance.Players[1].Nickname + "_"
                            + DateTime.Now.ToString("yyyy-MM-dd");
            dlg.DefaultExt = ".sw";
            dlg.Filter = "SmallWorld save file (.sw)|*.sw";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Game.Instance.saveCurrentGame(dlg.FileName);
                MessageBox.Show("Game saved!");
            }
        }

        /// <summary>
        /// Delete the current window and returns to the start page
        /// </summary>
        private void backToMenu(object sender, RoutedEventArgs e)
        {
            StartPage page = new StartPage();
            App.Current.MainWindow = page;
            Window.GetWindow(this).Close();
            page.Show();
        }

        /// <summary>
        /// Close the game by exiting the main window.
        /// </summary>
        private void clickExit(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
