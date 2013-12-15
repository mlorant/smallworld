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

            drawMap();
            drawUnits();

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
                String filename = tileType.GetType().Name.ToLower();
                if (!textures.ContainsKey(filename))
                {
                    BitmapImage img = new BitmapImage(new Uri("pack://application:,,,/GraphicInterface;component/Resources/img/terrain/" + filename + ".png"));
                    textures.Add(filename, new ImageBrush(img));
                }
                tile.Fill = textures[filename];
                
                canvasMap.Children.Add(tile);
            }
        }

        private void drawUnits()
        {
            for (int i = 0; i < 2; i++)
            {
                // Get unit reference (every units start at the same location)
                IUnit unit =  Game.Instance.Players[i].Units[0];
                System.Drawing.Point pt = unit.CurrentPosition;

                Rectangle units = new Rectangle();
                units.Width = Case.SIZE;
                units.Height = Case.SIZE;

                Canvas.SetTop(units, pt.Y * Case.SIZE);
                Canvas.SetLeft(units, pt.X * Case.SIZE);
                Canvas.SetZIndex(units, 10);

                String filename = unit.GetType().Name.ToLower();
                BitmapImage img = new BitmapImage(new Uri("pack://application:,,,/GraphicInterface;component/Resources/img/units/" + filename + ".png"));

                units.Fill = new ImageBrush(img);

                canvasMap.Children.Add(units);
            }
        }

        private System.Drawing.Point getPointFromCoordinates(System.Windows.Point p)
        {
            int x = (int) p.X / Case.SIZE;
            int y = (int) p.Y / Case.SIZE;

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
