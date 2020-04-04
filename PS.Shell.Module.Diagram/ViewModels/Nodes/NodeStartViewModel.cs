﻿using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace PS.Shell.Module.Diagram.ViewModels.Nodes
{
    [DependencyRegisterAsSelf]
    public class NodeStartViewModel : BaseNodeViewModel,
                                      IViewModel
    {
    }
}