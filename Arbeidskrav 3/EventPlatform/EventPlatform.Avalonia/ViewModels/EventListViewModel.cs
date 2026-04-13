using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using EventPlatform.Models.Events;

namespace EventPlatform.Avalonia.ViewModels;

/// <summary>
/// ViewModel for the main event list / browse screen.
/// </summary>
public partial class EventListViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<Event> _events = new();
    [ObservableProperty] private string _searchQuery = string.Empty;

    [RelayCommand]
    private void Search() => throw new NotImplementedException();

    [RelayCommand]
    private void OpenDetail(Event ev) => throw new NotImplementedException();
}
