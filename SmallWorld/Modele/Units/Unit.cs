using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public abstract class Unit : IUnit
    {
        /// <summary>
        /// Current health status of the unit
        /// </summary>
        private int health;

        private int attackPoints;
        private int defensePoints;
        private int movePoint;

        public void attack(ICase target)
        {
            throw new NotImplementedException();
        }

        public bool isAlive()
        {
            return (health > 0);
        }

        public int getMoveCost()
        {
            throw new System.NotImplementedException();
        }

        public int getBonusPoint()
        {
            throw new System.NotImplementedException();
        }

        public int move()
        {
            throw new System.NotImplementedException();
        }


        public void move(ICase target)
        {
            throw new NotImplementedException();
        }
    }
}
