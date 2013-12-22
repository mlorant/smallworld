using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class Player : IPlayer
    {

        /// <summary>
        /// Player nickname
        /// </summary>
        private string nickname;

        /// <summary>
        /// Units list of the player
        /// </summary>
        private List<IUnit> units;

        /// <summary>
        /// Player's nation. Use to fabric units
        /// </summary>
        private INation nation;


        public string Nickname
        {
            get { return this.nickname; }
        }

        public List<IUnit> Units
        {
            get { return this.units; }
        }
        
        public INation Nation
        {
            get { return this.nation; }
        }

        
        public Player(string nickname, NationType nationType, int nbUnits)
        {
            this.nickname = nickname;

            switch (nationType)
            {
                case NationType.GALLIC:
                    nation = new NationGallic();
                    break;
                case NationType.DWARF:
                    nation = new NationDwarf();
                    break;
                case NationType.VIKING:
                    nation = new NationViking();
                    break;
            }

            units = new List<IUnit>();
            for (int i = 0; i < nbUnits; i++)
                units.Add(nation.fabricUnit(i));
        
        }


        /// <summary>
        /// Compute and returns the number of points of the player,
        /// by suming every points of his units.
        /// </summary>
        /// <returns>The score of the player</returns>
        public int computePoints()
        {
            int total = 0;
            foreach (Unit unit in this.Units)
            {
                total += unit.getPoints();
            }

            return total;
        }
    }
}
