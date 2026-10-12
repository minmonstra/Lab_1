using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Text.Json.Serialization;
namespace Lab_1
{
    public abstract class CEnemyTemplate
    {
        string name;
        public string Name
        {
            get { return name; }
            set { if (value.Length > 0) name = value; else name = "unknown"; }
        }

        string iconname;
        public string IconName
        {
            get { return name; }
            set { if (value.Length > 0) name = value; else name = "unknown"; }
        }

        int baselife;

        public int BaseLife
        {
            get { return baselife; }
            set { if (value > 0) baselife = value; else baselife = 25; }
        }

        double lifeModifier;
        public double LifeModifier
        {
            get { return lifeModifier; }
            set { if (value > 0) lifeModifier = value; else lifeModifier = 25; }
        }
        int baseGold;
        public int BaseGold
        {
            get { return baseGold; }
            set { if (value > 0) baseGold = value; else baseGold = 25; }
        }
        double goldModifier;
        public double GoldModifier
        {
            get { return goldModifier; }
            set { if (value > 0) goldModifier = value; else goldModifier = 25; }
        }
        double spawnChance;
        public double SpawnChance
        {
            get { return spawnChance; }
            set { if (value > 0) spawnChance = value; else spawnChance = 25; }
        }



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
        public void SetIconName(string iconName)
        {
            IconName = iconName;
        }
    }
}