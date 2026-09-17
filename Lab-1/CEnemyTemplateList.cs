using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Text.Json.Serialization;

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
        public List<string> GetListOfEnemyNames()
        {
            List<string> enemyNames = new List<string>();
            foreach (var enemy in enemies)
            {
                enemyNames.Add(enemy.Name);
            }
            return enemyNames;
        }

        public void SaveToJson(string path)
        {
            string json = JsonSerializer.Serialize(enemies);
            File.WriteAllText(path, json);
        }

        public void LoadFromJson(string path)
        {
            if (File.Exists(path))
            {
                enemies.Clear();
                string json = File.ReadAllText(path);
                JsonDocument document = JsonDocument.Parse(json);
                foreach (JsonElement enemyElement in document.RootElement.EnumerateArray())
                {
                    string name=enemyElement.GetProperty("Name").GetString();
                    string iconName = enemyElement.GetProperty("IconName").GetString();
                    int baseLife = enemyElement.GetProperty("BaseLife").GetInt32();
                    double lifeModifier = enemyElement.GetProperty("LifeModifier").GetDouble();
                    int baseGold = enemyElement.GetProperty("BaseGold").GetInt32();
                    double goldModifier = enemyElement.GetProperty("GoldModifier").GetDouble();
                    double spawnChance = enemyElement.GetProperty("SpawnChance").GetDouble();
                    AddEnemy(name,iconName,baseLife,lifeModifier,baseGold,goldModifier,spawnChance);

                }
                 
            }


        }
    }
}
    

