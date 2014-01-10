using SmallWorld;
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

namespace GraphicInterface
{
    /// <summary>
    /// Logique d'interaction pour GameEnding.xaml
    /// </summary>
    public partial class GameEnding : UserControl
    {
        public GameEnding(IPlayer winner)
        {
            InitializeComponent();

            IPlayer looser = Game.Instance.Players[(winner == Game.Instance.Players[0]) ? 1 : 0];
            int nbUnitsKilled = Game.Instance.NbUnits - looser.Units.Count;

            PlayerName.Text = winner.Nickname;
            PlayerPoints.Text = winner.computePoints().ToString();
            
            // Unit image
            string unitType = winner.Units[0].GetType().Name.ToString().ToLower();
            Uri bg = new Uri("pack://application:,,,/GraphicInterface;component/Resources/img/portraits/"+unitType+".png");
            UnitImg.Source = new BitmapImage(bg);

            // Check victory condition
            if(looser.Units.Count == 0) // Military victory
                VictoryCondition.Text = "by killing enemy's units.";
            else // Score victory
                VictoryCondition.Text = "";

            UnitsLeft.Text = winner.Units.Count.ToString();
            UnitsKilled.Text = nbUnitsKilled.ToString();
        }


        private void clickReplay(object sender, RoutedEventArgs e)
        {
            // TODO
        }

        /// <summary>
        /// Delete the current window and returns to the start page
        /// </summary>
        private void clickMainMenu(object sender, RoutedEventArgs e)
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
