using System;
using System.Collections.Generic;

namespace MAS_Project.Models.Base
{
    [Serializable]
    public class PromotedRequest
    {
        private static List<PromotedRequest> _promotedRequests = new();
        public static IReadOnlyList<PromotedRequest> GetExtent() => _promotedRequests.AsReadOnly();
        
        private static int _idCounter = 1;

        public int PromotedRequestId { get; }

        public Event TargetEvent { get; }
        public Organizer Organizer { get; }
        public DateTime RequestDate { get; }
        public PromotionStatus Status { get; private set; }

        public Transaction LinkedTransaction { get; }

        public static double MinCostOfPromotion = 10.0;

        public PromotedRequest(Event targetEvent, Organizer organizer, DateTime requestDate, double amount)
        {
            if (targetEvent == null) throw new ArgumentNullException(nameof(targetEvent), "Target event cannot be null.");
            
            if (organizer == null) throw new ArgumentNullException(nameof(organizer), "organizer cannot be null.");
            
            if (organizer.PaymentDetail == null)
                throw new InvalidOperationException("Organizer must have a PaymentDetail before requesting promotion.");

            if (amount < MinCostOfPromotion)
                throw new ArgumentException($"Amount must be at least {MinCostOfPromotion}.", nameof(amount));

            PromotedRequestId = _idCounter++;
            TargetEvent = targetEvent;
            Organizer = organizer;
            RequestDate = requestDate;
            Status = PromotionStatus.Pending;

            // create automatic transaction and link it !!! consult this !!!
            LinkedTransaction = organizer.PaymentDetail.AddTransaction(amount, requestDate);

            _promotedRequests.Add(this);
        }

        public void Confirm()
        {
            Status = PromotionStatus.Confirmed;
        }

        public void Fail()
        {
            Status = PromotionStatus.Failed;
        }

        
        public void RemovePromotedRequest()
        {
            LinkedTransaction.Remove();
            _promotedRequests.Remove(this);
        }
    }

    public enum PromotionStatus
    {
        Pending,
        Confirmed,
        Failed
    }
}
