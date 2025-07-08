using System;
using System.Collections.Generic;

namespace MAS_Project.Models.Base
{
    [Serializable]
    public class Transaction
    {
        private static List<Transaction> _transactions = new();
        public static IReadOnlyList<Transaction> GetExtent() => _transactions.AsReadOnly();

        private static int _idCounter = 1;
        
        public int TransactionId { get; private set; }
        public double Amount { get; private set; }
        public DateTime TransactionDate { get; private set; }

        private PaymentDetail _paymentDetail;
        public PaymentDetail PaymentDetail
        {
            get => _paymentDetail;
            private set
            {
                _paymentDetail = value ?? throw new ArgumentNullException(nameof(value), "PaymentDetail cannot be null.");
            }
        }

        public Transaction(double amount, DateTime transactionDate, PaymentDetail paymentDetail)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");

            Amount = amount;
            TransactionDate = transactionDate;
            PaymentDetail = paymentDetail;

            TransactionId = _idCounter++;
            _transactions.Add(this);
        }

        public void EditTransaction(double newAmount, DateTime newDate)
        {
            if (newAmount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");

            Amount = newAmount;
            TransactionDate = newDate;
        }

        public void Remove()
        {
            PaymentDetail?.RemoveTransaction(this);
            _transactions.Remove(this);
        }
    }
}