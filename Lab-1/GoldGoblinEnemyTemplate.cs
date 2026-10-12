using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1
{
    public class GoldGoblinEnemyTemplate : CEnemyTemplate
    {
        public int LifeTime
        {
            get { return TimeValue; }
            set { TimeValue = value > 0 ? value : 25; }
        }
        int TimeValue;
        public GoldGoblinEnemyTemplate(string name, string iconName, int baseLife,
            double lifeModifier, int baseGold,
            double goldModifier, double spawnChance, int time)
            : base(name, iconName, baseLife, lifeModifier, baseGold, goldModifier, spawnChance)
        {
            LifeTime = time;  


        }
    }
}