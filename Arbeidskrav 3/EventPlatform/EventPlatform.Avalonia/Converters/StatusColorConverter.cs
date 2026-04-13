using Avalonia.Data.Converters;
using Avalonia.Media;
using System.Globalization;

namespace EventPlatform.Avalonia.Converters;

/// <summary>
/// Converts EventStatus to a display colour — e.g. Upcoming → green, Cancelled → red.
/// </summary>
public class StatusColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value?.ToString() switch
        {
            "Upcoming"  => Brushes.Green,
            "Completed" => Brushes.Gray,
            "Cancelled" => Brushes.Red,
            _           => Brushes.Transparent
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
