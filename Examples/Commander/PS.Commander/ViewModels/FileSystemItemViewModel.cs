using System.IO;
using PS.IoC.Attributes;

namespace PS.Commander.ViewModels
{
    [DependencyRegisterAsSelf]
    public class FileSystemItemViewModel : BaseNotifyPropertyChanged
    {
        public FileSystemItemViewModel(FileSystemInfo info)
        {
            Name = info.Name;
            Path = info.FullName;
            Extension = info.Extension.TrimStart('.');
        }

        public string Extension { get; }

        public string Name { get; }

        public string Path { get; }
    }
}
