using System;
using System.Collections.Generic;
using System.Linq;

namespace MAS_Project.Models.Base
{
    [Serializable]
    public class Event
    {
        private static List<Event> _events = new();
        public static IReadOnlyList<Event> GetExtent() => _events.AsReadOnly();

        private static int _idCounter = 1;

        private string _eventTitle = null!;
        private string _description = null!;
        private DateTime _startDate;
        private DateTime _endDate;
        private int _availableSpots;
        
        public int EventId { get; private set; }

        
        public string EventTitle
        {
            get => _eventTitle;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Title cannot be empty.");
                _eventTitle = value;
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Description cannot be empty.");
                _description = value;
            }
        }

        // validations taken into constructor due to compilation error!!!!! 
        public DateTime StartDate
        {
            get => _startDate;
            set => _startDate = value;
        }

        public DateTime EndDate
        {
            get => _endDate;
            set => _endDate = value;
        }

        public int AvailableSpots
        {
            get => _availableSpots;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Available spots must be greater than zero.");
                _availableSpots = value;
            }
        }

        private readonly List<string> _tags = new();
        public IReadOnlyList<string> Tags => _tags.AsReadOnly();
        
        private readonly List<EventCategory> _categories = new();
        public IReadOnlyList<EventCategory> Categories => _categories.AsReadOnly();
        
        private readonly List<Enrollment> _enrollments = new();
        public IReadOnlyList<Enrollment> Enrollments => _enrollments.AsReadOnly();

        private readonly List<DiscussionComment> _comments = new();
        public IReadOnlyList<DiscussionComment> Comments => _comments.AsReadOnly();


        private Venue _venue = null!;
        public Venue Venue
        {
            get => _venue;
            set
            {
                _venue = value ?? throw new ArgumentNullException(nameof(Venue), "Venue cannot be null.");
                _venue.AddEvent(this); 
            }
        }
        
        public Organizer Organizer { get; private set; }
        public PromotedRequest? PromotedRequest { get; private set; }
        

        public Event(string title, string description, DateTime startDate, DateTime endDate,
                     int availableSpots, Organizer organizer, IEnumerable<EventCategory>? categories = null,
                     IEnumerable<string>? tags = null)
        {
                        
            if (startDate >= endDate)
                throw new ArgumentException("Start date must be before end date.");
            
            EventId = _idCounter++;

            EventTitle = title;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            AvailableSpots = availableSpots;

            Organizer = organizer ?? throw new ArgumentNullException(nameof(organizer));
            organizer.AddEvent(this); // Add the event into list in organizer.cs, 

            
            if (categories != null)
            {
                foreach (var cat in categories.Where(c => c != null))
                    AddCategory(cat);
            }

            if (tags != null)
                _tags.AddRange(tags.Where(tag => !string.IsNullOrWhiteSpace(tag)));
            
            _events.Add(this);
        }
        
        // add remove methods set only the relation !!! do not confuse later on 
        public void AddEnrollment(Enrollment enrollment)
        {
            if (!_enrollments.Contains(enrollment))
                _enrollments.Add(enrollment);
        }

        public void RemoveEnrollment(Enrollment enrollment)
        {
            _enrollments.Remove(enrollment);
        }

        public void AddComment(DiscussionComment comment)
        {
            if (!_comments.Contains(comment))
                _comments.Add(comment);
        }

        public void RemoveComment(DiscussionComment comment)
        {
            _comments.Remove(comment);
        }

        
        public void AddCategory(EventCategory category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));
            if (!_categories.Contains(category))
                _categories.Add(category);
        }

        public void RemoveCategory(EventCategory category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));
            _categories.Remove(category);
        }
        

        public void AddTag(string tag)
        {
            if (!string.IsNullOrWhiteSpace(tag) && !_tags.Contains(tag))
                _tags.Add(tag);
        }

        public void RemoveTag(string tag)
        {
            _tags.Remove(tag);
        }
        

        public void SetPromotedRequest(PromotedRequest request)
        {
            PromotedRequest = request ?? throw new ArgumentNullException(nameof(request));
        }

        
        public void Remove()
        {
            PromotedRequest?.RemovePromotedRequest();
            
            // Clear eventcategory relation 
            foreach (var cat in _categories.ToList())
                cat.RemoveEvent(this);
            
            foreach (var enrollment in _enrollments.ToList())
                enrollment.Remove();

            foreach (var comment in _comments.ToList())
                comment.Remove();
            
            Venue.RemoveEvent(this);
            
            _events.Remove(this);
        }
        
        
    }
}
