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
        
        private Player[] _players;
        private Player _firstPlayer;
        private Player _currentPlayer;
        private int _currentRound;
        private Map _map;
        private int _nbRounds;
        private int _nbUnits;

        public Player[] Players
        {
            get { return this._players; }
        }

        public int CurrentRound
        {
            get { return this._currentRound; }
            set 
            { 
                if(value != this._currentRound+1) {
                    throw new Exception("Anormal");
                }

                this._currentRound = value;
            }
        }

        public Player FirstPlayer
        {
            get { return this._firstPlayer; }
            set { throw new NotSupportedException(); }
        }

        public Player CurrentPlayer
        {
            get { return this._currentPlayer;  }
            set { this._currentPlayer = value; }
        }

        public int NbUnits 
        {
            get { return this._nbUnits; }
            set { this._nbUnits = value; }
        }

        public int NbRounds
        {
            get { return this._nbRounds; }
        }

        public Map Map
        {
            get { return this._map; }
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
            _map = new Map();
            _players = new Player[2];
            _currentRound = 1;

            this._nbRounds = nbRounds;
            this._nbUnits = nbUnits;
        }


        /// <summary>
        /// Init a player according to its configuration
        /// </summary>
        /// <param name="i">Player number</param>
        /// <param name="nickname">Personal nickname</param>
        /// <param name="nation">Nation choosed</param>
        public void initPlayer(int i, string nickname, NationType nation)
        {
            _players[i] = new Player(nickname, nation, this._nbUnits);
            // TODO: check unicity of nickname and nation?
        }

        /// <summary>
        /// Generate a new random map with the size defined
        /// by the game type
        /// </summary>
        public void createMap(int mapSize)
        {
            _map = new Map();
            _map.generateMap(mapSize);
        }

        /// <summary>
        /// Place units players as far as possible on the game board
        /// </summary>
        public void placePlayers()
        {
            // Get random start position
            Point[] starts = _map.getStartPoints();
            for (int i = 0; i < 2; i++)
            {
                // Update map units list
                _map.initUnits(_players[i].Units, starts[i]);
                // Update unit current position
                foreach (IUnit u in _players[i].Units)
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
            _firstPlayer = _players[index];
            _currentPlayer = _players[index];
        }

        /// <summary>
        /// End of a round. If it was the first player, passed
        /// to the second player. Else, move to the next round.
        /// 
        /// Also reset every movement points for each units
        /// </summary>
        public void endRound()
        {
            foreach (Unit unit in this._currentPlayer.Units)
            {
                unit.MovePoint = 1;
            }

            if (this._currentPlayer == this._firstPlayer)
            {
                int index = (this._players[0] == this._firstPlayer) ? 1 : 0;
                this._currentPlayer = this._players[index];
            }
            else
            {
                this._currentPlayer = this._firstPlayer;
                this._currentRound++;
            }
        }

        /// <summary>
        /// Check if the game is finished or not, according to
        /// the round number and units remaining
        /// </summary>
        /// <returns>A boolean (true if the game is finished)</returns>
        public bool isFinished()
        {
            // Check round limit 
            if (_currentRound > this._nbRounds)
                return true;

            // Check units number
            return (_players[0].Units.Count == 0 ||
                     _players[1].Units.Count == 0);
        }

        public IPlayer getWinner()
        {
            // Compute points of each player and takes the maximum
            int p1 = _players[0].computePoints();
            int p2 = _players[1].computePoints();
            if (p1 > p2 || _players[1].Units.Count == 0)
                return _players[0];
            else
                return _players[1];
        }
    }
}
