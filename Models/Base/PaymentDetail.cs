using System;
using System.Collections.Generic;
using System.Linq;

namespace MAS_Project.Models.Base
{
    [Serializable]
    public class PaymentDetail
    {
        private static List<PaymentDetail> _paymentDetails = new();
        public static IReadOnlyList<PaymentDetail> GetExtent() => _paymentDetails.AsReadOnly();
        
        private readonly List<Transaction> _transactions = new();
        public IReadOnlyList<Transaction> Transactions => _transactions.AsReadOnly();

        private static int _idCounter = 1;

        public int PaymentDetailsId { get; private set; }

        public string AccountName { get; set; }
        public string IBAN { get; set; }

        public PaymentDetail(string accountName, string iban)
        {
            if (string.IsNullOrWhiteSpace(accountName))
                throw new ArgumentException("Account name cannot be empty.");

            if (string.IsNullOrWhiteSpace(iban))
                throw new ArgumentException("IBAN cannot be empty.");

            PaymentDetailsId = _idCounter++;
            AccountName = accountName;
            IBAN = iban;

            _paymentDetails.Add(this);
        }

        public Transaction AddTransaction(double amount, DateTime date)
        {
            var transaction = new Transaction(amount, date, this);
            _transactions.Add(transaction);
            return transaction;
        }

        public void RemoveTransaction(Transaction transaction)
        {
            if (transaction == null || !_transactions.Contains(transaction))
                throw new ArgumentException("Transaction not found in this payment detail.");

            _transactions.Remove(transaction);
        }
        
        public void Remove()
        {
            foreach (var t in _transactions.ToList())
            {
                t.Remove(); 
            }

            _paymentDetails.Remove(this);
        }
        
    }
}