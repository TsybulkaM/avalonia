// ViewModels/ConfirmationDialogViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Reversi.ViewModels;

public partial class ConfirmationDialogViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _message;

    public ConfirmationDialogViewModel(string message)
    {
        _message = message;
    }
}