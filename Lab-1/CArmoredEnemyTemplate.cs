using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1
{
    public class CArmoredEnemyTemplate:CEnemyTemplate
    {
        public double Armor
        {
            get { return armorValue; }
            set { armorValue = value > 0 ? value : 25; }
        }
        double armorValue;
        public CArmoredEnemyTemplate(string name, string iconName, int baseLife,
            double lifeModifier, int baseGold,
            double goldModifier, double spawnChance, double armor)
            : base(name, iconName, baseLife, lifeModifier, baseGold, goldModifier, spawnChance)
            {
            Armor = armor;


        }
}
}
