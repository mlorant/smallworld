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
using System.Threading;

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

        /// <summary>
        /// Flyweight for units textures
        /// </summary>
        Dictionary<string, ImageBrush> unitsTextures = new Dictionary<string, ImageBrush>();

        /// <summary>
        /// Z-index for the background
        /// </summary>
        
        const int BG_INDEX = 1;
        /// <summary>
        /// Z-index for map tiles textures
        /// </summary>
        const int MAP_INDEX = 5;

        /// <summary>
        /// Z-index for units textures
        /// </summary>
        const int UNIT_INDEX = 10;

        /// <summary>
        /// Initialize the game board, by drawing the map,
        /// initial position of units and players information.
        /// </summary>
        /// <param name="g"></param>
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
            

            // Init player nickname and max round (won't change in the game after)
            Player1Nickname.Text = game.Players[0].Nickname;
            Player2Nickname.Text = game.Players[1].Nickname;
            MaxRound.Text = game.NbRounds.ToString();

            // First drawing of the map, units, and game info.
            drawMap();
            drawInitialUnits();
            refreshGameInfos();

            InfoBox.Text = "Bienvenue. C'est à " + game.CurrentPlayer.Nickname + " de commencer à jouer";
        }

        /// <summary>
        /// Draw every tiles of the map, with a flyweigh to
        /// load each tile type image once.
        /// </summary>
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
                Grid.SetZIndex(tile, MAP_INDEX);
                
                // Set tile texture (create it if not used yet)
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
        
        /// <summary>
        /// Drax initial position of units on the map
        /// </summary>
        private void drawInitialUnits()
        {
            for (int i = 0; i < 2; i++)
            {
                // Get unit reference (every units start at the same location)
                IUnit unit = Game.Instance.Players[i].Units[0];
                int nbUnits = Game.Instance.Players[i].Units.Count;

                this.drawUnits(unit.CurrentPosition, unit.GetType(), nbUnits); 
            }
        }

        /// <summary>
        /// Draw units icon on a tile of the map
        /// </summary>
        /// <param name="pt">Map point to update</param>
        /// <param name="unitType">Type of the unit to draw</param>
        /// <param name="nb">Number of unit, to determine the image to draw</param>
        private void drawUnits(System.Drawing.Point pt, Type unitType, int nb)
        {
            // If the tile is now empty, remove the rectangle
            if (nb == 0)
            {
                Rectangle unitPos = mapGrid.Children.Cast<Rectangle>().Last(e => Grid.GetRow(e) == pt.Y && 
                                                                                 Grid.GetColumn(e) == pt.X &&
                                                                                 Grid.GetZIndex(e) == UNIT_INDEX);
                mapGrid.Children.Remove(unitPos);

            }
            else
            {
                // Get unit Rectangle on the grid if already exists
                Rectangle units = mapGrid.Children.Cast<Rectangle>()
                                                  .FirstOrDefault(e => Grid.GetRow(e) == pt.Y &&
                                                                       Grid.GetColumn(e) == pt.X &&
                                                                       Grid.GetZIndex(e) == UNIT_INDEX);
                if (units == null)
                {
                    units = new Rectangle();
                    // Set rectangle size and position
                    units.Width = Case.SIZE;
                    units.Height = Case.SIZE;
                    Grid.SetColumn(units, pt.X);
                    Grid.SetRow(units, pt.Y);
                    // Add to map
                    Grid.SetZIndex(units, UNIT_INDEX);
                    mapGrid.Children.Add(units);
                }

                // Construct the filename of the texture to draw
                String key = unitType.Name.ToLower();
                key += (nb > 1) ? "_multiple" : "";

                // Create new brush if the textures hasn't been used yet
                if (!unitsTextures.ContainsKey(key))
                {
                    BitmapImage img = new BitmapImage(new Uri(IMAGEUNITS + key + ".png"));
                    unitsTextures[key] = new ImageBrush(img);
                }

                units.Fill = unitsTextures[key];
            }
        }

        /// <summary>
        /// Refresh game information: units numbers, scores
        /// and round.
        /// </summary>
        private void refreshGameInfos()
        {
            Player1Units.Text = game.Players[0].Units.Count.ToString();
            Player2Units.Text = game.Players[1].Units.Count.ToString();

            Player1Points.Text = game.Players[0].computePoints().ToString();
            Player2Points.Text = game.Players[1].computePoints().ToString();

            CurrentRound.Text = game.CurrentRound.ToString();
            if (game.CurrentPlayer == game.Players[0])
            {
                Player1Nickname.FontWeight = FontWeights.Black;
                Player2Nickname.FontWeight = FontWeights.Normal;
            }
            else
            {
                Player1Nickname.FontWeight = FontWeights.Normal;
                Player2Nickname.FontWeight = FontWeights.Black;
            }
        }

        /// <summary>
        /// Move the image associates to an unit to another case
        /// </summary>
        /// <param name="nationUnit">String name of the nation</param>
        /// <param name="sourcePt">The case where the unit come from</param>
        /// <param name="destPt">The point where the unit go</param>
        private void moveImageUnit (string nationUnit, System.Drawing.Point sourcePt, System.Drawing.Point destPt){
            // retrieve unit type 
            IUnit unit = game.Map.getUnits(destPt)[0];
            drawUnits(sourcePt, unit.GetType(), game.Map.getUnits(sourcePt).Count);
            drawUnits(destPt, unit.GetType(), game.Map.getUnits(destPt).Count);
        }

        /// <summary>
        /// Translate coorinates in pixel to whole coodinates
        /// </summary>
        /// <param name="p">Point in pixel</param>
        /// <returns>Whole coordinates</returns>
        private System.Drawing.Point getPointFromCoordinates(System.Windows.Point p)
        {
            int x = (int) p.X / Case.SIZE;
            int y = (int) p.Y / Case.SIZE;

            return new System.Drawing.Point(x, y);
        }

        /// <summary>
        /// Action when a player ends its turn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void endRound(object sender, RoutedEventArgs e)
        {
            game.endRound();
            if (game.isFinished())
            {
                MessageBox.Show(game.getWinner().Nickname + " won!");
            }
            else
            {
                refreshGameInfos();

                InfoBox.Text = "";
                int roundsLeft = game.NbRounds - game.CurrentRound;
                if(roundsLeft == 0)
                    InfoBox.Text = "Dernier tour ! ";
                else if (roundsLeft < 3)
                    InfoBox.Text = "Attention, plus que " + (roundsLeft+1) + " tours. ";

                InfoBox.Text += "C'est à " + game.CurrentPlayer.Nickname + " de jouer";
            }
        }

        /// <summary>
        /// Allow to ckick on the map and treats the possibility after it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clickOnMap(object sender, MouseButtonEventArgs e)
        {
            // Translate a point to a case point
            System.Windows.Point pt = e.GetPosition(mapGrid);
            System.Drawing.Point tile = getPointFromCoordinates(pt);

            // Make visible the information frame about units
            // And displays information about the terrain and the number of units above
            CaseInfoOnClick.Visibility = Visibility.Visible;
            CaseInfo.Text = game.Map.getCase(tile).GetType().Name;
            NbUnitOnCase.Text = game.Map.getUnits(tile).Count.ToString();
            // begin to erase all the frame
            UnitsInfo.Children.Clear();
            // If the click is the first click to select a unit
            // otherwise it moves the unit or attacks another
            if (!inMove)
            {
                // if there is units on the case clicked
                if (Game.Instance.Map.getUnits(tile).Count > 0)
                {
                    displayUnitsOnCase(tile);                    
                }
            }
            else
            {
                // if the case clicked have units of the oponent
                if (game.Map.getUnits(tile).Count > 0 && (game.Map.getUnits(tile)[0].GetType() != selectedUnit.GetType()))
                {
                    // Get best defense unit on the tile
                    IUnit defender = Game.Instance.Map.getBestDefensiveUnit(tile);
                    // Run the battle
                    bool wonTheBattle = selectedUnit.attack(defender, tile);
                    // If unit wins the battle then oponent is dead and we verify if case is free
                    // otherwise we check if our unit is alive
                    if (wonTheBattle)
                    {
                        // Get the oponent player
                        IPlayer general = (Game.Instance.Players[0] == Game.Instance.FirstPlayer) ? Game.Instance.Players[1] : Game.Instance.Players[0];
                        // Delete units
                        defender.buryUnit(general, tile);

                        if (Game.Instance.Map.getUnits(tile).Count == 0)
                        {
                            selectedUnit.move(tile);
                            this.moveImageUnit(selectedUnit.GetType().ToString(), previous, tile);
                        }
                    }
                    else
                    {
                        if (!selectedUnit.isAlive())
                        {
                            selectedUnit.buryUnit(Game.Instance.CurrentPlayer, tile);
                            drawUnits(tile, selectedUnit.GetType(), game.Map.getUnits(tile).Count);
                        }
                    }
                    
                }
                else
                {
                    if (!selectedUnit.move(tile))
                    {
                        MessageBox.Show("You can't move here !");
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
                Button selectUnit = new Button();
                unitAndButton.Children.Add(selectUnit);

                // ce clique donne plus d'info sur l'unité en question et la selectionne pour attaquer ou bouger
                selectUnit.Content = u.GetType().Name.ToLower() + " " + u.Id;
                selectUnit.Click += (source, evt) =>
                {
                    UnitsInfo.Children.Clear();
                    TextBlock unitName = new TextBlock();
                    TextBlock unitStat = new TextBlock();

                    unitStat.Text = "Health = " + u.Health
                    + "\nMove left = " + u.MovePoint;

                    selectedUnit = u;
                    if(game.CurrentPlayer.Nation.hasUnit(u))
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


        /// <summary>
        /// Track the cursor position on the map. Allows to move the
        /// scrollbar when the mouse goes in the bottom (or top) of the
        /// map.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hoverOnMap(object sender, MouseEventArgs e)
        {
            Point pt = e.GetPosition(scrollMap);
            double viewSize = scrollMap.ActualHeight;
            int offset = 0;

            // Check if we're either in the top or bottom of the screen
            if (pt.Y < (viewSize * 0.10))
                offset = -5;
            else if ((viewSize - pt.Y) < (viewSize * 0.10))
                offset = 5;

            if (offset != 0)
            {
                scrollMap.ScrollToVerticalOffset(scrollMap.VerticalOffset + offset);
                Thread.Sleep(20); // Wait a short time to avoid to scroll to the top
                                  // (or bottom) directly.
            }
        }

    }
}
