using System;

namespace PS.WPF.DataTemplate.Base
{
    public interface IIoCCustomDataTemplate : ICustomDataTemplate
    {
        #region Properties

        Func<Type, string, object> FrameworkElementResolver { get; set; }

        #endregion
    }
}