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
        private int maxHealth;

        private int attackPoints;
        private int defensePoints;
        private int movePoint;


        public int Health
        {
            get { return this.health; }
            set { }
        }

        public int Attack
        {
            get { return (int)(this.attackPoints * ((double)this.health / this.maxHealth)); }
        }

        public int Defense
        {
            get { return this.defensePoints; }
        }

        public void attack(ICase target)
        {
            // Get best defense unit on the tile
            IUnit defender = target.getBestDefensiveUnit();

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
