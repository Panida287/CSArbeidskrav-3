using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace EventPlatform.Avalonia.ViewModels;

/// <summary>
/// ViewModel for the Login / Register view.
/// </summary>
public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty] private string _username = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private string? _errorMessage;

    [RelayCommand]
    private void Login() => throw new NotImplementedException();

    [RelayCommand]
    private void Register() => throw new NotImplementedException();
}
