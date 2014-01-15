using SmallWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace GraphicInterface
{
    /// <summary>
    /// Logique d'interaction pour MapViewer.xaml
    /// </summary>
    public partial class MapViewer : UserControl
    {

        // Base folder for units images
        const string IMAGEUNITS = "pack://application:,,,/GraphicInterface;component/Resources/img/units/";

        // Z-index definitions
        const int BG_INDEX = 1;         // Background
        const int MAP_INDEX = 5;        // Tiles environnement textures
        const int SUGG_INDEX = 6;       // Blank rectangle for suggestion
        const int HALO_INDEX = 7;       // Halo under units when current player
        const int UNIT_INDEX = 10;      // Unit image

        // Flyweight textures
        Dictionary<string, ImageBrush> unitsTextures = new Dictionary<string, ImageBrush>();
        ImageBrush haloImage = new ImageBrush(new BitmapImage(new Uri(IMAGEUNITS + "halo.png")));


        /// <summary>
        /// Initialize a new map view and define the grid with
        /// the board size in the current Game
        /// </summary>
        public MapViewer()
        {
            InitializeComponent();

            // Avoid crash in Visual Studio with the design mode. Otherwise, VS
            // tries to call this constructor when there no game defined...
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            // Initialization of the map grid
            mapGrid.Width = Game.Instance.Map.Width * Case.SIZE;
            mapGrid.Height = mapGrid.Width;

            // Define the correct number of column with a size defined in the model
            for (int i = 0; i < Game.Instance.Map.Width; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(Case.SIZE, GridUnitType.Pixel);
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(Case.SIZE, GridUnitType.Pixel);
                mapGrid.ColumnDefinitions.Add(col);
                mapGrid.RowDefinitions.Add(row);
            }

            // Draw initial data
            drawMap();
            drawInitialUnits();
            drawHalo();
        }



        /// <summary>
        /// Move the image associates to an unit to another case
        /// </summary>
        /// <param name="sourcePt">The case where the unit come from</param>
        /// <param name="destPt">The point where the unit go</param>
        public void moveImageUnit(System.Drawing.Point sourcePt, System.Drawing.Point destPt)
        {
            // retrieve unit type 
            IUnit unit = Game.Instance.Map.getUnits(destPt)[0];
            drawUnits(sourcePt, unit.GetType(), Game.Instance.Map.getUnits(sourcePt).Count);
            drawUnits(destPt, unit.GetType(), Game.Instance.Map.getUnits(destPt).Count);
            // Refresh halos
            drawHalo();
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

            for (int i = 0; i < Game.Instance.Map.Size; i++)
            {
                // Get case type
                pt.X = (i % Game.Instance.Map.Width);
                pt.Y = (i / Game.Instance.Map.Width);
                ICase tileType = Game.Instance.Map.getCase(pt);

                // Draw square at the correct position
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
                foreach (IUnit unit in Game.Instance.Players[i].Units)
                {
                    if (unitsPositions.ContainsKey(unit.CurrentPosition))
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
        public void drawUnits(System.Drawing.Point pt, Type unitType, int nb)
        {
            // If the tile is now empty, remove the rectangle
            if (nb == 0)
            {
                // Remove unit icon
                Rectangle rect = mapGrid.Children.Cast<Rectangle>().Last(e => Grid.GetRow(e) == pt.Y &&
                                                                              Grid.GetColumn(e) == pt.X &&
                                                                              Grid.GetZIndex(e) == UNIT_INDEX);
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
            }
        }

        /// <summary>
        /// Draws halo under each 
        /// </summary>
        public void drawHalo()
        {
            // Erase every halos already present
            List<Rectangle> rects = mapGrid.Children.Cast<Rectangle>().Where(e => Grid.GetZIndex(e) == HALO_INDEX).ToList();
            foreach (Rectangle r in rects)
                mapGrid.Children.Remove(r);


            // Get positions of every units
            HashSet<System.Drawing.Point> points = new HashSet<System.Drawing.Point>();
            foreach (Unit unit in Game.Instance.CurrentPlayer.Units)
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

        public void drawSuggestion(System.Drawing.Point pt, MoveType moveType)
        {
            Rectangle sugg = new Rectangle();
            
            Grid.SetColumn(sugg, pt.X);
            Grid.SetRow(sugg, pt.Y);
            Grid.SetZIndex(sugg, SUGG_INDEX);
            
            Color color = (moveType == MoveType.Suggested) ? Colors.Honeydew : Colors.PowderBlue;

            sugg.Stroke = new SolidColorBrush(color);
            sugg.StrokeThickness = 2.25;

            mapGrid.Children.Add(sugg);
        }

        public void cleanSuggestions()
        {
            List<Rectangle> rects = mapGrid.Children.Cast<Rectangle>().Where(e => Grid.GetZIndex(e) == SUGG_INDEX).ToList();
            foreach (Rectangle r in rects)
                mapGrid.Children.Remove(r);
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

        private void clickOnMap(object sender, MouseButtonEventArgs e) {
            // Get coordinates of the click
            var rect = (UIElement)e.Source;
            int row = Grid.GetRow(rect);
            int col = Grid.GetColumn(rect);

            var tile = new System.Drawing.Point(col, row);

            ((GameBoard)Window.GetWindow(this)).clickOnMap(tile);
        }
    }
}
