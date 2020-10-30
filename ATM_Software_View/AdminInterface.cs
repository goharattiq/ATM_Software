using ATM_Software_BLL;
using ATM_Software_BO;
using System;
using System.Text.RegularExpressions;
using System.Collections;
using static System.Console;

namespace ATM_Software_View
{
    public class AdminInterface : ATMInterface
    {
        public delegate bool adminDelegator();
        public override bool menu(int userType)
        {
            Clear();
            WriteLine("\nWelcome in Admin menu\n1----Create New Account.");
            WriteLine("2----Delete Existing Account.\n3----Update Account Information.");
            WriteLine("4----Search for Account.\n5----View Reports\n6----Exit");
            return selectMenu(GetInput('1', '6'));
        }
        
        public override bool selectMenu(char selectedMenu)
        {
            adminDelegator d1 = null;
            d1 = selectedMenu switch
            {
                '1' => createAccount,
                '2' => deleteAccount,
                '3' => updateAccount,
                '4' => SearchAccount,
                '5' => viewReports,
                '6' => Exit,
                _ => Exit
            };
            return d1.Invoke();
        }
        public bool createAccount()
        {
            Clear();
            decimal balance=0;
            int accountType = 0 , accountStatus = 0;
            string pinCode = String.Empty;
            Write("Login: ");
            ATMInterface.changeTextColor(Color.Green);
            string login = ReadLine();
            ATMInterface.changeTextColor(Color.Gray);
        inputPinCode:
            try {
                Write("Pin Code: ");
                ATMInterface.changeTextColor(Color.Green);
                pinCode = ReadLine();
                if (pinCode.Length != 5 || !Regex.Match(pinCode, @"\d\d\d\d\d").Success)
                    throw new Exception("Pincode is incorrect");
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                goto inputPinCode;
            }
            ATMInterface.changeTextColor(Color.Gray);
            Write("Holders Name: ");
            ATMInterface.changeTextColor(Color.Green);
            string holderName = ReadLine();
            ATMInterface.changeTextColor(Color.Gray);
        inputTypeAgain:
            try {           
                Write("Type(Savings, Current): ");
                ATMInterface.changeTextColor(Color.Green);
                string type = ReadLine();
                ATMInterface.changeTextColor(Color.Gray);
                if (type.ToLower() != "savings" && type.ToLower() != "current") throw new Exception("savings/current");
                else
                {
                    accountType = type.ToLower() switch
                    {
                        "savings" => 0,
                        "current" => 1
                    };
                }
            }
            catch (Exception e)
            {
                errorMessage(e.Message);
                goto inputTypeAgain;
            }
        inputBalanceAgain:
            try {
                Write("Starting Balance: ");
                ATMInterface.changeTextColor(Color.Green);
                balance = decimal.Parse(ReadLine());
                ATMInterface.changeTextColor(Color.Gray);
                if (balance < 0)
                    throw new Exception("Starting Balance should Not be less than zero");
            }
            catch (Exception ex){
                errorMessage(ex.Message);
                goto inputBalanceAgain;
            }
        inputStatusAgain:
            try {
                Write("Status(Active/Inactive): ");
                ATMInterface.changeTextColor(Color.Green);
                string status = ReadLine();
                ATMInterface.changeTextColor(Color.Gray);
                if (status.ToLower() != "active" && status.ToLower() != "inactive") throw new Exception("Active/Inactive");
                else
                {
                    accountStatus = status.ToLower() switch
                    {
                        "active" => 0,
                        "inactive" => 1
                    };
                }
            }
            catch (Exception ex) {
                errorMessage(ex.Message);
                goto inputStatusAgain;
            }
            AdminBLL bll = new AdminBLL();
            int accountNo = bll.createAccount((new Person(holderName, 0)),(new Credentials(login,pinCode)),(new Account(balance,accountType, accountStatus)));
            WriteLine($"Account created Suuessfully - the account number assigned is: {accountNo}");
            delay();
            return true;   
        }
        public bool deleteAccount()
        {
            Clear();
            AdminBLL bll = new AdminBLL();
            int accountNo;
        inputAccountAgain:
            try
            {
                Write("\nEnter the account number to which you want to delete: ");
                ATMInterface.changeTextColor(Color.Green);
                accountNo = int.Parse(ReadLine());
                ATMInterface.changeTextColor(Color.Gray);
                Person person = (Person)bll.getPerson(accountNo)?[0];
                if (person == null || accountNo == -1) {
                    errorMessage("Account did not found!");
                    delay();
                    return true;
                }
                Write($"\nYou wish to delete the account held by {person.Name}; \nIf " +
                    $"this information is correct please re - enter the account number:");
                ATMInterface.changeTextColor(Color.Green);
                accountNo = int.Parse(ReadLine());
                ATMInterface.changeTextColor(Color.Gray);
                if (accountNo == person.accountNo) {
                    bll.deleteAccount(accountNo, person.id);
                    WriteLine("Account Deleted Successfully");
                    delay();
                }
                else
                {
                    errorMessage("Cannot be deleted");
                    delay();
                }
                    
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                goto inputAccountAgain;
            }
            return true;
        }
        public bool updateAccount()
        {
            Clear();
            AdminBLL bll = new AdminBLL();
            int accountNo;
            Account account;
            Person person;
            Credentials credentials;
        inputAccountAgain:
            try
            {
                Write("\nEnter the Account Number: ");
                ATMInterface.changeTextColor(Color.Green);
                accountNo = int.Parse(ReadLine());
                ATMInterface.changeTextColor(Color.Gray);
                account = (Account)bll.getAccount(accountNo)?[0];
                person = (Person)bll.getPerson(accountNo)?[0];
                credentials = bll.getCredentials(person.id);
                if (account == null || accountNo == -1)
                {
                    errorMessage("Account did not Found");
                    delay();
                    return true; ;
                }
                WriteLine($"\nAccount # {account.ID}\nType: {account.Type}\nHolder {person.Name}\n" +
                    $"Balance: {account.Balance}\nStatus: {account.Status}\n");
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                goto inputAccountAgain;
            }
            WriteLine("Please enter in the fields you wish to update(leave blank otherwise):");
            Write("Login: ");
            ATMInterface.changeTextColor(Color.Green);
            string login = ReadLine();
            ATMInterface.changeTextColor(Color.Gray);
            string pinCode;
        inputPinCode:
            try
            {
                Write("Pin Code: ");
                ATMInterface.changeTextColor(Color.Green);
                pinCode = ReadLine();
                ATMInterface.changeTextColor(Color.Gray);
                if (!(pinCode == String.Empty)  && ( pinCode.Length != 5 || !Regex.Match(pinCode, @"\d\d\d\d\d").Success))
                    throw new Exception("Pin Code is incorrect format");
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                goto inputPinCode;
            }
            Write("Holders Name: ");
            ATMInterface.changeTextColor(Color.Green);
            string holderName = ReadLine();
            ATMInterface.changeTextColor(Color.Gray);
            int accountStatus = 0;
        inputStatusAgain:
            try
            {
                Write("Status(Active/Inactive): ");
                ATMInterface.changeTextColor(Color.Green);
                string status = ReadLine();
                ATMInterface.changeTextColor(Color.Gray);
                if (status.ToLower() != "active" && status.ToLower() != "inactive" && status != String.Empty) 
                    throw new Exception("Active/Inactive");
                else
                {
                    accountStatus = status.ToLower() switch
                    {
                        "active" => 0,
                        "inactive" => 1,
                        "" => -1
                    };
                }
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                goto inputStatusAgain;
            }         
            bll.deleteAccount(account.ID, person.id);
            if (login != String.Empty)
                credentials.Login = login;
            if (pinCode != String.Empty)
                credentials.Password = pinCode;
            if (holderName != String.Empty)
                person.Name = holderName;
            if (accountStatus != -1)
                account.Status = (Account.AccountStatus)accountStatus;
            bll.createAccount(person,credentials ,account);
            WriteLine("Your account has been successfully been updated.");
            delay();
            return true;
        }
        public bool SearchAccount()
        {
            Clear();


            int accountID = -2, userID = -2, accountType = -2, accountStatus = -2;
            decimal balance = 0;
            WriteLine("\nSEARCH MENU: ");
        inputAccountID:
            try
            {
                Write("Account ID: ");
                ATMInterface.changeTextColor(Color.Green);
                string accountid = ReadLine();
                if (accountid != String.Empty)
                    accountID = int.Parse(accountid);

            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                goto inputAccountID;
            }
            ATMInterface.changeTextColor(Color.Gray);
        inputUserID:
            try
            {
                Write("User ID: ");
                ATMInterface.changeTextColor(Color.Green);
                string userid = ReadLine();
                if (userid != String.Empty)
                    userID = int.Parse(userid);
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                goto inputUserID;
            }
            ATMInterface.changeTextColor(Color.Gray);
            Write("Holder Name: ");
            ATMInterface.changeTextColor(Color.Green);
            String holderName = ReadLine();
            ATMInterface.changeTextColor(Color.Gray);
        inputTypeAgain:
            try
            {
                Write("Type(Savings, Current): ");
                ATMInterface.changeTextColor(Color.Green);
                string type = ReadLine();
                if (type.ToLower() != "savings" && type.ToLower() != "current" && type.ToLower() != String.Empty) throw new Exception("savings/current");
                else
                {
                    accountType = type.ToLower() switch
                    {
                        "savings" => 0,
                        "current" => 1,
                        _ => -2
                    };
                }
            }
            catch (Exception e)
            {
                errorMessage(e.Message);
                goto inputTypeAgain;
            }
            ATMInterface.changeTextColor(Color.Gray);

        inputBalanceAgain:
            try
            {
                Write("Starting Balance: ");
                ATMInterface.changeTextColor(Color.Green);
                string bal = ReadLine();                
                if (bal != String.Empty)
                    balance = decimal.Parse(bal);
                //if (balance < 0)
                //    throw new Exception("Starting Balance should Not be less than zero");
            
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                goto inputBalanceAgain;
            }
            ATMInterface.changeTextColor(Color.Gray);
        inputStatusAgain:
            try
            {
                Write("Status(Active/Inactive): ");
                ATMInterface.changeTextColor(Color.Green);
                string status = ReadLine();
                if (status.ToLower() != "active" && status.ToLower() != "inactive" && status.ToLower() != String.Empty) throw new Exception("Active/Inactive");
                else
                {
                    accountStatus = status.ToLower() switch
                    {
                        "active" => 0,
                        "inactive" => 1,
                        _ => -2
                    };
                }
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                goto inputStatusAgain;
            }
            ATMInterface.changeTextColor(Color.Gray);
            AdminBLL bll = new AdminBLL();
            (ArrayList persons,ArrayList accounts) = bll.searchAccount(accountID, userID,holderName, accountType, accountStatus, balance);
            if(accounts.Count == 0)
            {
                errorMessage("Account did not match");
                return true;
            }
            WriteLine("=================Search Results=================");
            WriteLine($"{"Account ID",-15}{"User ID",-10}{"Holders Name",-20}{"Type",-15}{"Balance",-15}{"Status",-15}");
            for (int i=0; i< accounts.Count ; i++)
            {
                Account account = (Account)accounts[i];
                Person person = (Person)persons[i];
                WriteLine($"{account.ID,-15}{person.id,-10}{person.Name,-20}{account.Type,-15}{account.Balance,-15}{account.Status,-15}");
            }
            delay();
            return true;
        }
        public bool viewReports()
        {
            Clear();
            WriteLine("\n1---Accounts By Amount\n2--- Transaction By Date");
            adminDelegator vr = GetInput('1', '2') switch
            {
                '1' => accountsByAmmount,
                '2' => transactionsByDate
            };
            return vr.Invoke();
        }
        private bool accountsByAmmount()
        {
            decimal minBalance = 0, maxBalance = 0;
            Clear();
        inputMinBalanceAgain:
            try
            {
                Write("\nEnter the minimum amount: ");
                ATMInterface.changeTextColor(Color.Green);
                minBalance = decimal.Parse(ReadLine());

                if (minBalance < 0)
                    throw new Exception("Minimum Balance should Not be less than zero");
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                goto inputMinBalanceAgain;
            }
            ATMInterface.changeTextColor(Color.Gray);
        inputMaxBalanceAgain:
            try
            {
                Write("Enter the maximum amount: ");
                ATMInterface.changeTextColor(Color.Green);
                maxBalance = decimal.Parse(ReadLine());
                if (minBalance < 0)
                    throw new Exception("Maximum Balance should Not be less than zero");
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                goto inputMaxBalanceAgain;
            }
            ATMInterface.changeTextColor(Color.Gray);
            AdminBLL bll = new AdminBLL();
            (ArrayList persons, ArrayList accounts) = bll.accountsByAmount(minBalance,maxBalance);
            if (accounts.Count != 0)
            {
                WriteLine("=================Search Results=================");
                WriteLine($"{"Account ID",-15}{"User ID",-10}{"Holders Name",-20}{"Type",-15}{"Balance",-15}{"Status",-15}");
                for (int i = 0; i < accounts.Count; i++)
                {
                    Account account = (Account)accounts[i];
                    Person person = (Person)persons[i];
                    WriteLine($"{account.ID,-15}{person.id,-10}{person.Name,-20}{account.Type,-15}{account.Balance,-15}{account.Status,-15}");
                }
            }
            else
                errorMessage("Acccount did not match your criteria");
            delay();
            return true;
        }
        private bool transactionsByDate()
        {
            DateTime startDate , endDate;
            Clear();
        inputStartDateAgain:
            try
            {
                Write("\nEnter the Starting Date[DD/MM/YYYY]: ");
                ATMInterface.changeTextColor(Color.Green);
                string date = ReadLine();
                string[] dates = date.Split("/");
                if (dates.Length != 3)
                    throw new Exception("Date is incorrect format [DD/MM/YYYY]");

                int day = int.Parse(dates[0]);
                int month = int.Parse(dates[1]);
                int year = int.Parse(dates[2]);
                startDate = new DateTime(year,month,day);
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                goto inputStartDateAgain;
            }
            ATMInterface.changeTextColor(Color.Gray);
        inputEndDateAgain:
            try
            {
                Write("\nEnter the Ending Date[DD/MM/YYYY]: ");
                ATMInterface.changeTextColor(Color.Green);
                string date = ReadLine();
                string[] dates = date.Split("/");
                if (dates.Length != 3)
                    throw new Exception("Date is incorrect format [DD/MM/YYYY]");

                int day = int.Parse(dates[0]);
                int month = int.Parse(dates[1]);
                int year = int.Parse(dates[2]);
                endDate = new DateTime(year, month, day);
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                goto inputEndDateAgain;
            }
            ATMInterface.changeTextColor(Color.Gray);
            AdminBLL bll = new AdminBLL();
            (ArrayList persons, ArrayList transactions) = bll.transactionByDates(startDate, endDate);
            if (transactions.Count != 0)
            {
                WriteLine("=================Search Results=================");
                WriteLine($"{"Transactions Type",-25}{"User ID",-15}{"Holders Name",-20}{"Amount",-15}{"Date",-20}");
                for (int i = 0; i < transactions.Count; i++)
                {
                    Transaction transaction = (Transaction)transactions[i];
                    Person person = (Person)persons[i];
                    WriteLine($"{transaction.Id,-25}{person.id,-15}{person.Name,-20}{transaction.Amount,-15}{transaction.Date,-20}");
                }
            }
            else
                errorMessage("Transaction did not match your criteria");
            delay();
            return true;
        }
    }
}