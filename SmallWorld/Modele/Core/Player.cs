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
        private IUnit[] units;

        /// <summary>
        /// Player's nation. Use to fabric units
        /// </summary>
        private INation nation;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="nationType"></param>
        /// <param name="nbUnits"></param>
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

            units = new Unit[nbUnits];
            for (int i = 0; i < nbUnits; i++)
                units[i] = nation.fabricUnit();
        }

        public IUnit[] getUnits()
        {
            return units;
        }
    }
}
