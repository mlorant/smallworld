﻿using System;
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
            drawUnits();
            refreshGameInfos();
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
                Grid.SetZIndex(tile, 5);
                
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

            // If the unit is the first to move on the case
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
            // If it's the last unit in the case then we delete it
            if (game.Map.getUnits(sourcePt).Count == 0)
            {
                Rectangle unitPos = mapGrid.Children.Cast<Rectangle>().Last(e => Grid.GetRow(e) == sourcePt.Y && Grid.GetColumn(e) == sourcePt.X);
                mapGrid.Children.Remove(unitPos);
            }
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
            refreshGameInfos();

            if (game.isFinished())
            {
                MessageBox.Show(game.getWinner().Nickname + " won!");
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
            CaseInfo.Text = "Type de terrain : " + game.Map.getCase(tile).GetType().Name;
            NbUnitOnCase.Text = game.Map.getUnits(tile).Count + " units camp in this region";
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
                    // If unit wins the battle (defender lose life and no other units in the case) then she moves 
                    if (selectedUnit.attack(tile) && (game.Map.getUnits(tile).Count == 0))
                    {
                        selectedUnit.move(tile);
                        this.moveImageUnit(selectedUnit.GetType().ToString(), previous, tile);
                    }
                    else
                    {
                        MessageBox.Show("lose or can't attack !");
                    }
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
