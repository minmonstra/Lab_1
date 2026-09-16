using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<EnemyIcon> enemyIcons;
        CEnemyTemplate? currentEnemy;
        CEnemyTemplateList enemyList;

        public MainWindow()
        {
            InitializeComponent();

            enemyIcons = new List<EnemyIcon>();
            enemyList = new CEnemyTemplateList();
        }
        public void LoadIconsFromFolder(string path)
        {
            //очистка старого списка иконок, чтобы при повторном выборе папки они не дублировались
            enemyIcons.Clear();

            //фильтры расширений изображений — по заданию нужны ВСЕ изображения из папки
            string[] filters = { "*.png", "*.jpg", "*.jpeg", "*.bmp", "*.gif" };
            //получение путей до изображений по каждому из расширений
            List<string> files = new List<string>();
            foreach (string filter in filters)
            {
                files.AddRange(Directory.GetFiles(path, filter));
            }
            //перебор всех полученных путей
            //в file содержится полный путь до изображения
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
            //очистка отображаемого списка перед повторным заполнением
            IconsListBox.Items.Clear();

            foreach (EnemyIcon icon in enemyIcons)
            {
                Image image = new Image()
                {
                    //форматирование иконок под единый размер
                    Source = new BitmapImage(
                        new Uri(icon.ImagePath)
                    ),
                    Height = 450,
                    Width = 450
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
            if (iconHolder != null && iconHolder.SelectedItem is Image selectedImage)
            {
                // получение имени файла из источника изображения
                // так как Source это Uri, то для получения имени файла
                // нужно преобразовать его в строку и использовать Path.GetFileName.
                // ListBox хранит в себе объекты типа Image
                // selectedImage.Source.ToString() возвращает полный путь до изображения
                string iconName = System.IO.Path.GetFileName(selectedImage.Source.ToString());
                // присвоение имени иконки в шаблон врага (только если противник уже выбран/добавлен)
                currentEnemy?.SetIconName(iconName);
            }
        }

        // добавление нового противника в список на основании данных из полей ввода
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!TryReadEnemyFields(out string name, out int baseLife, out double lifeModifier,
                out int baseGold, out double goldModifier, out double spawnChance))
            {
                return;
            }

            string iconName = currentEnemy?.IconName ?? string.Empty;

            enemyList.AddEnemy(name, iconName, baseLife, lifeModifier, baseGold, goldModifier, spawnChance);
            currentEnemy = enemyList.GetEnemyByName(name);

            MessageBox.Show($"Противник \"{name}\" добавлен.");
        }

        // редактирование выбранного противника значениями из полей ввода
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (currentEnemy == null)
            {
                MessageBox.Show("Сначала добавьте или выберите противника для редактирования.");
                return;
            }

            if (!TryReadEnemyFields(out string name, out int baseLife, out double lifeModifier,
                out int baseGold, out double goldModifier, out double spawnChance))
            {
                return;
            }

            string oldName = currentEnemy.Name;
            string iconName = currentEnemy.IconName;

            // т.к. свойства CEnemyTemplate доступны только для чтения,
            // редактирование реализовано через удаление старой записи и добавление обновлённой
            enemyList.DeleteEnemyByName(oldName);
            enemyList.AddEnemy(name, iconName, baseLife, lifeModifier, baseGold, goldModifier, spawnChance);
            currentEnemy = enemyList.GetEnemyByName(name);

            MessageBox.Show($"Противник \"{name}\" отредактирован.");
        }

        // удаление текущего выбранного противника из списка
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (currentEnemy == null)
            {
                MessageBox.Show("Сначала добавьте или выберите противника для удаления.");
                return;
            }

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
                MessageBox.Show("Список противников сохранён.");
            }
        }

        // загрузка списка противников из JSON-файла
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "JSON файлы (*.json)|*.json"
            };

            if (dialog.ShowDialog() == true)
            {
                enemyList.LoadFromJson(dialog.FileName);
                MessageBox.Show("Список противников загружен.");
            }
        }

        // вспомогательный метод: чтение и валидация числовых полей ввода
        private bool TryReadEnemyFields(out string name, out int baseLife, out double lifeModifier,
            out int baseGold, out double goldModifier, out double spawnChance)
        {
            name = Name_enemy.Text;
            baseLife = 0;
            lifeModifier = 0;
            baseGold = 0;
            goldModifier = 0;
            spawnChance = 0;

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Введите имя противника.");
                return false;
            }

            if (!int.TryParse(Health_enemy.Text, out baseLife) ||
                !double.TryParse(Health_mod_enemy.Text, out lifeModifier) ||
                !int.TryParse(Gold_enemy.Text, out baseGold) ||
                !double.TryParse(Gold_mod_enemy.Text, out goldModifier) ||
                !double.TryParse(Spawn_enemy.Text, out spawnChance))
            {
                MessageBox.Show("Проверьте правильность заполнения числовых полей.");
                return false;
            }

            return true;
        }

    }
}