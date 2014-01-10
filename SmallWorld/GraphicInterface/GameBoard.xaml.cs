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

        EscapeMenu inEscapeMenu = null;

        bool inMove = false;
        IUnit selectedUnit;
        System.Drawing.Point previous;


        /// <summary>
        /// Initialize the game board, by drawing the map,
        /// initial position of units and players information.
        /// </summary>
        /// <param name="g"></param>
        public GameBoard(Game g)
        {
            game = g;
            InitializeComponent();

            // Attack event to display the escape menu
            this.PreviewKeyDown += new KeyEventHandler(HandleKeyboard);
            

            // Init player nickname and max round (won't change in the game after)
            Player1Nickname.Text = game.Players[0].Nickname;
            Player2Nickname.Text = game.Players[1].Nickname;
            MaxRound.Text = game.NbRounds.ToString();

            // First drawing of the map, units, and game info.
            refreshGameInfos();

            InfoBox.Text = "Welcome. It's the turn of " + game.CurrentPlayer.Nickname + ".";
        }

        /// <summary>
        /// Set actions on keyboard (open menu, pass turn, move units...)
        /// </summary>
        private void HandleKeyboard(object sender, KeyEventArgs e)
        {

            switch (e.Key)
            {
                // Show EscapeMenu
                case Key.Escape:
                    if (inEscapeMenu == null)
                        openEscapeMenu();
                    else
                        closeEscapeMenu();
                    break;

                // End of turn
                case Key.Enter:
                    this.endRound(sender, e);
                    break;

                case Key.Up:
                case Key.Left:
                case Key.Right:
                case Key.Down:
                    // Unit move
                    moveUnitKeyboard(e.Key);
                    break;
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
        /// Action when a player ends its turn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void endRound(object sender, RoutedEventArgs e)
        {
            game.endRound();
            if (game.isFinished())
            {
                var ending = new GameEnding(game.Winner);

                Canvas.SetZIndex(ending, 999);
                ending.mainGrid.Width = mainCanvas.ActualWidth;
                ending.mainGrid.Height = mainCanvas.ActualHeight;

                mainCanvas.Children.Add(ending);
            }
            else
            {
                mapViewer.drawHalo();
                refreshGameInfos();
                TileInfo.Visibility = Visibility.Hidden;

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
        /// <param name="tile">Coordinates of the tile clicked</param>
        public void clickOnMap(System.Drawing.Point tile)
        {

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
                            mapViewer.moveImageUnit(previous, tile);
                            InfoBox.Text = "You won the battle! There's no unit left on the tile, so your unit decide to move on it.";
                        }
                        else
                        {
                            IUnit unit = Game.Instance.Map.getUnits(tile)[0];
                            // verify modification on the destination tile (in case they were 2 and now 1)
                            mapViewer.drawUnits(tile, unit.GetType(), Game.Instance.Map.getUnits(tile).Count);
                            InfoBox.Text = "You won the battle!";
                        }
                    }
                    // If defeat, remove player unit
                    else if (!selectedUnit.isAlive())
                    {
                        selectedUnit.buryUnit(Game.Instance.CurrentPlayer, previous);
                        mapViewer.drawUnits(previous, selectedUnit.GetType(), game.Map.getUnits(tile).Count);
                        InfoBox.Text = "Your unit take a fatal hit in the heart. You lost the battle.";
                    }


                }
                else if (tile == previous)
                { // Click on the tile of the unit selected
                    displayUnitsOnCase(tile);
                }
                else
                {
                    if (!selectedUnit.move(tile))
                    {
                        InfoBox.Text = "You can't move here!";
                    }
                    else
                    {
                        mapViewer.moveImageUnit(previous, tile);
                    }

                }
                inMove = false;
                selectedUnit = null;
                mapViewer.cleanSuggestions();
            }
        }


        private void moveUnitKeyboard(Key direction)
        {
            if (selectedUnit != null)
            {
                // simulate a click to move the unit (or battle)
                var moveTo = new System.Drawing.Point(previous.X, previous.Y);
                switch(direction) {
                    case Key.Up:
                        moveTo.Y--;
                        break;
                    case Key.Left:
                        moveTo.X--;
                        break;
                    case Key.Right:
                        moveTo.X++;
                        break;
                    case Key.Down:
                        moveTo.Y++;
                        break;
                }

                int width = Game.Instance.Map.Width;
                if (moveTo.X >= 0 && moveTo.Y >= 0 && moveTo.X < width && moveTo.Y < width)
                    clickOnMap(moveTo);
            }
        }

        /// <summary>
        /// Update the first block of the bottom panel, with tile
        /// selected coords, who controls it and its type.
        /// </summary>
        /// <param name="coord"></param>
        private void updateTileInformation(System.Drawing.Point coord)
        {
            TileInfo.Visibility = Visibility.Visible;

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

            // If there's no unit on the tile, unselect the current unit 
            // and display nothing
            if (listUnit.Count == 0)
            {
                inMove = false;
                selectedUnit = null;
                return;
            }
            
            foreach (Unit u in listUnit)
            {
                UnitSelector unitPanel = new UnitSelector(u);
                // If it's the current player, interaction on click to select the unit
                if (game.CurrentPlayer.Nation.hasUnit(u))
                {
                    unitPanel.MouseLeftButtonDown += (source, evt) =>
                    {
                        this.makeUnitSelected((UnitSelector)source);
                    };
                }

                UnitsInfo.Children.Add(unitPanel);
            }

            // Make the first unit of the tile selected
            UnitSelector selected = (UnitSelector)UnitsInfo.Children[0];
            if (game.CurrentPlayer.Nation.hasUnit(selected.Unit))
            {
                this.makeUnitSelected(selected);
            }
        }

        /// <summary>
        /// Change the unit selected to the one in the selector
        /// </summary>
        /// <param name="unitPanel"></param>
        private void makeUnitSelected(UnitSelector unitPanel)
        {
            mapViewer.cleanSuggestions();
            // Remove any Highlight if a unit is already clicked
            foreach (UnitSelector p in UnitsInfo.Children)
                p.Highlight(false);

            // Check if this unit isn't already selected
            if (selectedUnit != unitPanel.Unit)
            {
                unitPanel.Highlight(true);
                selectedUnit = unitPanel.Unit;

                List<System.Drawing.Point> suggestions = selectedUnit.getSuggestedPoints();
                foreach (System.Drawing.Point pt in suggestions)
                    mapViewer.drawSuggestion(pt);

                inMove = true;
                previous = selectedUnit.CurrentPosition;
            }
            else
            {
                selectedUnit = null;
                inMove = false;
                mapViewer.cleanSuggestions();
            }
        }


        /// <summary>
        /// Open the menu via the menu button
        /// </summary>
        private void clickMenuButton(object sender, RoutedEventArgs e)
        {
            openEscapeMenu();
        }

        /// <summary>
        /// Display the menu in foreground
        /// </summary>
        private void openEscapeMenu()
        {
            inEscapeMenu = new EscapeMenu();

            // Set menu to fill the whole space
            Canvas.SetZIndex(inEscapeMenu, 999);
            inEscapeMenu.mainGrid.Width = mainCanvas.ActualWidth;
            inEscapeMenu.mainGrid.Height = mainCanvas.ActualHeight;

            mainCanvas.Children.Add(inEscapeMenu);
        }


        /// <summary>
        /// Hide the menu and destroy its reference
        /// </summary>
        public void closeEscapeMenu()
        {
            mainCanvas.Children.Remove(inEscapeMenu);
            inEscapeMenu = null;
        }
    }
}

