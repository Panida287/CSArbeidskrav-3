using EventPlatform.Services;

namespace EventPlatform.UI.Menus;

/// <summary>
/// Browse, Search, Filter, Event Detail, Create Event, My Events screens.
/// </summary>
public class EventMenu
{
    private readonly EventService _eventService;
    private readonly UserService _userService;

    public EventMenu(EventService eventService, UserService userService)
    {
        _eventService = eventService;
        _userService = userService;
    }

    public void ShowBrowse() => throw new NotImplementedException();
    public void ShowSearch() => throw new NotImplementedException();
    public void ShowFilter() => throw new NotImplementedException();
    public void ShowDetail(int eventId) => throw new NotImplementedException();
    public void ShowCreate() => throw new NotImplementedException();
    public void ShowMyEvents() => throw new NotImplementedException();
}
