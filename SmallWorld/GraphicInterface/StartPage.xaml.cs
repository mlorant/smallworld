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

        private Game game;
        private string gameType = null;
        private string player1, player2;
        private NationType nation1, nation2;
        private string nationType1, nationType2;

        public StartPage()
        {
            InitializeComponent();
        }


        private void selectGameMode(object sender, RoutedEventArgs e)
        {
            this.gameType = (sender as RadioButton).Content.ToString();
            gridInfoPlayer.IsEnabled = true;
        }

        /// <summary>
        /// Convert the string to a NationType
        /// </summary>
        /// <param name="content">The string to convert</param>
        /// <returns>a NationType</returns>
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

        /// <summary>
        /// Give the nation choosen to the player 1 and disable the same nation for the other player
        /// </summary>
        /// <param name="sender">Object related to the radioButton clicked</param>
        /// <param name="e"></param>
        private void initNationPlayer1(object sender, RoutedEventArgs e) 
        {
            RadioButton sender1 = (sender as RadioButton);
            nationType1 = sender1.Content.ToString();
            nation1 = initNation(nationType1);
           
            Dwarf1.ClearValue(TextBox.BorderBrushProperty);
            Gallic1.ClearValue(TextBox.BorderBrushProperty);
            Viking1.ClearValue(TextBox.BorderBrushProperty);
        }

        /// <summary>
        /// Give the nation choosen to the player 2 and disable the same nation for the other player
        /// </summary>
        /// <param name="sender">Object related to the radioButton clicked</param>
        /// <param name="e"></param>
        private void initNationPlayer2(object sender, RoutedEventArgs e)
        {
            nationType2 = (sender as RadioButton).Content.ToString();
            nation2 = initNation(nationType2);
        
            Dwarf2.ClearValue(TextBox.BorderBrushProperty);
            Gallic2.ClearValue(TextBox.BorderBrushProperty);
            Viking2.ClearValue(TextBox.BorderBrushProperty);
        }

        /// <summary>
        /// Verify if the player 1 has filled the nicknamed textbox
        /// if not he focuses the textbox and border it in Red
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void verifyInfo1(object sender, RoutedEventArgs e)
        {
            player1 = Player1Nickname.Text;

            Player1Nickname.ClearValue(TextBox.BorderBrushProperty);
            
            if (player1 == null || player1 == "")
            {
                Player1Nickname.BorderBrush = Brushes.Red;
                InfoP1.IsChecked = false;
            }
            if (nationType1 == null)
            {
                Dwarf1.BorderBrush = Brushes.Red;
                Gallic1.BorderBrush = Brushes.Red;
                Viking1.BorderBrush = Brushes.Red;
                InfoP1.IsChecked = false;
            }
        }

        /// <summary>
        /// Verify if the player 2 has filled the nicknamed textbox
        /// if not he focuses the textbox and border it in Red
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void verifyInfo2(object sender, RoutedEventArgs e)
        {
            player2 = Player2Nickname.Text;

            Player2Nickname.ClearValue(TextBox.BorderBrushProperty);

            if (player2 == null || player2 == "")
            {
                Player2Nickname.BorderBrush = Brushes.Red;
                InfoP2.IsChecked = false;
            }
            if (nationType2 == null)
            {
                Dwarf2.BorderBrush = Brushes.Red;
                Gallic2.BorderBrush = Brushes.Red;
                Viking2.BorderBrush = Brushes.Red;
                InfoP2.IsChecked = false;
            }          
        }

        private void startGame(object sender, RoutedEventArgs e)
        {
            GameCreation builder = null;

            // Create Game object
            switch (gameType)
            {
                case "Demo":
                    builder = new DemoGameBuilder();
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

            game = builder.createGame();
            game.initPlayer(0, player1, nation1);
            game.initPlayer(1, player2, nation2);

            GameBoard main = new GameBoard(game);
            App.Current.MainWindow = main;
            this.Close();
            main.Show();
        }
    }
}
