using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Text.Json.Serialization;
namespace Lab_1
{
    public class CEnemyTemplate
    {
        //Свойства класса — доступны для чтения снаружи,
        //изменяются только изнутри класса (через конструктор)
        [JsonInclude]
        public string Name { get; private set; }
        [JsonInclude]
        public string IconName { get; private set; }
        [JsonInclude]
        public int BaseLife { get; private set; }
        [JsonInclude]
        public double LifeModifier { get; private set; }
        [JsonInclude]
        public int BaseGold { get; private set; }
        [JsonInclude]
        public double GoldModifier { get; private set; }
        [JsonInclude]
        public double SpawnChance { get; private set; }
        

        //Конструктор класса
        public CEnemyTemplate(string name, string iconName, int baseLife,
        double lifeModifier, int baseGold,
        double goldModifier, double spawnChance)
        {
            Name = name;
            IconName = iconName;
            BaseLife = baseLife;
            LifeModifier = lifeModifier;
            BaseGold = baseGold;
            GoldModifier = goldModifier;
            SpawnChance = spawnChance;
        }
    }
}