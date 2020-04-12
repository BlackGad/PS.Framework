﻿using System.ComponentModel;
using PS.Patterns.Aware;

namespace PS.Shell.Module.Diagram.Controls.MVVM
{
    public interface INodeVisual : IIsSelectedAware,
                                   INotifyPropertyChanged
    {
    }
}