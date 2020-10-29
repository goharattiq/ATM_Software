namespace ATM_Software_BO
{
    public class Account
    {
        public enum AccountType
        {
            Savings,
            Current
        }
        public enum AccountStatus
        {
            Active,
            Inactive
        }
        public int ID { get; set; }
        public AccountType Type { get; set; }
        public AccountStatus Status { get; set; }
        public decimal Balance { get; set; }
        public Account(decimal balance,int type, int status)
        {
            Balance = balance;
            Type = (AccountType) type;
            Status = (AccountStatus) status;
        }
    }
}