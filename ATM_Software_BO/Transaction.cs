using System;

namespace ATM_Software_BO
{
    public class Transaction
    {
        public enum TranscationType
        {
            WithDrawn,
            Transfer,
            Deposit,
        }
        public int Id { get; set; }
        public int AccountNo { get; set; }
        public DateTime Date { get; set; }
        public TranscationType type { get; set; }
        public decimal Amount { get; set; }
        public Transaction(int AccountNo,DateTime date, TranscationType type,decimal Amount)
        {
            this.AccountNo = AccountNo;
            this.Date = date;
            this.type = type;
            this.Amount = Amount;
        }
    }
}