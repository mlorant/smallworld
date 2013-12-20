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
        bool inMove = false;
        Unit selectedUnit;
        System.Drawing.Point previous;

        const string IMAGEUNITS = "pack://application:,,,/GraphicInterface;component/Resources/img/units/";

        public GameBoard(Game g)
        {
            game = g;
            InitializeComponent();

            mapGrid.Width = game.Map.Width * Case.SIZE;
            mapGrid.Height = mapGrid.Width;
            for (int i = 0; i < g.Map.Width; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(Case.SIZE, GridUnitType.Pixel);
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(Case.SIZE, GridUnitType.Pixel);
                mapGrid.ColumnDefinitions.Add(col);
                mapGrid.RowDefinitions.Add(row);
            }
            

            // Init players points
            Player1Nickname.Text = game.Players[0].Nickname;
            Player2Nickname.Text = game.Players[1].Nickname;            

            drawMap();
            drawUnits();
            refreshPlayerInfos();

            CurrentRound.Text = game.CurrentRound.ToString(); 
            MaxRound.Text = game.NbRounds.ToString();

        }


        private void drawMap()
        {
            // Clear the current view
            mapGrid.Children.Clear();

            Uri bg = new Uri("pack://application:,,,/GraphicInterface;component/Resources/img/map-background.png");
            mapGrid.Background = new ImageBrush(new BitmapImage(bg));

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

                Grid.SetRow(tile, pt.Y);
                Grid.SetColumn(tile, pt.X);
                Grid.SetZIndex(tile, 5);
                
                // Set tile texture
                String filename = tileType.GetType().Name.ToLower();
                if (!textures.ContainsKey(filename))
                {
                    BitmapImage img = new BitmapImage(new Uri("pack://application:,,,/GraphicInterface;component/Resources/img/terrain/" + filename + ".png"));
                    textures.Add(filename, new ImageBrush(img));
                }
                tile.Fill = textures[filename];

                mapGrid.Children.Add(tile);
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

                String filename = unit.GetType().Name.ToLower();
                BitmapImage img = new BitmapImage(new Uri(IMAGEUNITS + filename + ".png"));
                units.Fill = new ImageBrush(img);

                Grid.SetColumn(units, pt.X);
                Grid.SetRow(units, pt.Y);
                Grid.SetZIndex(units, 10);
                mapGrid.Children.Add(units);
            }
        }

        /// <summary>
        /// Refresh player information (nb units and score)
        /// </summary>
        private void refreshPlayerInfos()
        {
            Player1Units.Text = game.Players[0].Units.Count.ToString();
            Player2Units.Text = game.Players[1].Units.Count.ToString();

            Player1Points.Text = game.Players[0].computePoints().ToString();
            Player2Points.Text = game.Players[1].computePoints().ToString();
        }

        private void moveImageUnit (string nationUnit, System.Drawing.Point sourcePt, System.Drawing.Point destPt){
            // retrieve unit type 
            IUnit unit = game.Map.getUnits(destPt)[0];

            // si l'unité est la première qui ait bougée sur la case
            if (game.Map.getUnits(destPt).Count == 1)
            {
                Rectangle units = new Rectangle();
                units.Width = Case.SIZE;
                units.Height = Case.SIZE;

                Grid.SetColumn(units, destPt.X);
                Grid.SetRow(units, destPt.Y);
                Grid.SetZIndex(units, 10);

                String filename = unit.GetType().Name.ToLower();
                BitmapImage img = new BitmapImage(new Uri(IMAGEUNITS + filename + ".png"));

                units.Fill = new ImageBrush(img);

                mapGrid.Children.Add(units);
            }
            // si c'est la dernière unité sur la case avant d'en partir on la supprime
            if (game.Map.getUnits(sourcePt).Count == 0)
            {
                Rectangle unitPos = mapGrid.Children.Cast<Rectangle>().Last(e => Grid.GetRow(e) == sourcePt.Y && Grid.GetColumn(e) == sourcePt.X);
                mapGrid.Children.Remove(unitPos);
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
            game.endRound();
            CurrentRound.Text = game.CurrentRound.ToString();

            refreshPlayerInfos();
        }

        /// <summary>
        /// Gère le clique sur une case de la carte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clickOnMap(object sender, MouseButtonEventArgs e)
        {
            // Transforme un point de coordonnée pixels en point de cases.
            System.Windows.Point pt = e.GetPosition(mapGrid);
            System.Drawing.Point tile = getPointFromCoordinates(pt);

            // rend visible le cadre d'information sur les unitées
            // et affiche des infos sur le terrain et le nombre d'unité dessus
            CaseInfoOnClick.Visibility = Visibility.Visible;
            CaseInfo.Text = "Type de terrain : " + game.Map.getCase(tile).GetType().Name;
            NbUnitOnCase.Text = game.Map.getUnits(tile).Count + " units camp in this region";
            // Commence par effacer ce qu'il y a dans le cadre pour mettre à jour
            UnitsInfo.Children.Clear();
            // si le clic est un premier clic de selection (pas une attaque ou un déplacement)
            // sinon l'action deplace ou attaque la case visée
            if (!inMove)
            {
                // si il y a des unitées sur la case
                if (game.Map.getUnits(tile).Count > 0)
                {
                    displayUnitsOnCase(tile);                    
                }
            }
            else
            {
                if (game.Map.getUnits(tile).Count > 0 && game.Map.getUnits(tile)[0].GetType() != selectedUnit.GetType())
                {
                    //TODO
                    selectedUnit.attack(tile);
                }
                else
                {
                    if (!selectedUnit.move(tile))
                    {
                        MessageBox.Show("false");
                    }
                    else
                    {
                        this.moveImageUnit(selectedUnit.GetType().ToString(),  previous, tile);
                    }
                    
                }
                inMove = false;
                selectedUnit = null;
            }
        }

        /// <summary>
        /// Display all units in the "tile" case, with information and a select button
        /// </summary>
        /// <param name="tile"></param>
        private void displayUnitsOnCase(System.Drawing.Point tile)
        {
            List<IUnit> listUnit = Game.Instance.Map.getUnits(tile);
            // liste afin de conserver les unités en cas de "back"
            List<StackPanel> panel = new List<StackPanel>();
            Button back = new Button();

            foreach (Unit u in listUnit)
            {
                StackPanel unitAndButton = new StackPanel();
                unitAndButton.Orientation = Orientation.Horizontal;
                TextBlock unitId = new TextBlock();
                Button selectUnit = new Button();
                unitAndButton.Children.Add(unitId);
                unitAndButton.Children.Add(selectUnit);
                // Donne un nom à l'unité
                unitId.Text = u.GetType().Name.ToLower() + " " + u.Id;
                unitId.TextWrapping = TextWrapping.Wrap;

                // ce clique donne plus d'info sur l'unité en question et la selectionne pour attaquer ou bouger
                selectUnit.Content = "Select unit";
                selectUnit.Click += (source, evt) =>
                {
                    UnitsInfo.Children.Clear();
                    TextBlock unitName = new TextBlock();
                    TextBlock unitStat = new TextBlock();

                    unitStat.Text = "Health = " + u.Health
                    + "\nMove left = " + u.MovePoint;

                    selectedUnit = u;
                    inMove = true;
                    previous = tile;

                    UnitsInfo.Children.Add(unitName);
                    UnitsInfo.Children.Add(unitStat);
                    UnitsInfo.Children.Add(back);

                };

                UnitsInfo.Children.Add(unitAndButton);

                panel.Add(unitAndButton);
                inMove = false;
                selectedUnit = null;
            }

            // Un bouton retour pour revenir à la liste des unités sur la case.
            back.Content = "Back";
            back.Click += (s, e2) =>
            {
                inMove = false;
                selectedUnit = null;
                UnitsInfo.Children.Clear();
                foreach (StackPanel sp in panel)
                {
                    UnitsInfo.Children.Add(sp);
                }
            };
        }

    }
}
