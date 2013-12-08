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

                ICase tileType = game.Map.getCase(new SmallWorld.Point(x, y));

                Rectangle tile = new Rectangle();
                tile.Width = 40;
                tile.Height = 30;

                Canvas.SetTop(tile, y * 30);
                Canvas.SetLeft(tile, x * 30);
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

        private void endRound(object sender, RoutedEventArgs e)
        {
            game.CurrentRound += 1;
            CurrentRound.Text = game.CurrentRound.ToString();
        }
    }
}
