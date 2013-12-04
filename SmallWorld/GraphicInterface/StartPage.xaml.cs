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
using SmallWorld;

namespace GraphicInterface
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class StartPage : Window
    {

        Game game;

        public StartPage()
        {
            InitializeComponent();
            game = new Game();
            game.setGameType(GameSize.NORMAL); //TOFIX
        }
        
        private void startGame(object sender, RoutedEventArgs e)
        {
            // get player info + partie type

            GameBoard main = new GameBoard(game);
            App.Current.MainWindow = main;
            this.Close();
            main.Show();
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
