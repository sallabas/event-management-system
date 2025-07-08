using System;
using System.Collections.Generic;
using System.IO;

namespace MAS_Project.Models.Base
{
    [Serializable]
    public abstract class User
    {

        // protected cause we are using for removal for child classes
        protected static List<User> _users = new();
        public static IReadOnlyList<User> GetExtent() => _users.AsReadOnly();
        
        private static int _idCounter = 1;
        private string _username = null!;
        private string _email = null!;
        private string _password = null!;
        private DateTime _dateOfBirth;

        public int UserId { get; private set; }

        public string Username
        {
            get => _username;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Username cannot be empty.");
                _username = value;
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                    throw new ArgumentException("Invalid email format.");
                _email = value;
            }
        }

        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                if (value >= DateTime.Now)
                    throw new ArgumentException("Date of birth must be in the past.");
                _dateOfBirth = value;
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Password cannot be empty.");
                _password = value;
            }
        }
        
        public FileInfo? ProfilePicture { get; set; }

        public UserType UserTypes { get; set; }

        // public PaymentDetail? PaymentDetail { get; set; }

        // public List<Event> EnrolledEvents { get; set; } = new();
        
        private PaymentDetail? _paymentDetail;
        public PaymentDetail? PaymentDetail => _paymentDetail;

        // Derived Attributes - Calculating dynamically, do not add inside constructor!!!!!!
        public int FollowingCount => _following.Count;
        public int FollowersCount => _followers.Count;

        // Reverse connection 
        private readonly List<Message> _messagesReceived = new();
        private readonly List<Message> _messagesSent = new();
        
        private readonly List<Enrollment> _enrollments = new();
        public IReadOnlyList<Enrollment> Enrollments => _enrollments.AsReadOnly();

        private readonly List<DiscussionComment> _comments = new();
        public IReadOnlyList<DiscussionComment> Comments => _comments.AsReadOnly();

        
        public IReadOnlyList<Message> MessagesReceived => _messagesReceived.AsReadOnly();
        public IReadOnlyList<Message> MessagesSent => _messagesSent.AsReadOnly();
        
        private readonly HashSet<User> _following = new();
        private readonly HashSet<User> _followers = new();

        public IReadOnlyCollection<User> GetFollowers() => _followers;
        public IReadOnlyCollection<User> GetFollowing() => _following;
        
        
        // Consturctor
        protected User(string username, string email, DateTime dateOfBirth, string password)
        {
            UserId = _idCounter++;
            Username = username;
            Email = email;
            DateOfBirth = dateOfBirth;
            Password = password;
            
            _users.Add(this);
        }

        
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
        

        public void Follow(User other)
        {
            if (other == this) return;
            if (_following.Add(other))
            {
                other._followers.Add(this);
            }
        }

        public void Unfollow(User other)
        {
            if (_following.Remove(other))
            {
                other._followers.Remove(this);
            }
        }
        
        public void BeFollowedBy(User user)
        {
            if (user == this) return;
            if (_followers.Add(user))
            {
                user._following.Add(this);
            }
        }

        public void BeUnfollowedBy(User user)
        {
            if (_followers.Remove(user))
            {
                user._following.Remove(this);
            }
        }
        
        
        
        
        // Message methods 
        // this saves and inform that user get a message
        public void AddSentMessage(Message message)
        {
            if (!_messagesSent.Contains(message))
                _messagesSent.Add(message);
        }

        public void AddReceivedMessage(Message message)
        {
            if (!_messagesReceived.Contains(message))
                _messagesReceived.Add(message);
        }
        
        // this creates new message
        public Message SendMessage(User recipient, string content)
        {
            if (recipient == null)
                throw new ArgumentNullException(nameof(recipient), "Recipient cannot be null.");

            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Message content cannot be empty.");

            var message = new Message(_messagesSent.Count + 1, content, DateTime.Now);
            message.SetParticipants(this, recipient);
            return message;
        }
        
        public void EditSentMessage(int messageId, string newContent)
        {
            var message = _messagesSent.Find(m => m.MessageId == messageId);

            if (message == null)
                throw new InvalidOperationException("No message with that ID exists in sent messages.");

            message.EditContent(newContent);
        }

        /*
         TO WORK ON ID'S ARE WAY MORE SLOWER APPROACH !!!! ASK IT IN CONSULTATION
         
        public void RemoveSentMessage(int messageId)
        {
            var message = _messagesSent.Find(m => m.MessageId == messageId);

            if (message == null)
                throw new InvalidOperationException("No message with that ID exists in sent messages.");

            message.Remove(); 
        }
        
        public void RemoveReceivedMessage(int messageId)
        {
            var message = _messagesReceived.Find(m => m.MessageId == messageId);

            if (message == null)
                throw new InvalidOperationException("No message with that ID exists in received messages.");

            message.Remove();
        }
        */
        
        public void RemoveSentMessage(Message message)
        {
            _messagesSent.Remove(message);
        }

        public void RemoveReceivedMessage(Message message)
        {
            _messagesReceived.Remove(message);
        }
        
        // .Add .Remove doesnt exist because i set the relation as 0..1 !! do not confuse later aqqq!!!!
        public void SetPaymentDetail(PaymentDetail detail)
        {
            if (detail == null)
                throw new ArgumentNullException(nameof(detail), "Payment detail cannot be null.");
            
            if (_paymentDetail != null)
                throw new InvalidOperationException("This user already has a payment detail.");

            _paymentDetail = detail;
        }

        public void RemovePaymentDetail()
        {
            _paymentDetail = null;
        }
        
        
    }
    

    [Flags]
    public enum UserType
    {
        Regular = 1,
        Organizer = 2
    }
}
