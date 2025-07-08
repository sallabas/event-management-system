using MAS_Project.Models.Base;

[Serializable]
public class Enrollment
{
    private static List<Enrollment> _enrollments = new();
    public static IReadOnlyList<Enrollment> GetExtent() => _enrollments.AsReadOnly();

    private User _user;
    public User User => _user;
    
    private Event _event;
    public Event Event => _event;
    public DateTime EnrollmentDate { get; }

    public Enrollment(User user, Event ev, DateTime date)
    {
        if (user == null || ev == null)
            throw new ArgumentNullException();

        // do not allow duplicated or repeated enrollment
        if (IsDuplicate(user, ev))
            throw new InvalidOperationException("User is already enrolled in this event.");

        _user = user;
        _event = ev;
        EnrollmentDate = date;

        user.AddEnrollment(this);
        ev.AddEnrollment(this);
        _enrollments.Add(this);
    }

    private bool IsDuplicate(User user, Event ev)
    {
        return _enrollments.Any(e => e.User == user && e.Event == ev);
    }

    public void Remove()
    {
        User.RemoveEnrollment(this);
        Event.RemoveEnrollment(this);
        _enrollments.Remove(this);
    }
}