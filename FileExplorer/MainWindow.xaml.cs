using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static FileExplorer.Controllers;


namespace FileExplorer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _labelPathText;

        public string LabelPathText
        {
            get { return _labelPathText; }
            set
            {
                _labelPathText = value;
                LabelPath.Content = value;
            }
        }


        public string NewName
        {
            get { 
                return TextBoxNewName.Text; 
            }
        }

        public DisplayMode DisplayMode { 
            get
            {
                return RadioButtonFiles.IsChecked == true ? DisplayMode.Files : DisplayMode.Folders;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonChoose_Click(object sender, RoutedEventArgs e)
        {
            LabelPathText = ChooseFilePathDialogResut();
            InitFileSystemInfo(LabelPathText);
            ListView.ItemsSource = InitializeData(DisplayMode);
            ListView.View = CreateColumns(DisplayMode, LabelPathText);
        }


        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            ListView.ItemsSource = InitializeData(DisplayMode);
            ListView.View = CreateColumns(DisplayMode, LabelPathText);
            OffOnButtons(false);
        }

     


        private void OffOnButtons(bool value)
        {
            //ButtonChoose.IsEnabled = value;
            ButttonDelete.IsEnabled = value;
            ButtonRename.IsEnabled = value;
            ButtonRefresh.IsEnabled = value;
            ButtonMove.IsEnabled = value;
        }

        private void ButttonDelete_Click(object sender, RoutedEventArgs e)
        {
            object Item = ListView.SelectedItems[0];
            DeleteItem(LabelPathText, (BaseModel)Item);
        }

        private void ButtonMove_Click(object sender, RoutedEventArgs e)
        {
            BaseModel Item = (BaseModel)ListView.SelectedItems[0];
            MoveItem(LabelPathText, Item);
        }

        private void ButtonRename_Click(object sender, RoutedEventArgs e)
        {
            ButtonApplyNewName.Visibility = Visibility.Visible;
            TextBoxNewName.Visibility = Visibility.Visible;
        }

        private void ButtonApplyNewName_Click(object sender, RoutedEventArgs e)
        {
            BaseModel Item = (BaseModel)ListView.SelectedItems[0];
            RenameItem(LabelPathText, Item, NewName);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (!(ListView is null || ListView.ItemsSource is null))
                ButtonRefresh.IsEnabled = true;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // if (ListView.SelectedItems.Count != 0)
                OffOnButtons(true);
        }
    }
}
