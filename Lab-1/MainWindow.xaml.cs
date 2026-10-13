using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Lab_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<EnemyIcon> enemyIcons;
        CEnemyTemplate currentEnemy;
        CEnemyTemplateList enemyList;
        string selectedIconName;

        public MainWindow()
        {
            InitializeComponent();

            enemyIcons = new List<EnemyIcon>();
            enemyList = new CEnemyTemplateList();
            currentEnemy = null;
        }
        public void LoadIconsFromFolder(string path)
        {
            enemyIcons.Clear();
            //фильтр расширения изображения
            string filter = "*.png";
            //получение массива строк содержащих пути до изображений
            string[] files = Directory.GetFiles(path, filter);
            //перебор всех полученных путей
            //в file содержится путь до изображения с расширением .png
            foreach (string file in files)
            {
                enemyIcons.Add(
                new EnemyIcon(
                    System.IO.Path.GetFileName(file),
                    file
                )
                );
            }
        }
        public void DisplayIcons()
        {
            IconsListBox.Items.Clear();
            foreach (EnemyIcon icon in enemyIcons)
            {
                Image image = new Image()
                {
                    Source = new BitmapImage(
                        new Uri(icon.ImagePath)
                    ),
                    Height = 300,
                    Width = 500
                };

                IconsListBox.Items.Add(image);
            }
        }

        //выбор папки с изображениями
        public void SelectFolderButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new OpenFolderDialog();

            if (dialog.ShowDialog() == true)
            {
                LoadIconsFromFolder(dialog.FolderName);
                DisplayIcons();
            }
        }
        // обработчик события изменения выбора в ListBox
        private void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // приведение sender к типу ListBox
            ListBox iconHolder = sender as ListBox;
            // проверка что выбранный элемент является изображением
            // и что элемент не равен null
            if (iconHolder.SelectedItem is Image selectedImage && iconHolder.SelectedItem != null)
            {
                // получение имени файла из источника изображения
                // так как Source это Uri, то для получения имени файла
                // нужно преобразовать его в строку и использовать Path.GetFileName.
                // ListBox хранит в себе объекты типа Image
                // selectedImage.Source.ToString() возвращает полный путь до изображения
                string iconName = System.IO.Path.GetFileName(selectedImage.Source.ToString());
                // присвоение имени иконки в шаблон врага
                selectedIconName = iconName;

            }
        }

        private bool TryParseDoubleFlexible(string text, out double result)
        {
            // Попробовать локаль пользователя
            if (double.TryParse(text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out result))
                return true;
            // Попробовать invariant (точка как разделитель)
            if (double.TryParse(text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out result))
                return true;
            // Попробовать заменить запятую на точку и наоборот
            string t = text?.Replace(',', '.') ?? string.Empty;
            if (double.TryParse(t, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out result))
                return true;
            t = text?.Replace('.', ',') ?? string.Empty;
            if (double.TryParse(t, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out result))
                return true;
            result = 0;
            return false;
        }

        private CEnemyTemplate CreateTemplateFromForm() 
        {
            string name = Name_enemy.Text;
            if (name == null || name.Trim() == "")
            {
                MessageBox.Show("Введите имя противника.");
                return null;
            }
            
            string iconName = selectedIconName;
            if (string.IsNullOrEmpty(iconName))
            {
                MessageBox.Show("Выберите иконку для противника.");
                return null;
            }

            if (TypeEnemies.SelectedIndex == -1){ 
                MessageBox.Show("Выберите тип противника.");
                return null;
            }

            if (!int.TryParse(Health_enemy.Text, out int baseLife) ||
                   !TryParseDoubleFlexible(Health_mod_enemy.Text, out double lifeModifier) ||
                   !int.TryParse(Gold_enemy.Text, out int baseGold) ||
                   !TryParseDoubleFlexible(Gold_mod_enemy.Text, out double goldModifier) ||
                   !TryParseDoubleFlexible(Spawn_enemy.Text, out double spawnChance))
            {
                MessageBox.Show("Некорректные данные. Проверьте введённые значения.");
                return null;
            }


            switch (TypeEnemies.SelectedIndex)
            {
                case 0:
                    {
                        return new CNormalEnemyTemplate(name, selectedIconName, baseLife, lifeModifier,
                                                        baseGold, goldModifier, spawnChance);
                    }
                    
               case 1:
                    {
                        if (!TryParseDoubleFlexible(Special_enemy.Text, out double armor))
                        {
                            MessageBox.Show("Некорректное значение брони.");
                            return null;
                        }
                        return new CArmoredEnemyTemplate(name, selectedIconName, baseLife, lifeModifier,
                                                         baseGold, goldModifier, spawnChance, armor);
                    }
                case 2:
                    {
                        if (!int.TryParse(Special_enemy.Text, out int lifetime))
                        {
                            MessageBox.Show("Некорректное значение времени жизни.");
                            return null;
                        }
                        return new CGoldGoblinEnemyTemplate(name, selectedIconName, baseLife, lifeModifier,
                                                         baseGold, goldModifier, spawnChance, lifetime);
                    }
                default:
                    return null;
            }
        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            CEnemyTemplate newTemplate = CreateTemplateFromForm();
            if (newTemplate == null)
                return; // сообщение об ошибке уже показано внутри метода

            enemyList.AddEnemy(newTemplate);
            currentEnemy = newTemplate;
            EnemiesListBox.Items.Add(newTemplate.Name);

            MessageBox.Show($"Противник \"{newTemplate.Name}\" добавлен.");
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (currentEnemy == null)
            {
                MessageBox.Show("Выберите противника для редактирования.");
                return;
            }

            CEnemyTemplate newTemplate = CreateTemplateFromForm();
            if (newTemplate == null)
                return;

            enemyList.DeleteEnemyByName(currentEnemy.Name);
            enemyList.AddEnemy(newTemplate);
            currentEnemy = newTemplate;

            EnemiesListBox.Items.Clear();
            foreach (string n in enemyList.GetListOfEnemyNames())
                EnemiesListBox.Items.Add(n);

            currentEnemy = newTemplate;
            MessageBox.Show($"Противник \"{newTemplate.Name}\" изменён.");
        }
        // удаление текущего выбранного противника из списка
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (currentEnemy == null)
            {
                MessageBox.Show("Сначала добавьте или выберите противника для удаления.");
                return;
            }
            EnemiesListBox.Items.Remove(currentEnemy.Name);
            enemyList.DeleteEnemyByName(currentEnemy.Name);
            currentEnemy = null;

            MessageBox.Show("Противник удалён.");
        }

        // сохранение списка противников в JSON-файл
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "JSON файлы (*.json)|*.json",
                FileName = "enemies.json"
            };

            if (dialog.ShowDialog() == true)
            {
                enemyList.SaveToJson(dialog.FileName);
                MessageBox.Show("Противники сохранены");
            }
        }

        // загрузка списка противников из JSON-файла
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "JSON файлы (*.json)|*.json";

            if (dialog.ShowDialog() == true)
            {
                enemyList.LoadFromJson(dialog.FileName);

                EnemiesListBox.Items.Clear();

                foreach (string name in enemyList.GetListOfEnemyNames())
                {
                    EnemiesListBox.Items.Add(name);
                }

                MessageBox.Show("Противники загружены.");
            }

        }
        private void EnemiesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EnemiesListBox.SelectedItem == null)
                return;

            string name = EnemiesListBox.SelectedItem.ToString();

            currentEnemy = enemyList.GetEnemyByName(name);

            if (currentEnemy == null)
                return;



            Name_enemy.Text = currentEnemy.Name;
            Health_enemy.Text = currentEnemy.BaseLife.ToString();
            Health_mod_enemy.Text = currentEnemy.LifeModifier.ToString();
            Gold_enemy.Text = currentEnemy.BaseGold.ToString();
            Gold_mod_enemy.Text = currentEnemy.GoldModifier.ToString();
            Spawn_enemy.Text = currentEnemy.SpawnChance.ToString();

            selectedIconName = currentEnemy.IconName;
            IconsListBox.SelectedItem = null;
            // Ищем иконку противника
            foreach (object item in IconsListBox.Items)
            {
                if (item is Image image && image.Source != null)
                {
                    string imageName = System.IO.Path.GetFileName(
                    image.Source.ToString()
                    );

                    if (imageName == currentEnemy.IconName)
                    {
                        IconsListBox.SelectedItem = image;
                        IconsListBox.ScrollIntoView(image);
                        break;
                    }
                }
            }
            switch (currentEnemy)
            {
                case CArmoredEnemyTemplate armorValue:
                    TypeEnemies.SelectedIndex = 1;
                    Special_enemy.Text = armorValue.Armor.ToString();
                    break;

                case CGoldGoblinEnemyTemplate lifeTimeValue:
                    TypeEnemies.SelectedIndex = 2;
                    Special_enemy.Text = lifeTimeValue.LifeTime.ToString();
                    break;

                case CNormalEnemyTemplate:
                    TypeEnemies.SelectedIndex = 0;
                    Special_enemy.Text = "";
                    break;
            }
        }

    }
}