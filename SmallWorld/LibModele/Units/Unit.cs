using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using mapWrapper;

namespace SmallWorld
{

    /// <summary>
    /// Enumeration of possible Unit Type, including None
    /// </summary>
    public enum UnitType { None, Viking, Gallic, Dwarf }

    /// <summary>
    /// Move type possible in suggestions
    /// </summary>
    public enum MoveType { Impossible = 0,	Possible, Suggested }

    /// <summary>
    /// Unit base class. An unit has a name, 
    /// an health status and attack/defense points, compute
    /// according to the attacker/defender.
    /// Each unit known it own position. 
    /// </summary>
    [Serializable()]
    public abstract class Unit : IUnit, ISerializable
    {
        private int _health = MAXHEALTH;
        private const int MAXHEALTH = 5;
        private int _id;

        private int _attackPoints = 2;
        private int _defensePoints = 1;
        private double _movePoint = 1;

        private Point _currentPosition;

        /// <summary>
        /// Name of the unit
        /// </summary>
        public string Name
        {
            get { return this.GetType().Name + " " + this.Id; }
        }

        /// <summary>
        /// Current health status
        /// </summary>
        public int Health
        {
            get { return this._health; }
            set { this._health = value; }
        }

        /// <summary>
        /// Max Health (at the beginning of the game)
        /// </summary>
        public int MaxHealth
        {
            get { return MAXHEALTH; }
        }

        /// <summary>
        /// Unique identifier for the unit in its nation
        /// </summary>
        public int Id
        {
            get { return this._id; }
            set { this._id = value;}
        }

        /// <summary>
        /// Base attack points
        /// </summary>
        public int Attack
        {
            get 
            {                
                return (int)Math.Round(this._attackPoints * 
                    ((double)this._health / Unit.MAXHEALTH), 0, MidpointRounding.AwayFromZero); 
            }
        }

        /// <summary>
        /// Base defense points
        /// </summary>
        ///
        public int Defense
        {
            get { return this._defensePoints; }
        }

        /// <summary>
        /// Current move points available
        /// </summary>
        public double MovePoint
        {
            get { return this._movePoint; }
            set { this._movePoint = value; }
        }

        /// <summary>
        /// Max move points possible in a round
        /// </summary>
        public double MaxMovePoint
        {
            get { return 1; }
        }

        /// <summary>
        /// Position on the map
        /// </summary>
        public Point CurrentPosition
        {
            get { return this._currentPosition; }
            set { this._currentPosition = value; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Unit() { }

        /// <summary>
        /// Deserialization constructor.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        public Unit(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            this._id = (int)info.GetValue("UnitId", typeof(int));
            this._health = (int)info.GetValue("UnitHealth", typeof(int));
            this._movePoint = (int)info.GetValue("UnitMove", typeof(int));
            this._currentPosition = (Point)info.GetValue("UnitPosition", typeof(Point));
        }

        /// <summary>Attack another unit located on the `target` point.
        /// Returns true if the current unit wins</summary>
        public bool attack(IUnit defender, Point target)
        {
            
            // Verify if destination is possible
            if (this.canMoveOn(target))
            {
                if (defender != null && (this._movePoint > 0))
                {
                    // When oponent can't defend itself it died then unit wins.
                    if (defender.Defense == 0)
                    {
                        return true; 
                    }

                    // Get number of attacks, between 3 and the maxHeal+2
                    int maxHeal = Math.Max(this._health, defender.Health);
                    Random rand = new Random();
                    int attacksCount = rand.Next(3, maxHeal + 2);

                    int i = 0;
                    // the God of Smalworld choose who must be hurt
                    Random godHand = new Random();
                    while( i < attacksCount && this.isAlive()) 
                    {
                        //compute the percentage of victory for the attacker
                        double percentageAgainstAttacker = computePercentageToWin(defender);
                       
                        int attack = godHand.Next(0, 100);
                        // then god condemns the attacker otherwise the defender!
                        if (attack < percentageAgainstAttacker)
                        {
                            this.Health--;
                        }
                        else
                        {
                            defender.Health--;
                        }

                        // When oponent is dead then battle is won.
                        if (defender.Health == 0)
                        {
                            this._movePoint = 0;
                            return true;
                        }

                        i++;
                    }
                    this._movePoint = 0;
                }
                return false;
            }
            return false;

        }

        /// <summary>
        /// Get percentage of chance to win the battle
        /// against the defender unit given
        /// </summary>
        /// <param name="defender">Enemy unit</param>
        public double computePercentageToWin(IUnit defender)
        {
            double powerBalance;
            double percentageAgainstDefender = 50;
            double percentageAgainstAttacker = 50;
            // Force against the defense = his attack (see formulas) / oponent defense
            if (this.Attack < defender.Defense)
            {
                powerBalance = 1 - (double)this.Attack / defender.Defense;
                // Chance to lose the battle = 50% + 50%*powerbalance
                percentageAgainstAttacker += powerBalance * 50;
                percentageAgainstDefender = 100 - percentageAgainstAttacker;
            }
            else
            {
                powerBalance = 1 - (double)defender.Defense / this.Attack;
                percentageAgainstDefender += powerBalance * 50;
                percentageAgainstAttacker = 100 - percentageAgainstDefender;
            }

            return percentageAgainstAttacker;
        }

        /// <summary>
        /// Check if the unit is still alive
        /// </summary>
        /// <returns></returns>
        public bool isAlive()
        {
            return (_health > 0);
        }

        /// <summary>
        /// Compute how many points the units worth for
        /// the player
        /// </summary>
        /// <returns></returns>
        public virtual int getPoints()
        {
            return 1;
        }

        /// <summary>
        /// La méthode qui permet au unité de bouger de case. Prend en compte l'autorisation ou non de bouger
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual bool move(Point target)
        {
            // On vérifie si sa destination est possible
            if (this.canMoveOn(target))
            {

                Game.Instance.Map.getUnits(target).Add(this);
                Game.Instance.Map.getUnits(this.CurrentPosition).Remove(this);

                this.CurrentPosition = target;

                this._movePoint--;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if the position given is accessible from
        /// the current position of the unit. Only horizontal
        /// and vertical move of 1 are allowed (no diagonal)
        /// </summary>
        /// <param name="tgt">Target position</param>
        /// <returns>True is the position is accessible with 1 movement point</returns>
        protected bool isNear(Point tgt)
        {
            bool verticalMove = (tgt.X == _currentPosition.X &&
                                    Math.Abs(_currentPosition.Y - tgt.Y) == 1);
            bool horizontalMove = (tgt.Y == _currentPosition.Y &&
                                    Math.Abs(_currentPosition.X - tgt.X) == 1);

            return (verticalMove || horizontalMove);
        }

        /// <summary>
        /// Check if the unit can move to the position given.
        /// By default, only if the unit is near the tile and if
        /// it isn't to a Sea tile.
        /// </summary>
        /// <param name="tgt"></param>
        /// <returns></returns>
        public virtual bool canMoveOn(Point tgt)
        {
            // Elle doit avoir le droit de bouger
            if (this._movePoint >= 1)
            {
                ICase targetType = Game.Instance.Map.getCase(tgt);

                return this.isNear(tgt) && !(targetType is Sea);
            }
            return false;
        }

        /// <summary>
        /// Returns list of points where the unit can go on the map,
        /// according to the current units positions and envirronement. 
        /// (Compute via the C++ library)
        /// </summary>
        /// <returns>List of Point where the unit can go on the map, with the 
        /// type of suggestion : Possible or Suggested</returns>
        public List<Tuple<Point, MoveType>> getSuggestedPoints()
        {
            // Get tiles suggestion
            IMap map = Game.Instance.Map;
            UnitType unitEnum = (UnitType) Enum.Parse(typeof(UnitType), this.GetType().Name, true);

            var raw_points = map.SuggestEngine.get_tiles_suggested(map.NativeUnits, CurrentPosition.X, CurrentPosition.Y, 
                                                                    this._movePoint, (int) unitEnum);

            // Construct a list of Point
            var points = new List<Tuple<Point, MoveType>>();
            foreach (Tuple<int, int, int> pt in raw_points)
            {
                MoveType mt = (MoveType)pt.Item3;
                points.Add(new Tuple<Point, MoveType>(new Point(pt.Item1, pt.Item2), mt));
            }
            
            return points;
        }

        // Serialization function
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("UnitId", this._id);
            info.AddValue("UnitHealth", this._health);
            info.AddValue("UnitMove", this._movePoint);
            info.AddValue("UnitPosition", this.CurrentPosition);
        }
    }
}
