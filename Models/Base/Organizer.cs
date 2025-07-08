using System;

namespace MAS_Project.Models.Base
{
    [Serializable]
    public class Organizer : User
    {
        private static List<Organizer> _organizers = new();
        public static IReadOnlyList<Organizer> GetExtent() => _organizers.AsReadOnly();
        
        private readonly List<PromotedRequest> _promotedRequests = new();
        public IReadOnlyList<PromotedRequest> PromotedRequests => _promotedRequests.AsReadOnly();
        
        private readonly List<Event> _events = new();
        public IReadOnlyList<Event> Events => _events.AsReadOnly();

        
        private string _businessDescription = string.Empty;
        private double _earning;

        public string BusinessDescription
        {
            get => _businessDescription;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Business description cannot be empty.");
                _businessDescription = value;
            }
        }

        public bool IsMonetized { get; set; }

        public double Earning
        {
            get => _earning;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Earning cannot be negative.");
                _earning = value;
            }
        }
        
        public Organizer(string username, string email, DateTime dateOfBirth, string password,
            string businessDescription, bool isMonetized, double earning)
            : base(username, email, dateOfBirth, password)
        {
            BusinessDescription = businessDescription;
            IsMonetized = isMonetized;
            Earning = earning;
            UserTypes = UserType.Organizer;
            
            _organizers.Add(this); 
        }

        
        public void AddPromotedRequest(PromotedRequest pr)
        {
            if (pr == null)
                throw new ArgumentNullException(nameof(pr));
            if (!_promotedRequests.Contains(pr))
                _promotedRequests.Add(pr);
        }

        public void RemovePromotedRequest(PromotedRequest pr)
        {
            if (pr == null)
                throw new ArgumentNullException(nameof(pr));
            _promotedRequests.Remove(pr);
        }
        
        public void AddEvent(Event e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            if (!_events.Contains(e))
                _events.Add(e);
        }

        public void RemoveEvent(Event e)
        {
            if (_events.Contains(e))
                _events.Remove(e);
        }
        
        
        public static void RemoveOrganizer(Organizer organizer)
        {
            // 1. Mesajları temizle
            foreach (var msg in organizer.MessagesSent.ToList())
                msg.Remove();

            foreach (var msg in organizer.MessagesReceived.ToList())
                msg.Remove();

            // 2. Takip ilişkilerini kopar
            foreach (var followed in organizer.GetFollowing().ToList())
                organizer.Unfollow(followed);

            foreach (var follower in organizer.GetFollowers().ToList())
                follower.Unfollow(organizer);

            // 3. Extent’lerden çıkar
            _organizers.Remove(organizer);
            _users.Remove(organizer);
        }

        
    }
}