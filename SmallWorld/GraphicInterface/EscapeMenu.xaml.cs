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
    /// Logique d'interaction pour EscapeMenu.xaml
    /// </summary>
    public partial class EscapeMenu : UserControl
    {
        /// <summary>
        /// Function bind on the main window to remove the escape menu 
        /// when click on the "Resume" button.
        /// </summary>
        public Action hideEscapeMenu;

        public EscapeMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Remove the menu and resume the game
        /// </summary>
        private void clickResume(object sender, RoutedEventArgs e)
        {
            this.hideEscapeMenu();
            // Get parent and remove itself
            ((Panel)this.Parent).Children.Remove(this);
        }


        /// <summary>
        /// Load a game file, previously saved
        /// </summary>
        private void loadGame(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.FileName = "myGame";
            dlg.DefaultExt = ".sw";
            dlg.Filter = "SmallWorld save file (.sw)|*.sw";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Game.Instance.restoreGame(dlg.FileName);
                
                // Reload the whole window
                GameBoard main = new GameBoard(Game.Instance);
                App.Current.MainWindow = main;
                Window.GetWindow(this).Close();
                main.Show();
            }
        }


        /// <summary>
        /// Save the current game state in a file
        /// </summary>
        private void saveGame(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
            dlg.FileName = "myGame";
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
        /// Close the game
        /// </summary>
        private void clickExit(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
