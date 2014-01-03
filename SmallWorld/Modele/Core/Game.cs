using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SmallWorld
{
    /// <summary>
    /// Core class of SmallWorld : directs the
    /// game, and let play each player when it's
    /// their turn.
    /// </summary>
    [Serializable()]
    public class Game : IGame, ISerializable, INotifyPropertyChanged 
    {

        // property changed event
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String property)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        private static Game instance;
        
        private IPlayer[] _players;
        private IPlayer _firstPlayer;
        private IPlayer _currentPlayer;
        private int _currentRound;
        private Map _map;
        private int _nbRounds;
        private int _nbUnits;

        public IPlayer[] Players
        {
            get { return this._players; }
            set { this._players = value; }
        }

        public int CurrentRound
        {
            get { return this._currentRound; }
            set 
            { 
                if(value != this._currentRound+1) {
                    throw new Exception("Round number shouldn't be increment by more than 1.");
                }

                this._currentRound = value;
                OnPropertyChanged("CurrentRound");
            }
        }

        public IPlayer FirstPlayer
        {
            get { return this._firstPlayer; }
            set { throw new NotSupportedException(); }
        }

        public IPlayer CurrentPlayer
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
            set { this._map = value; }
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


        /// <summary>
        /// Serialization of a game instance (for saving feature)
        /// </summary>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("GamePlayers", this._players);
            info.AddValue("GameMap", this._map);
            info.AddValue("GameFirstPlayer", this._firstPlayer);
            info.AddValue("GameCurrentPlayer", this._currentPlayer);
            info.AddValue("GameCurrentRound", this._currentRound);
            info.AddValue("GameNbRounds", this._nbRounds);
        }

        // Deserialization constructor.
        public Game(SerializationInfo info, StreamingContext ctxt)
        {
            this._players = (IPlayer[]) info.GetValue("GamePlayers", typeof(IPlayer[]));
            this._map = (Map) info.GetValue("GameMap", typeof(Map));
            this._firstPlayer = (IPlayer)info.GetValue("GameFirstPlayer", typeof(IPlayer));
            this._currentPlayer = (IPlayer)info.GetValue("GameCurrentPlayer", typeof(IPlayer));
            this._currentRound = (int)info.GetValue("GameCurrentRound", typeof(int));
            this._nbRounds = (int)info.GetValue("GameNbRounds", typeof(int));
            
            // Overwrite the current game with the new one created
            Game.instance = this;
        }


        /// <summary>
        /// Save the current game state in a serialized file.
        /// </summary>
        /// <param name="destination">Destination file of the saved game</param>
        public void saveCurrentGame(string destination)
        {
            Stream stream = File.Open(destination, FileMode.Create);
            BinaryFormatter bformat = new BinaryFormatter();

            bformat.Serialize(stream, Game.instance);
            stream.Close();

        }

        /// <summary>
        /// Load a new game from a file. The previous game
        /// will be overwritten.
        /// </summary>
        /// <param name="saveFile">File to load</param>
        public void restoreGame(string saveFile)
        {
            Stream stream = File.Open(saveFile, FileMode.Open);
            BinaryFormatter bformat = new BinaryFormatter();


            // Try to deserialize the file. In case of exception, check if
            // the exception is due to a bad format file or it's unknown.
            try
            {
                bformat.Deserialize(stream);
            }
            catch (Exception e)
            {
                if (e is SerializationException || e is System.Reflection.TargetInvocationException)
                    throw new InvalidSaveFileException();
                else
                    throw;

            }
            stream.Close();
        }
    }

    public class InvalidSaveFileException : Exception { }
}
