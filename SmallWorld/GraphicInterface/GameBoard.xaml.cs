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
        private int _player1Points = 7;

        public int player1Points
        {
            get { return _player1Points; }
            set { _player1Points = value;  } 
        }

        public GameBoard(Game g)
        {
            InitializeComponent();
            game = g;

            // Init players points
            Player1Nickname.Text = game.Players[0].Nickname;
            Player2Nickname.Text = game.Players[1].Nickname;            

            // Draw the map
            drawMap();

            CurrentRound.Text = "1";
            MaxRound.Text = game.NbRounds.ToString();
        }


        private void drawMap()
        {
            // Clear the current view
            canvasMap.Children.Clear();

            for (int i = 0; i < game.Map.Size; i++)
            {
                int x = (i % game.Map.Width);
                int y = (i / game.Map.Width);

                ICase tileType = game.Map.getCase(new System.Drawing.Point(x, y));

                Rectangle tile = new Rectangle();
                tile.Width = 50;
                tile.Height = 50;

                Canvas.SetTop(tile, y * 50);
                Canvas.SetLeft(tile, x * 50);
                Canvas.SetZIndex(tile, 5);

                SolidColorBrush mySolidColorBrush = new SolidColorBrush();

                if (tileType is Sea)
                    mySolidColorBrush.Color = Color.FromRgb(135, 196, 250);
                else if (tileType is Desert)
                    mySolidColorBrush.Color = Color.FromRgb(237, 201, 175);
                else if (tileType is Forest)
                    mySolidColorBrush.Color = Color.FromRgb(0, 128, 0);
                else if (tileType is Plain)
                    mySolidColorBrush.Color = Color.FromRgb(0, 255, 0);
                else
                    mySolidColorBrush.Color = Color.FromRgb(100,100,100);
                    

                tile.Fill = mySolidColorBrush;

                canvasMap.Children.Add(tile);
            }
        }

        private System.Drawing.Point getPointFromCoordinates(System.Windows.Point p)
        {
            // TODO : use constant 
            int x = (int) p.X / 50;
            int y = (int) p.Y / 50;

            return new System.Drawing.Point(x, y);
        }

        private void endRound(object sender, RoutedEventArgs e)
        {
            game.CurrentRound += 1;
            CurrentRound.Text = game.CurrentRound.ToString();
        }

        private void clickOnMap(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point pt = e.GetPosition(canvasMap);
            System.Drawing.Point tile = getPointFromCoordinates(pt);

            
            
            foreach(IUnit unit in game.Map.getUnits(tile)) 
            {
                MessageBox.Show(unit.GetType().ToString());
            }
        }
    }
}
