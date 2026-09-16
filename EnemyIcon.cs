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