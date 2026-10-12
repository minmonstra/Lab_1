using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1
{
    public class CGoldGoblinEnemyTemplate : CEnemyTemplate
    {
        public int lifeTime
        {
            get { return lifeTimeValue; }
            set { lifeTimeValue = value > 0 ? value : 25; }
        }
        int lifeTimeValue;
        public CGoldGoblinEnemyTemplate(string name, string iconName, int baseLife,
            double lifeModifier, int baseGold,
            double goldModifier, double spawnChance, int time)
            : base(name, iconName, baseLife, lifeModifier, baseGold, goldModifier, spawnChance)
        {
            lifeTime = time;  


        }
    }
}