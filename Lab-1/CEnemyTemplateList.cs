using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Text.Json.Serialization;

namespace Lab_1
{
    public class CEnemyTemplateList
    {
        private readonly ISaveList<List<CEnemyTemplate>> _serializer = new JsonEnemySaver();
        //Список противников из класса CEnemyTemplate
        List<CEnemyTemplate> enemies;
        public CEnemyTemplateList()
        {
            enemies = new List<CEnemyTemplate>(); }

        public void AddEnemy(CEnemyTemplate enemy)
        {
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
            _serializer.Save(enemies, path);
        }

        public void LoadFromJson(string path)
        {
            _serializer.Load(path);
        }
    }
}
    

