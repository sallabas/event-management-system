using System;
using System.Collections.Generic;
using System.Linq;

namespace MAS_Project.Models.Base
{
    [Serializable]
    public class Venue
    {
        private static List<Venue> _venues = new();
        public static IReadOnlyList<Venue> GetExtent() => _venues.AsReadOnly();

        private static int _idCounter = 1;

        public int VenueId { get; private set; }

        private string _venueName = null!;
        public string VenueName
        {
            get => _venueName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Venue name cannot be empty.");
                _venueName = value;
            }
        }

        private Location _location = null!;
        public Location Location
        {
            get => _location;
            set
            {
                _location = value ?? throw new ArgumentNullException(nameof(Location), "Location cannot be null.");
                _location.AddVenue(this); // 🔁 ilişkiyi burada kur
            }
        }
        
        private readonly List<Event> _events = new();
        public IReadOnlyList<Event> Events => _events.AsReadOnly();

        public Venue(string venueName)
        {
            VenueName = venueName;

            VenueId = _idCounter++;
            _venues.Add(this);
            
        }

        public void AddEvent(Event e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (!_events.Contains(e)) _events.Add(e);
        }

        public void RemoveEvent(Event e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            _events.Remove(e);
        }

        public void Remove()
        {
            foreach (var e in _events.ToList())
                e.Remove();

            _location.RemoveVenue(this);
            _venues.Remove(this);
        }
    }
}