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
    /// Logique d'interaction pour GameBoard.xaml
    /// </summary>
    public partial class GameBoard : Window
    {
        Game game;

        public GameBoard(Game g)
        {
            InitializeComponent();
            game = g;

            // Init players points
            // TODO

            CurrentRound.Text = "1";
            MaxRound.Text = game.Builder.NB_ROUNDS.ToString();
        }


        private void endRound(object sender, RoutedEventArgs e)
        {
            game.CurrentRound += 1;
            CurrentRound.Text = game.CurrentRound.ToString();
        }
    }
}
