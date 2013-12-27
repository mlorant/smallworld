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
using GraphicInterface.GameInformation;

namespace GraphicInterface
{

    /// <summary>
    /// Logique d'interaction pour GameBoard.xaml
    /// </summary>
    public partial class GameBoard : Window
    {
        Game game;

        bool inEscapeMenu = false;

        bool inMove = false;
        Unit selectedUnit;
        System.Drawing.Point previous;

        const string IMAGEUNITS = "pack://application:,,,/GraphicInterface;component/Resources/img/units/";

        /// <summary>
        /// Flyweight for units textures
        /// </summary>
        Dictionary<string, ImageBrush> unitsTextures = new Dictionary<string, ImageBrush>();
        ImageBrush haloImage = new ImageBrush(new BitmapImage(new Uri(IMAGEUNITS + "halo.png")));

        // Z-index definitions
        const int BG_INDEX = 1;
        const int MAP_INDEX = 5;
        const int HALO_INDEX = 7;
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

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);

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
            drawHalo();
            refreshGameInfos();

            InfoBox.Text = "Welcome. It's the turn of " + game.CurrentPlayer.Nickname + ".";
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
                // Type is the same for every unit
                Type unitType = Game.Instance.Players[i].Units[0].GetType();

                // Make a dictionnary of points, associated to number of units on it
                var unitsPositions = new Dictionary<System.Drawing.Point, int>();
                foreach(IUnit unit in Game.Instance.Players[i].Units) 
                {
                    if(unitsPositions.ContainsKey(unit.CurrentPosition)) 
                        unitsPositions[unit.CurrentPosition]++;
                    else
                        unitsPositions[unit.CurrentPosition] = 1;
                }
                
                // Draw each positions
                foreach (KeyValuePair<System.Drawing.Point, int> pair in unitsPositions)
                    this.drawUnits(pair.Key, unitType, pair.Value); 
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
                // Remove unit icon
                Rectangle rect = mapGrid.Children.Cast<Rectangle>().Last(e => Grid.GetRow(e) == pt.Y && 
                                                                              Grid.GetColumn(e) == pt.X &&
                                                                              Grid.GetZIndex(e) == UNIT_INDEX);
                mapGrid.Children.Remove(rect);
                // Remove halo
                rect = mapGrid.Children.Cast<Rectangle>().Last(e => Grid.GetRow(e) == pt.Y && 
                                                                    Grid.GetColumn(e) == pt.X &&
                                                                    Grid.GetZIndex(e) == HALO_INDEX);
                mapGrid.Children.Remove(rect);
            }
            else
            {
                // Get unit Rectangle on the grid if already exists
                Rectangle units = mapGrid.Children.Cast<Rectangle>()
                                                  .FirstOrDefault(e => Grid.GetRow(e) == pt.Y &&
                                                                       Grid.GetColumn(e) == pt.X &&
                                                                       Grid.GetZIndex(e) == UNIT_INDEX);

                // No units here, draw halo + new unit
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

                // Add halo if necessary
                Rectangle rect = mapGrid.Children.Cast<Rectangle>()
                                                  .FirstOrDefault(e => Grid.GetRow(e) == pt.Y &&
                                                                       Grid.GetColumn(e) == pt.X &&
                                                                       Grid.GetZIndex(e) == HALO_INDEX);
                if(rect == null)
                    mapGrid.Children.Add(getHaloRectangle(pt));
            }
        }

        /// <summary>
        /// Draws halo under each 
        /// </summary>
        private void drawHalo()
        {
            // Erase every halos already present
            List<Rectangle> rects = mapGrid.Children.Cast<Rectangle>().Where(e => Grid.GetZIndex(e) == HALO_INDEX).ToList();
            foreach (Rectangle r in rects)
                mapGrid.Children.Remove(r);


            // Get positions of every units
            HashSet<System.Drawing.Point> points = new HashSet<System.Drawing.Point>();
            foreach (Unit unit in game.CurrentPlayer.Units)
            {
                if (!points.Contains(unit.CurrentPosition)) // works by comparaison
                    points.Add(unit.CurrentPosition);
            }

            // Draw halo on each points
            foreach (System.Drawing.Point pt in points)
            {
                mapGrid.Children.Add(getHaloRectangle(pt));
            }
        }

        private Rectangle getHaloRectangle(System.Drawing.Point destination)
        {
            Rectangle halo = new Rectangle();
            Grid.SetColumn(halo, destination.X);
            Grid.SetRow(halo, destination.Y);
            Grid.SetZIndex(halo, HALO_INDEX);
            halo.Fill = this.haloImage;

            return halo;
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
        /// <param name="sourcePt">The case where the unit come from</param>
        /// <param name="destPt">The point where the unit go</param>
        private void moveImageUnit (System.Drawing.Point sourcePt, System.Drawing.Point destPt){
            // retrieve unit type 
            IUnit unit = game.Map.getUnits(destPt)[0];
            drawUnits(sourcePt, unit.GetType(), game.Map.getUnits(sourcePt).Count);
            drawUnits(destPt, unit.GetType(), game.Map.getUnits(destPt).Count);
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
                drawHalo();
                refreshGameInfos();

                InfoBox.Text = "";
                int roundsLeft = game.NbRounds - game.CurrentRound;
                if(roundsLeft == 0)
                    InfoBox.Text = "Last turn! ";
                else if (roundsLeft < 3)
                    InfoBox.Text = "Watch out, " + (roundsLeft+1) + " turns left. ";

                InfoBox.Text += "It's the turn of " + game.CurrentPlayer.Nickname + ".";
            }
        }

        /// <summary>
        /// Allow to click on the map and treats the possibility after it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clickOnMap(object sender, MouseButtonEventArgs e)
        {
            // Get coordinates of the click
            var rect = (UIElement) e.Source;
            int row = Grid.GetRow(rect);
            int col = Grid.GetColumn(rect);

            var tile = new System.Drawing.Point(col, row);

            // Make visible the information frame about units
            // And displays information about the terrain and the number of units above
            UnitsInformation.Visibility = Visibility.Visible;
            updateTileInformation(tile);

            // begin to erase all the frame
            UnitsInfo.Children.Clear();
            // If the click is the first click to select a unit
            // otherwise it moves the unit or attacks another
            if (!inMove)
            {
                // if there is units on the case clicked
                if (game.Map.getUnits(tile).Count > 0)
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
                        IPlayer general = (Game.Instance.Players[0] == Game.Instance.CurrentPlayer) ? Game.Instance.Players[1] : Game.Instance.Players[0];
                        // Delete units
                        defender.buryUnit(general, tile);                        

                        if (Game.Instance.Map.getUnits(tile).Count == 0)
                        {
                            selectedUnit.move(tile);
                            this.moveImageUnit(previous, tile);
                        }
                        else
                        {
                            IUnit unit = Game.Instance.Map.getUnits(tile)[0];
                            // verify modification on the destination tile (in case they were 2 and now 1)
                            drawUnits(tile, unit.GetType(), Game.Instance.Map.getUnits(tile).Count);
                        }
                    }
                    // If defeat, remove player unit
                    else if (!selectedUnit.isAlive())
                    {
                        selectedUnit.buryUnit(Game.Instance.CurrentPlayer, previous);
                        drawUnits(previous, selectedUnit.GetType(), game.Map.getUnits(tile).Count);
                    }
                    
                    
                }
                else if(tile == previous) { // Click on the tile of the unit selected
                    displayUnitsOnCase(tile);
                }
                else
                {
                    if (!selectedUnit.move(tile))
                    {
                        MessageBox.Show("You can't move here !");
                    }
                    else
                    {
                        this.moveImageUnit(previous, tile);
                    }
                    
                }
                inMove = false;
                selectedUnit = null;
            }
        }


        private void updateTileInformation(System.Drawing.Point coord)
        {
            TileType.Text = game.Map.getCase(coord).GetType().Name;
            TileX.Text = coord.X.ToString();
            TileY.Text = coord.Y.ToString();

            List<IUnit> tileUnits = game.Map.getUnits(coord);
            NbUnitOnCase.Text = tileUnits.Count.ToString();

            if (tileUnits.Count == 0)
                TileController.Text = "nobody";
            else if (Game.Instance.Players[0].Nation.hasUnit(tileUnits[0]))
                TileController.Text = Game.Instance.Players[0].Nickname;
            else
                TileController.Text = Game.Instance.Players[1].Nickname;
        }

        /// <summary>
        /// Display all units in the "tile" case, with information and a select button
        /// </summary>
        /// <param name="tile"></param>
        private void displayUnitsOnCase(System.Drawing.Point tile)
        {
            List<IUnit> listUnit = Game.Instance.Map.getUnits(tile);
            inMove = false;
            selectedUnit = null;

            foreach (Unit u in listUnit)
            {

                UnitSelector unitPanel = new UnitSelector(u);
                // If it's the current player, interaction on click to select the unit
                if (game.CurrentPlayer.Nation.hasUnit(u))
                {
                    unitPanel.MouseLeftButtonDown += (source, evt) =>
                    {
                        // Remove any Highlight if a unit is already clicked
                        foreach (UnitSelector p in UnitsInfo.Children)
                            p.Highlight(false);

                        // Check if this unit isn't already selected
                        if (selectedUnit != u)
                        {
                            unitPanel.Highlight(true);
                            selectedUnit = u;

                            inMove = true;
                            previous = tile;
                        }
                        else
                        {
                            selectedUnit = null;
                            inMove = false;
                        }
                        
                    };
                }

                UnitsInfo.Children.Add(unitPanel);
            }
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


        private void HandleEsc(object sender, KeyEventArgs e)
        {
            // Show EscapeMenu
            if (e.Key == Key.Escape && !inEscapeMenu)
            {
                inEscapeMenu = true;

                EscapeMenu menu = new EscapeMenu();
                menu.hideEscapeMenu = () => { this.inEscapeMenu = false; };

                // Set menu to fill the whole space
                Canvas.SetZIndex(menu, 999);
                menu.mainGrid.Width = mainCanvas.ActualWidth;
                menu.mainGrid.Height = mainCanvas.ActualHeight;

                mainCanvas.Children.Add(menu);
            }
        }

    }
}

