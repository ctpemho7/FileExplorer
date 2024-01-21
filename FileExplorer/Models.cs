using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer
{
    public enum DisplayMode
    {
        Files,
        Folders
    }


    public class BaseModel : INotifyPropertyChanged
    {
        string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public virtual void Delete(string filepath) { }
        public virtual void Move(string source, string destination) { }
        public virtual void Rename(string source, string destination) 
        {
            Move(source, destination);
        }


        // для отображения изменения свойств в ObservableCollection
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class FileModel : BaseModel
    {
        // размер файла - в байтах - с помощью Utils перевод в человекочитаемый вид
        public string FileSize { get; set; }
        // то, что идет после точки
        public string Extension { get; set; }

        public DateTime CreationDate { get; set; }


        public override void Delete(string filepath)
        {
            File.Delete(filepath);
        }

        public override void  Move(string source, string destination)
        {
            File.Move(source, destination);
        }
    }

    public class FolderModel : BaseModel
    {
        public DateTime LastChanged { get; set; }

        public override void Delete(string filepath)
        {
            Directory.Delete(filepath, recursive: true);
        }

        public override void Move(string source, string destination)
        {
            Directory.Move(source, destination);
        }
    }
}
