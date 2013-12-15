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
            canvasMap.Width = game.Map.Width * Case.SIZE;
            canvasMap.Height = canvasMap.Width;

            Uri bg = new Uri("pack://application:,,,/GraphicInterface;component/Resources/img/map-background.png");
            canvasMap.Background = new ImageBrush(new BitmapImage(bg));

            // Flyweight on textures
            Dictionary<string, ImageBrush> textures = new Dictionary<string, ImageBrush>();
            System.Drawing.Point pt = new System.Drawing.Point();

            for (int i = 0; i < game.Map.Size; i++)
            {
                // Get case type
                pt.X = (i % game.Map.Width);
                pt.Y = (i / game.Map.Width);
                ICase tileType = game.Map.getCase(pt);

                // Draw rectangle at the correct position
                Rectangle tile = new Rectangle();
                tile.Width = Case.SIZE;
                tile.Height = Case.SIZE;

                Canvas.SetTop(tile, pt.Y * Case.SIZE);
                Canvas.SetLeft(tile, pt.X * Case.SIZE);
                Canvas.SetZIndex(tile, 5);
                
                // Set tile texture
                String filename;
                if (tileType is Sea)
                    filename = "sea";
                else if (tileType is Desert)
                    filename = "desert";
                else if (tileType is Forest)
                    filename = "forest";
                else if (tileType is Plain)
                    filename = "plain";
                else
                    filename = "mountain";

                if (!textures.ContainsKey(filename))
                {
                    BitmapImage img = new BitmapImage(new Uri("pack://application:,,,/GraphicInterface;component/Resources/img/terrain/" + filename + ".png"));
                    textures.Add(filename, new ImageBrush(img));
                }
                tile.Fill = textures[filename];
                
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
