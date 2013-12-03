using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    /// <summary>
    /// Core class of SmallWorld : directs the
    /// game, and let play each player when it's
    /// their turn.
    /// </summary>
    public class Game : IGame
    {
        /// <summary>
        /// Players list
        /// </summary>
        private Player[] players;

        /// <summary>
        /// Index of the current player
        /// </summary>
        private int currentPlayer;

        /// <summary>
        /// Number of the current round
        /// </summary>
        private int currentRound;

        /// <summary>
        /// Index of the winner
        /// </summary>
        private int winner;

        /// <summary>
        /// Map of the current map
        /// </summary>
        private Map map;

        /// <summary>
        /// Builder which gives parameters about the game 
        /// (map size, number of rounds, ...)
        /// </summary>
        private GameCreation builder;

        /// <summary>
        /// Initialize game data
        /// </summary>
        public Game()
        {
            map = new Map();
            players = new Player[2];
        }

        /// <summary>
        /// Set the game Type, according to player choice
        /// </summary>
        /// <param name="type">Type of game: Demo, Small or Normal</param>
        public void setGameType(GameSize type) 
        {
            switch(type)
            {
                case GameSize.DEMO:
                    builder = new DemoGame();
                    break;
                case GameSize.SMALL:
                    builder = new SmallGame();
                    break;
                case GameSize.NORMAL:
                    builder = new NormalGame();
                    break;
            }
        }

        /// <summary>
        /// Init a player according to its configuration
        /// </summary>
        /// <param name="i">Player number</param>
        /// <param name="nickname">Personal nickname</param>
        /// <param name="nation">Nation choosed</param>
        public void initPlayer(int i, string nickname, NationType nation)
        {
            players[i] = new Player(nickname, nation, builder.NB_UNITS);
            // TODO: check unicity of nickname and nation?
        }

        /// <summary>
        /// Generate a new random map with the size defined
        /// by the game type
        /// </summary>
        public void createMap()
        {
            map = new Map();
            map.generateMap(builder.MAP_SIZE);
        }

        public void placePlayers()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Determine who's playing first
        /// </summary>
        public void determinePlayOrder()
        {
            Random random = new Random();
            currentPlayer = random.Next(0, 2);
        }

        /// <summary>
        /// Check if the game is finished or not, according to
        /// the round number and units remaining
        /// </summary>
        /// <returns>A boolean (true if the game is finished)</returns>
        public bool isGameFinished()
        {
            // Check round limit 
            if (currentRound >= builder.NB_ROUNDS)
                return true;

            // Check units number
            return (players[0].getUnits().Length == 0 ||
                     players[1].getUnits().Length == 0);
        }

        public void setWinner()
        {
            if (players[0].getUnits().Length == 0)
            {
                winner = 0;
            }
            else if (players[0].getUnits().Length == 0)
            {
                winner = 1;
            }
            else
            {
                // Compute points of each player and takes the maximum

            }
        }
    }
}
