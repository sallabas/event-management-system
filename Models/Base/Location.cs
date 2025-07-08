using System;
using System.Collections.Generic;
using System.Linq;

namespace MAS_Project.Models.Base
{
    [Serializable]
    public class Location
    {
        private static List<Location> _locations = new();
        public static IReadOnlyList<Location> GetExtent() => _locations.AsReadOnly();

        private static int _idCounter = 1;

        public int LocationId { get; private set; }

        private string _country = null!;
        private string _city = null!;
        private string _street = null!;
        private string _postCode = null!;

        private readonly List<Venue> _venues = new();
        public IReadOnlyList<Venue> Venues => _venues.AsReadOnly();

        public string Country
        {
            get => _country;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Country cannot be empty.");
                _country = value;
            }
        }

        public string City
        {
            get => _city;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("City cannot be empty.");
                _city = value;
            }
        }

        public string Street
        {
            get => _street;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Street cannot be empty.");
                _street = value;
            }
        }

        public string PostCode
        {
            get => _postCode;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Post code cannot be empty.");
                _postCode = value;
            }
        }

        public Location(string country, string city, string street, string postCode)
        {
            Country = country;
            City = city;
            Street = street;
            PostCode = postCode;

            LocationId = _idCounter++;
            _locations.Add(this);
        }

        public void AddVenue(Venue venue)
        {
            if (venue == null) throw new ArgumentNullException(nameof(venue));
            if (!_venues.Contains(venue))
                _venues.Add(venue);
        }

        public void RemoveVenue(Venue venue)
        {
            if (venue == null) throw new ArgumentNullException(nameof(venue));
            _venues.Remove(venue);
        }

        public void Remove()
        {
            _locations.Remove(this);
        }
    }
}
