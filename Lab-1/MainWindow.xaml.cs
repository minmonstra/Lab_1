using System.IO;
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
using Microsoft.Win32;

namespace Lab_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<EnemyIcon> enemyIcons;
 

        public MainWindow()
        {
            InitializeComponent();
            enemyIcons = new List<EnemyIcon>();
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
        //выбор папки с изображениями
        public void SelectFolderButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new OpenFolderDialog();

            if (dialog.ShowDialog() == true)
            {
                LoadIconsFromFolder(dialog.FolderName);
            }
        }
    }
}