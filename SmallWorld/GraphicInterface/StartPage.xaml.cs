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
        private string gameType;

        private GameSize gameTypeSelected;
        private NationType nation1, nation2;

        public StartPage()
        {
            InitializeComponent();
            game = new Game();
        }

        private void selectGameMode(object sender, RoutedEventArgs e)
        {
            this.gameType = (sender as RadioButton).Content.ToString(); 
        }

        private NationType initNation(string content)
        {
            NationType nation = 0;
            switch (content)
            {
                case "Dwarf":
                    nation = NationType.DWARF;
                    break;
                case "Gallic":
                    nation = NationType.GALLIC;
                    break;
                case "Viking":
                    nation = NationType.VIKING;
                    break;
            }
            return nation;
        }

        private void initNationPlayer1(object sender, RoutedEventArgs e) 
        {
            RadioButton sender1 = (sender as RadioButton);
            string nationType = (sender1.Content.ToString();
            nation1 = initNation(nationType);
        }

        private void initNationPlayer2(object sender, RoutedEventArgs e)
        {
            string nationType = (sender as RadioButton).Content.ToString();
            nation2 = initNation(nationType);
        }

       
        private void startGame(object sender, RoutedEventArgs e)
        {
            GameCreation builder = null;

            // Create Game object
            switch (gameType)
            {
                case "Demo":
                    builder = new SmallGameBuilder();
                    break;
                case "Normal":
                    builder = new NormalGameBuilder();
                    break;
                case "Small":
                    builder = new SmallGameBuilder();
                    break;
                default:
                    throw new Exception("Unexpected game mode");
            }

            Game game = builder.createGame();
            
            GameBoard main = new GameBoard(game);
            App.Current.MainWindow = main;
            this.Close();
            main.Show();
        }
    }
}
