using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class Gallic : Unit, IGallic
    {
        public Gallic(int id)
        {
            this.Id = id;
        }

        /// <summary>
        /// La méthode qui permet au unité de bouger de case. Prend en compte l'autorisation ou non de bouger
        /// Les Gaulois ont le droit de bouger de 2 cases "plaine".
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override bool move(Point target)
        {
            bool ret = base.move(target);
            Console.WriteLine(ret);
            if (ret)
            {
                Console.WriteLine("vrai");
                if (Game.Instance.Map.getCase(target) is Plain)
                {
                    Console.WriteLine(Game.Instance.Map.getCase(target));
                    this.MovePoint += 0.5;
                }
            }
            return ret;
        }


        /// <summary>
        /// Compute how many points the units worth for
        /// the player
        /// </summary>
        /// <returns></returns>
        public override int getPoints()
        {
            ICase targetType = Game.Instance.Map.getCase(this.CurrentPosition);

            if (targetType is Plain)
                return base.getPoints() + 1;
            else if (targetType is Mountain)
                return 0;
            else
                return base.getPoints();
        }

    }
}
