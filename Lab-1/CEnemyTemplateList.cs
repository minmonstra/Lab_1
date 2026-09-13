using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab_1
{
    public class CEnemyTemplateList
    {
        //Список противников из класса CEnemyTemplate
        List<CEnemyTemplate> enemies;
        public CEnemyTemplateList()
        {
            enemies = new List<CEnemyTemplate>(); }
        
            public void AddEnemy(string name, string iconName, int baseLife, double lifeModifier, int baseGold, double goldModifier, double spawnChance)
        {
            CEnemyTemplate enemy = new CEnemyTemplate(name, iconName, baseLife, lifeModifier, baseGold, goldModifier, spawnChance);
            enemies.Add(enemy);
        }
        public void DeleteEnemyByName(string name)
        {

            CEnemyTemplate delet_enemy_byname = null; // переменная типа CEnemyTemplate хранящий ссылку на обьект 
            foreach (var enemy in enemies)
            {
                if (enemy.Name == name)
                {
                    delet_enemy_byname = enemy;
                    break;
                }

            }
            if (delet_enemy_byname != null)
            {
                enemies.Remove(delet_enemy_byname);
            }
        }
        public void DeleteEnemyByIndex(int index)
        {
            if (index < enemies.Count && index >= 0)
            {
                enemies.Remove(enemies[index]);
            }
        }

        public CEnemyTemplate GetEnemyByName(string name) 
        {
            CEnemyTemplate findenemy = null;
            foreach (var enemy in enemies)
            {
                if (enemy.Name == name)
                {
                    findenemy = enemy;
                    break;
                }
            }
            return findenemy;
        }

        public CEnemyTemplate GetEnemyByIndex(int index)
        {
            if (index < enemies.Count && index >= 0)
            {
                return enemies[index];
            }
            else
            {
                return null;
            }



        }
    }
}
    

