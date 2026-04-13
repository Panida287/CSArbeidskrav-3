using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using EventPlatform.Models;

namespace EventPlatform.Avalonia.ViewModels;

/// <summary>
/// ViewModel for the My Bookings view.
/// </summary>
public partial class BookingViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<Booking> _upcomingBookings = new();
    [ObservableProperty] private ObservableCollection<Booking> _pastBookings = new();

    [RelayCommand]
    private void Cancel(int bookingId) => throw new NotImplementedException();
}
