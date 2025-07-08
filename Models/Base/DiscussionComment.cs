using MAS_Project.Models.Base;

[Serializable]
public class DiscussionComment
{
    private static List<DiscussionComment> _comments = new();
    public static IReadOnlyList<DiscussionComment> GetExtent() => _comments.AsReadOnly();

    private User _author;
    public User Author => _author;

    private Event _targetEvent;
    public Event TargetEvent => _targetEvent;
    
    public string Content { get; set; }
    public DateTime CreatedAt { get; }

    public DiscussionComment(User user, Event ev, string content)
    {
        // subset relation
        if (!EnrollmentExists(user, ev))
            throw new InvalidOperationException("User must be enrolled to comment.");

        _author = user;
        _targetEvent = ev;
        Content = content;
        CreatedAt = DateTime.Now;

        user.AddComment(this);
        ev.AddComment(this);
        _comments.Add(this);
    }

    private bool EnrollmentExists(User user, Event ev)
    {
        return Enrollment.GetExtent().Any(e => e.User == user && e.Event == ev);
    }

    public void Remove()
    {
        Author.RemoveComment(this);
        TargetEvent.RemoveComment(this);
        _comments.Remove(this);
    }
}