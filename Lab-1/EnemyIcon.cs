using Lab_1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1
{
    // класс представляющий иконку врага
    public class EnemyIcon
    {
        //имя иконки
        public string Name { get; set; }
        
        //путь
        public string ImagePath { get; set; }

        public EnemyIcon(string name, string imagePath)
        {
            Name = name;
            ImagePath = imagePath;
        }
    }

}