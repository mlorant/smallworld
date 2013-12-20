using System;
using System.Collections.Generic;
using System.Drawing;
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
        private static Game instance;

        /// <summary>
        /// Players list
        /// </summary>
        private Player[] players;

        /// <summary>
        /// First player who plays
        /// </summary>
        private Player firstPlayer;

        /// <summary>
        /// Current player
        /// </summary>
        private Player currentPlayer;

        /// <summary>
        /// Number of the current round
        /// </summary>
        private int currentRound;

        /// <summary>
        /// Index of the winner
        /// </summary>
        private Player winner;

        /// <summary>
        /// Map of the current map
        /// </summary>
        private Map map;

        private int nbRounds;

        private int nbUnits;


        public Player[] Players
        {
            get { return this.players; }
        }

        public int CurrentRound
        {
            get { return this.currentRound; }
            set 
            { 
                if(value != this.currentRound+1) {
                    throw new Exception("Anormal");
                }

                this.currentRound = value;
            }
        }

        public Player FirstPlayer
        {
            get { return this.firstPlayer; }
            set { throw new NotSupportedException(); }
        }

        public Player CurrentPlayer
        {
            get { return this.currentPlayer;  }
            set { this.currentPlayer = value; }
        }

        public int NbUnits 
        {
            get { return this.nbUnits; }
        }

        public int NbRounds
        {
            get { return this.nbRounds; }
        }

        public Map Map
        {
            get { return this.map; }
        }


        private Game() {}

        public static Game Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Game();
                }
                return instance;
            }
        }

        /// <summary>
        /// Initialize game data
        /// </summary>
        public void initGame(int nbRounds, int nbUnits)
        {
            map = new Map();
            players = new Player[2];
            currentRound = 1;

            this.nbRounds = nbRounds;
            this.nbUnits = nbUnits;
        }


        /// <summary>
        /// Init a player according to its configuration
        /// </summary>
        /// <param name="i">Player number</param>
        /// <param name="nickname">Personal nickname</param>
        /// <param name="nation">Nation choosed</param>
        public void initPlayer(int i, string nickname, NationType nation)
        {
            players[i] = new Player(nickname, nation, this.nbUnits);
            // TODO: check unicity of nickname and nation?
        }

        /// <summary>
        /// Generate a new random map with the size defined
        /// by the game type
        /// </summary>
        public void createMap(int mapSize)
        {
            map = new Map();
            map.generateMap(mapSize);
        }

        /// <summary>
        /// Place units players as far as possible on the game board
        /// </summary>
        public void placePlayers()
        {
            // Get random start position
            Point[] starts = map.getStartPoints();
            for (int i = 0; i < 2; i++)
            {
                // Update map units list
                map.initUnits(players[i].Units, starts[i]);
                // Update unit current position
                foreach (IUnit u in players[i].Units)
                {
                    u.CurrentPosition = starts[i];
                }
            }
        }


        /// <summary>
        /// Determine who's playing first
        /// </summary>
        public void determinePlayOrder()
        {
            Random random = new Random();
            int index = random.Next(0, 2);
            firstPlayer = players[index];
            currentPlayer = players[index];
        }

        /// <summary>
        /// End of a round. If it was the first player, passed
        /// to the second player. Else, move to the next round.
        /// 
        /// Also reset every movement points for each units
        /// </summary>
        public void endRound()
        {
            foreach (Unit unit in this.currentPlayer.Units)
            {
                unit.MovePoint = 1;
            }

            if (this.currentPlayer == this.firstPlayer)
            {
                int index = (this.players[0] == this.firstPlayer) ? 1 : 0;
                this.currentPlayer = this.players[index];
            }
            else
            {
                this.currentPlayer = this.firstPlayer;
                this.currentRound++;
            }
        }

        /// <summary>
        /// Check if the game is finished or not, according to
        /// the round number and units remaining
        /// </summary>
        /// <returns>A boolean (true if the game is finished)</returns>
        public bool isGameFinished()
        {
            // Check round limit 
            if (currentRound >= this.nbRounds)
                return true;

            // Check units number
            return (players[0].Units.Count == 0 ||
                     players[1].Units.Count == 0);
        }

        public void setWinner()
        {
            if (players[1].Units.Count == 0)
            {
                winner = players[0];
            }
            else if (players[0].Units.Count == 0)
            {
                winner = players[1];
            }
            else
            {
                // Compute points of each player and takes the maximum

            }
        }
    }
}
