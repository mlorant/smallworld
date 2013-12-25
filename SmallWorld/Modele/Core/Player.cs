using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SmallWorld
{
    [Serializable()]
    public class Player : IPlayer, ISerializable
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

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("PlayerNickname", this.nickname);
            info.AddValue("PlayerUnits", this.units);
            info.AddValue("PlayerNation", this.nation);
        }

        // Deserialization constructor.
        public Player(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            this.nickname = (String) info.GetString("PlayerNickname");
            this.units = (List<IUnit>) info.GetValue("PlayerUnits", typeof(List<IUnit>));
            this.nation = (INation)info.GetValue("PlayerNation", typeof(INation));
        }
    
    }
}
