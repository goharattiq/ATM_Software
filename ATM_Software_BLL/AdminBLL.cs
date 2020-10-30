using ATM_Software_BO;
using ATM_Software_DAL;
using System;
using System.Collections;

namespace ATM_Software_BLL
{
    public class AdminBLL
    {
        public int createAccount(Person person , Credentials credentials, Account account)
        {
            AdminDAL dal = new AdminDAL();
            int accountNo = ATMDAL.getLastNumber("account.csv") + 1;
            int personID = ATMDAL.getLastNumber("users.csv") + 1;
            person.id = personID;
            credentials.ID = personID;
            person.accountNo = accountNo;
            account.ID = person.accountNo;
            credentials.Password = Authentication.encryption(credentials.Password);
            dal.savePerson(person);
            dal.saveCredentials(credentials);
            dal.saveAccount(account);
            return accountNo;
        }
        public void deleteAccount(int accountNo,int personId)
        {
            AdminDAL dal = new AdminDAL();
            dal.deletePerson(personId);
            dal.deleteCredentials(personId);
            dal.deleteAccount(accountNo);
        }
        public (ArrayList,ArrayList) searchAccount(int accountID,int userID,string holderName,int accountType,int accountStatus,decimal balance)
        {
            ArrayList searchAccounts = new ArrayList();
            ArrayList searchPersons = new ArrayList();
            Person person = (Person)getPerson(-2,userID, holderName)?[0];            
            if(person != null)
            {
                searchAccounts.Add((Account)getAccount(person.accountNo)[0]);
                searchPersons.Add(person);
                return (searchPersons, searchAccounts);
            }
            ArrayList accounts = getAccount(accountID);
            if (accounts.Count == 1)
            {
                Account account = (Account)accounts[0];
                searchPersons.Add((Person)getPerson(accountID)[0]);
                searchAccounts.Add(account);
                return (searchPersons, searchAccounts);
            }

            foreach(Account acc in accounts)
            {
                bool meetCriteria = false;
                
                if (accountType == -2 || (Account.AccountType)accountType == acc.Type)
                    meetCriteria = true;
                else
                    continue;
                if (accountStatus == -2 || (Account.AccountStatus)accountStatus == acc.Status)
                    meetCriteria = true;
                else
                    continue;
                if (balance == 0 || balance == acc.Balance)
                    meetCriteria = true;
                else
                    continue;
                if (meetCriteria)
                {
                    searchPersons.Add((Person)getPerson(acc.ID)[0]);
                    searchAccounts.Add(acc);
                }
            }
            return (searchPersons,searchAccounts);
        }

        public ArrayList getPerson(int accountNo,int userID = -2,string holderName = "")
        {
            AdminDAL dal = new AdminDAL();
            ArrayList details = dal.getPersonDetails("users.csv",accountNo,userID,holderName);
            ArrayList persons = new ArrayList();
            if (details != null && details.Count > 0)
            {
                foreach (string line in details)
                {
                    
                    Person person = new Person(line.Split(",")[1], 0);
                    person.accountNo = int.Parse(line.Split(",")[3]);
                    person.id = int.Parse(line.Split(",")[0]);
                    persons.Add(person);
                }
                return persons;
            }
            return null;
        }

        public ArrayList getAccount(int accountNo,int accountType = -2, int accountStatus = -2,decimal balance = -2)
        {
            AdminDAL dal = new AdminDAL();
            ArrayList details = dal.getAccountDetails("account.csv", accountNo);
            ArrayList accounts = new ArrayList();
            if (details != null && details.Count > 0)
            {
                foreach(string detail in details)
                {
                    Account account = new Account(decimal.Parse(detail.Split(",")[1]),
                                int.Parse(detail.Split(",")[3]), int.Parse(detail.Split(",")[2]));
                    account.ID = int.Parse(detail.Split(",")[0]);
                    accounts.Add(account);
                }
                return accounts;
            }
            return null;
        }
        public Credentials getCredentials(int personID)
        {
            AdminDAL dal = new AdminDAL();
            ArrayList details = dal.getCredentialDetails("credentials.csv", personID);

            if (details != null && details.Count > 0)
            {
                string detail = (string)details[0];
                Credentials credentials = new Credentials(detail.Split(",")[1], detail.Split(",")[2]);
                credentials.ID = int.Parse(detail.Split(",")[0]);
                return credentials;
            }
            return null;
        }
        public (ArrayList, ArrayList) accountsByAmount(decimal minBalance, decimal maxBalance)
        {
            ArrayList searchAccounts = new ArrayList();
            ArrayList searchPersons = new ArrayList();
            AdminDAL dal = new AdminDAL();
            ArrayList list = dal.accountsByDate("account.csv",minBalance,maxBalance);
            if(list.Count != 0)
            {
                foreach(string line in list)
                {
                    Account account = new Account(decimal.Parse(line.Split(",")[1]), int.Parse(line.Split(",")[3]), int.Parse(line.Split(",")[2]));
                    account.ID = int.Parse(line.Split(",")[0]);
                    searchAccounts.Add(account);
                    searchPersons.Add((Person)getPerson(account.ID)[0]);
                }
            }
            return (searchPersons,searchAccounts);
        }
        public (ArrayList,ArrayList) transactionByDates(DateTime startDate, DateTime endDate)
        {
            ArrayList searchTransactions = new ArrayList();
            ArrayList searchPersons = new ArrayList();
            AdminDAL dal = new AdminDAL();
            ArrayList list = dal.transactionByDates("transactions.csv", startDate, endDate);
            if (list.Count != 0)
            {
                foreach (string line in list)
                {
                    Transaction transaction = new Transaction(int.Parse(line.Split(",")[1]), DateTime.Parse(line.Split(",")[2]),
                        (Transaction.TranscationType)int.Parse(line.Split(",")[3]), decimal.Parse(line.Split(",")[4]));
                    transaction.Id = int.Parse(line.Split(",")[0]);
                    searchTransactions.Add(transaction);
                    searchPersons.Add((Person)getPerson(transaction.AccountNo)[0]);
                }
            }
            return (searchPersons,searchTransactions);
        }
    }
}