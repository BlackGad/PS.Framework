using System.Threading.Tasks;

namespace PS.MVVM.Services.WindowService;

public interface IWindowService
{
    TViewModel Show<TViewModel>(TViewModel viewModel = default, string key = null);

    Task<TViewModel> ShowModalAsync<TViewModel>(TViewModel viewModel = default, string key = null);
}
