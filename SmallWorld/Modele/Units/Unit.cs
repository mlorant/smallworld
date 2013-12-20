using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public abstract class Unit : IUnit
    {
        /// <summary>
        /// Current health status of the unit
        /// </summary>
        private int _health = _maxHealth;
        private const int _maxHealth = 5;
        private int _id;
        private Rectangle _imagePosition;

        private int _attackPoints = 2;
        private int _defensePoints = 1;
        private double _movePoint = 1;

        private Point _currentPosition;

        public int Health
        {
            get { return this._health; }
            set { this._health = value; }
        }

        public int Id
        {
            get { return this._id; }
            set { this._id = value;}
        }


        public int Attack
        {
            get { return (int)(this._attackPoints * ((double)this._health / Unit._maxHealth)); }
        }

        public int Defense
        {
            get { return this._defensePoints; }
        }

        public double MovePoint
        {
            get { return this._movePoint; }
            set { this._movePoint = value; }
        }

        public Point CurrentPosition
        {
            get { return this._currentPosition; }
            set { this._currentPosition = value; }
        }

        public void attack(Point target)
        {
            // Get best defense unit on the tile
            IUnit defender = Game.Instance.Map.getBestDefensiveUnit(target);

            if (defender != null && (this._movePoint > 0))
            {

                if (defender.Defense == 0)
                {
                    // Win 
                }

                // Get number of attacks, between 3 and the maxHeal+2
                int maxHeal = Math.Max(this._health, defender.Health);
                Random rand = new Random();
                int attacksCount = rand.Next(3, maxHeal + 2);

                for (int i = 0; i < attacksCount; i++)
                {
                    double powerBalance = (double)this.Attack / defender.Defense;
                }

                this._movePoint--;
            }

        }

        public bool isAlive()
        {
            return (_health > 0);
        }

        public int getMoveCost()
        {
            throw new System.NotImplementedException();
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
            // Elle doit avoir le droit de bouger
            if (this._movePoint > 0)
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
            ICase targetType = Game.Instance.Map.getCase(tgt);

            return this.isNear(tgt) && !(targetType is Sea);
        }
    }
}
