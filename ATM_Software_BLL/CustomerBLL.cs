using ATM_Software_BO;
using ATM_Software_DAL;
using System;
using static System.Console;

namespace ATM_Software_BLL
{
    public class CustomerBLL
    {
        public Transaction withDraw(decimal requestedAmount,int accountNo)
        {
            Transaction transaction = null;
            Account account = getAccount(accountNo);
            try
            {
                if (account == null)
                    throw new Exception("\nAccount Did not Found");
                if (requestedAmount > account.Balance || requestedAmount < 0) 
                    throw new Exception("\nWithdraw cannot be posssible because of the low ammount");
                else
                {
                    account.Balance -= requestedAmount;
                    transaction = new Transaction(account.ID, DateTime.Now,
                        Transaction.TranscationType.WithDrawn, requestedAmount);
                    transaction.Id = ATMDAL.getLastNumber("transactions.csv") +1;
                    updateAccount(account);
                    CustomerDAL dal = new CustomerDAL();
                    dal.saveTransiction(transaction);
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            return transaction;
        }
        public Transaction transferAmount(decimal requestedAmount,int customerFrom,int customerTo)
        {
            Transaction transaction = null;
            Account accountfrom = getAccount(customerFrom);
            Account accountTo = getAccount(customerTo);
            accountfrom.Balance -= requestedAmount;
            accountTo.Balance += requestedAmount;
            transaction = new Transaction(accountfrom.ID, DateTime.Now, Transaction.TranscationType.Transfer
                ,requestedAmount);
            transaction.Id = ATMDAL.getLastNumber("transactions.csv") + 1;
            (new CustomerDAL()).saveTransiction(transaction);
            updateAccount(accountfrom);
            updateAccount(accountTo);
            return transaction;
        }
        public Transaction depositCash(decimal requestedAmount, int accountNo)
        {
            Account account = getAccount(accountNo);
            if (account == null) return null;
            Transaction transaction = null;
            account.Balance += requestedAmount;
            transaction = new Transaction(account.ID, DateTime.Now,
                        Transaction.TranscationType.WithDrawn, requestedAmount);
            transaction.Id = ATMDAL.getLastNumber("transactions.csv") + 1;
            updateAccount(account);
            CustomerDAL dal = new CustomerDAL();
            dal.saveTransiction(transaction);
            return transaction;
        }
        public void  updateAccount(Account account)
        {
            AdminDAL dal = new AdminDAL();
            dal.deleteAccount(account.ID);
            dal.saveAccount(account);
        }
        public void displayReciept(Transaction transaction,int accountNo)
        {
            Account account = getAccount(accountNo);
            WriteLine($"\nAccount #{account.ID}\nDate: {DateTime.Now}\n");
            if (transaction != null)
                WriteLine($"{transaction.type}: {transaction.Amount}");
            WriteLine($"Balance: {account.Balance}\n");
        }
        public Person getPerson(int accountNo,int userName= -2)
        {
            CustomerDAL dal = new CustomerDAL();
            string details = dal.getPersonDetails("users.csv", accountNo,userName);

            if (details != null)
            {
                Person person = new Person(details.Split(",")[1], 0);
                person.accountNo = int.Parse(details.Split(",")[3]);
                person.id = int.Parse(details.Split(",")[0]);
                return person;
            }
            return null;
        }
        public Account getAccount(int accountNo)
        {
            CustomerDAL dal = new CustomerDAL();
            string details = dal.getAccountDetails("account.csv", accountNo);
            if (details != null)
            {
                Account account = new Account(decimal.Parse(details.Split(",")[1]),
                    int.Parse(details.Split(",")[3]), int.Parse(details.Split(",")[2]));
                account.ID = int.Parse(details.Split(",")[0]);
                return account;
            }
            return null;
        }
    }
}