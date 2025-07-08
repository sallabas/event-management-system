using System;

namespace MAS_Project.Models.Base
{
    [Serializable]
    public class RegularUser : User
    {
        private static List<RegularUser> _regularUsers = new();
        public static IReadOnlyList<RegularUser> GetExtent() => _regularUsers.AsReadOnly();
        
        
        private string _address = string.Empty;

        public string Address
        {
            get => _address;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Address cannot be empty.");
                _address = value;
            }
        }
        
        public RegularUser(string username, string email, DateTime dateOfBirth, string password, string address)
            : base(username, email, dateOfBirth, password)
        {
            Address = address;
            UserTypes = UserType.Regular;
            
            _regularUsers.Add(this); 
        }
        
        public static void RemoveRegularUser(RegularUser user)
        {
            // Clearing each instances from each relation !!!! Consult it 
            // Clearing message instances
            foreach (var msg in user.MessagesSent.ToList())
                msg.Remove();

            foreach (var msg in user.MessagesReceived.ToList())
                msg.Remove();

            // Clearing follo0w instances
            foreach (var followed in user.GetFollowing().ToList())
                user.Unfollow(followed);

            foreach (var follower in user.GetFollowers().ToList())
                follower.Unfollow(user);
            
            
            _regularUsers.Remove(user);
            _users.Remove(user);
        }


    }
}