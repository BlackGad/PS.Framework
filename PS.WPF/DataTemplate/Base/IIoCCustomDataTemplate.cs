using System;

namespace PS.WPF.DataTemplate.Base
{
    public interface IIoCCustomDataTemplate : ICustomDataTemplate
    {
        Func<Type, string, object> FrameworkElementResolver { get; set; }
    }
}