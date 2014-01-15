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
        // Private attributes
        private string nickname;
        private List<IUnit> units;
        private INation nation;

        /// <summary>Player nickname</summary>
        public string Nickname
        {
            get { return this.nickname; }
        }

        /// <summary>Units list of the player</summary>
        public List<IUnit> Units
        {
            get { return this.units; }
        }

        /// <summary>Player's nation. Use to fabric units</summary>
        public INation Nation
        {
            get { return this.nation; }
        }

        /// <summary>
        /// Construct one player with his initial units
        /// </summary>
        /// <param name="nickname">Player nickname</param>
        /// <param name="nationType">Nation chosen by the player</param>
        /// <param name="nbUnits">Number of units to create</param>
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
                default:
                    throw new ArgumentException("Unknown nation");
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
                total += unit.getPoints();

            return total;
        }

        /// <summary>
        /// Returns the number of units the player can still
        /// play in the round
        /// </summary>
        public int getNbUnitsToPlay()
        {
            int total = 0;
            foreach (Unit unit in this.Units)
                if (unit.MovePoint > 0)
                    total += 1;

            return total;
        }


        /// <summary>
        /// Delete the unit of the player list and the map
        /// </summary>
        /// <param name="general">Player of the unit dead</param>
        public void buryUnit(IUnit unit)
        {
            Game.Instance.Map.getUnits(unit.CurrentPosition).Remove(unit);
            this.Units.Remove(unit);
        }

        
        /// <summary>Serialization function</summary>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("PlayerNickname", this.nickname);
            info.AddValue("PlayerUnits", this.units);
            info.AddValue("PlayerNation", this.nation);
        }

        /// <summary> Deserialization constructor.</summary>
        public Player(SerializationInfo info, StreamingContext ctxt)
        {
            this.nickname = (string) info.GetString("PlayerNickname");
            this.units = (List<IUnit>) info.GetValue("PlayerUnits", typeof(List<IUnit>));
            this.nation = (INation)info.GetValue("PlayerNation", typeof(INation));
        }
    }
}
