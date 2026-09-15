using Microsoft.Win32;
using System;
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
        CEnemyTemplate currentEnemy;
        CEnemyTemplateList enemyList;

        public MainWindow()
        {
            InitializeComponent();

            enemyIcons = new List<EnemyIcon>();
            enemyList = new CEnemyTemplateList();
        }
        public void LoadIconsFromFolder(string path)
        {
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
            foreach (EnemyIcon icon in enemyIcons)
            {
                Image image = new Image()
                {
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
                currentEnemy.SetIconName(iconName);
            }
        }

    }
}