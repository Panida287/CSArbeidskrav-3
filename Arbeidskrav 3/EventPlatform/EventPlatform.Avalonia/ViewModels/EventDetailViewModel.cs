using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventPlatform.Models.Events;

namespace EventPlatform.Avalonia.ViewModels;

/// <summary>
/// ViewModel for the event detail view.
/// </summary>
public partial class EventDetailViewModel : ObservableObject
{
    [ObservableProperty] private Event? _event;

    [RelayCommand]
    private void Book(int ticketTypeId) => throw new NotImplementedException();
}
