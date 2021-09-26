using System.IO;
using PS.IoC.Attributes;

namespace PS.Commander.ViewModels
{
    [DependencyRegisterAsSelf]
    public class FileSystemItemViewModel : BaseNotifyPropertyChanged
    {
        #region Constructors

        public FileSystemItemViewModel(FileSystemInfo info)
        {
            Name = info.Name;
            Path = info.FullName;
            Extension = info.Extension.TrimStart('.');
        }

        #endregion

        #region Properties

        public string Extension { get; }
        public string Name { get; }
        public string Path { get; }

        #endregion
    }
}