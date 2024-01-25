using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using static FileExplorer.Utils;

namespace FileExplorer
{

    public static class Controllers
    {
        private const string PATH_NOT_CHOSEN = "Путь не выбран!";
        private const string OK_RESULT = "OK";

        public static ObservableCollection<BaseModel> Items { get; set; }
        private static FileSystemInfo[] FileSystemInfos;

        public static string ChooseFilePathDialogResut()
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();

            if (result.ToString() == OK_RESULT)
                return openFileDlg.SelectedPath;

            return PATH_NOT_CHOSEN;
        }

        //init data with all files and folders
        public static void InitFileSystemInfo(string path)
        {
            if (path == PATH_NOT_CHOSEN)
                return;

            DirectoryInfo di = new DirectoryInfo(path);
            FileSystemInfos = di.GetFileSystemInfos();
        }


        public static ObservableCollection<BaseModel> InitializeData(DisplayMode displayMode)
        {
            //путь всё ещё не выбран -> не создан новый экземпляр класса
            if (FileSystemInfos is null)
                return null;

            Items = new ObservableCollection<BaseModel>();

            switch (displayMode)
            {
                case DisplayMode.Files:
                    {
                        foreach (FileInfo file in FileSystemInfos.Where(i => i.Attributes != FileAttributes.Directory))
                            Items.Add(new FileModel { Name = file.Name, Extension = file.Extension, FileSize = HumanReadableFileSize(file.Length), CreationDate = file.CreationTime });
                        break;
                    }

                default:
                    {
                        foreach (DirectoryInfo folder in FileSystemInfos.Where(i => i.Attributes == FileAttributes.Directory))
                            Items.Add(new FolderModel { Name = folder.Name, LastChanged = folder.LastWriteTime });
                        break;
                    }
            }

            return Items;
        }


        public static GridView CreateColumns(DisplayMode displayMode, string path)
        {
            if (path == PATH_NOT_CHOSEN)
                return null;

            GridView gridView = new GridView();

            if (displayMode == DisplayMode.Files)
            {
                gridView.Columns.Add(new GridViewColumn { Header = "Name", DisplayMemberBinding = new Binding("Name") });
                gridView.Columns.Add(new GridViewColumn { Header = "Extension", DisplayMemberBinding = new Binding("Extension") });
                gridView.Columns.Add(new GridViewColumn { Header = "Size", DisplayMemberBinding = new Binding("FileSize") });
                gridView.Columns.Add(new GridViewColumn { Header = "Date", DisplayMemberBinding = new Binding("CreationDate") });
            }
            else
            {
                gridView.Columns.Add(new GridViewColumn { Header = "Name", DisplayMemberBinding = new Binding("Name") });
                gridView.Columns.Add(new GridViewColumn { Header = "Last Changed", DisplayMemberBinding = new Binding("LastChanged") });
            }

            return gridView;
        }

        public static void DeleteItem(string path, BaseModel Item)
        {
            //удалить файл
            Item.Delete(Path.Combine(path, Item.Name));
            //удалить объект
            Items.Remove(Item);
        }

        public static void MoveItem(string source, BaseModel Item)
        {
            string destination = ChooseFilePathDialogResut();
            if (destination != PATH_NOT_CHOSEN)
                Item.Move(Path.Combine(source, Item.Name), Path.Combine(destination, Item.Name));

            //удалить объект
            Items.Remove(Item);
        }

        public static void RenameItem(string path, BaseModel Item, string newName)
        {
            if (newName != string.Empty)
            {
                Item.Move(Path.Combine(path, Item.Name), Path.Combine(path, newName));
                Item.Name = newName;
            }
        }
    }
}
