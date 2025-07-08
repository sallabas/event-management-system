using System;

namespace MAS_Project.Models.Base
{
    [Serializable]
    public class Message
    {
        private static List<Message> _extent = new();
        public static IReadOnlyList<Message> GetExtent() => _extent.AsReadOnly();

        private static int _idCounter = 1;
        private string _content = null!;
        private DateTime _sentDate;
        private DateTime? _viewDate;
        
        private User? _sender;
        private User? _receiver;

        public int MessageId { get; private set; }

        public string Content
        {
            get => _content;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Message content cannot be empty.");
                _content = value;
            }
        }

        public DateTime SentDate
        {
            get => _sentDate;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Sent date cannot be in the future.");
                _sentDate = value;
            }
        }

        public DateTime? ViewDate
        {
            get => _viewDate;
            set => _viewDate = value;
        }

        public bool Received => ViewDate.HasValue;

        public User Sender
        {
            get => _sender ?? throw new InvalidOperationException("Sender has not been set.");
            private set
            {
                _sender = value ?? throw new ArgumentNullException(nameof(Sender), "Sender cannot be null.");
            }
        }

        public User Receiver
        {
            get => _receiver ?? throw new InvalidOperationException("Receiver has not been set.");
            private set
            {
                _receiver = value ?? throw new ArgumentNullException(nameof(Receiver), "Receiver cannot be null.");
            }
        }
        
        // reflexive set up - it supports reflexive association !!! WARNING !!!
        public void SetParticipants(User sender, User receiver)
        {
            if (sender == null || receiver == null)
                throw new ArgumentNullException("Sender and receiver cannot be null.");

            _sender = sender;
            _receiver = receiver;

            sender.AddSentMessage(this);
            receiver.AddReceivedMessage(this);
        }

        public Message(int messageId, string content, DateTime sentDate)
        {
            MessageId = _idCounter++;
            Content = content;
            SentDate = sentDate;

            _extent.Add(this);
        }
        
        public void EditContent(string newContent)
        {
            if (string.IsNullOrWhiteSpace(newContent))
                throw new ArgumentException("New content cannot be empty.");

            Content = newContent;
        }
        public void Remove()
        {
            if (_sender != null)
                _sender.RemoveSentMessage(this);

            if (_receiver != null)
                _receiver.RemoveReceivedMessage(this);

            _extent.Remove(this);
        }
    }
}