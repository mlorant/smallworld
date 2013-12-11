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
        private int health = maxHealth;
        private const int maxHealth = 5;

        private int attackPoints = 2;
        private int defensePoints = 1;
        private int movePoint = 1;

        private Point currentPosition;

        public int Health
        {
            get { return this.health; }
            set { }
        }

        public int Attack
        {
            get { return (int)(this.attackPoints * ((double)this.health / Unit.maxHealth)); }
        }

        public int Defense
        {
            get { return this.defensePoints; }
        }

        public Point CurrentPosition
        {
            get { return this.currentPosition; }
            set { this.currentPosition = value; }
        }

        public void attack(Point target)
        {
            // Get best defense unit on the tile
            IUnit defender = Game.Instance.Map.getBestDefensiveUnit(target);

            if (defender != null)
            {

                if (defender.Defense == 0)
                {
                    // Win 
                }

                // Get number of attacks, between 3 and the maxHeal+2
                int maxHeal = Math.Max(this.health, defender.Health);
                Random rand = new Random();
                int attacksCount = rand.Next(3, maxHeal + 2);

                for (int i = 0; i < attacksCount; i++)
                {
                    double powerBalance = (double) this.Attack / defender.Defense;
                }

            }
            
        }

        public bool isAlive()
        {
            return (health > 0);
        }

        public int getMoveCost()
        {
            throw new System.NotImplementedException();
        }

        public void move(Point target)
        {
            throw new NotImplementedException();
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
            bool verticalMove = (tgt.X == currentPosition.X &&
                                    Math.Abs(currentPosition.Y - tgt.Y) == 1);
            bool horizontalMove = (tgt.Y == currentPosition.Y &&
                                    Math.Abs(currentPosition.X - tgt.X) == 1);

            return (verticalMove || horizontalMove);
        }

        /// <summary>
        /// Check if the unit can move to the position given.
        /// By default, only if the unit is near the tile and if
        /// it isn't to a Sea tile.
        /// </summary>
        /// <param name="tgt"></param>
        /// <returns></returns>
        public bool canMoveOn(Point tgt)
        {
            ICase targetType = Game.Instance.Map.getCase(tgt);

            return this.isNear(tgt) && !(targetType is Sea);
        }
    }
}
