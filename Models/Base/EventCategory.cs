using System;
using System.Collections.Generic;
using System.Linq;

namespace MAS_Project.Models.Base
{
    [Serializable]
    public class EventCategory
    {
        private static List<EventCategory> _categories = new();
        public static IReadOnlyList<EventCategory> GetExtent() => _categories.AsReadOnly();

        private static int _idCounter = 1;

        public int EventCategoryId { get; private set; }

        private string _categoryName = null!;
        private string _categoryDescription = null!;
        
        public string CategoryName
        {
            get => _categoryName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Category name cannot be empty.");
                _categoryName = value;
            }
        }

        public string CategoryDescription
        {
            get => _categoryDescription;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Category description cannot be empty.");
                _categoryDescription = value;
            }
        }

        private readonly List<Event> _events = new();
        public IReadOnlyList<Event> Events => _events.AsReadOnly();
        
        public EventCategory(string name, string description)
        {
            CategoryName = name;
            CategoryDescription = description;
            EventCategoryId = _idCounter++;

            _categories.Add(this);
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
                e.RemoveCategory(this); 
            _categories.Remove(this);
        }
    }
}
