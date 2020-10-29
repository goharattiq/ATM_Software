using ATM_Software_BLL;
using ATM_Software_BO;
using static System.Console;
using System;

namespace ATM_Software_View
{
    public class CustomerInterface : ATMInterface
    {
        Person customer;
        public delegate bool customerDelegator();
        public override bool menu(int userType)
        {
            CustomerBLL bll = new CustomerBLL();
            this.customer = bll.getPerson(-2,userType);
            ATMInterface.changeTextColor(Color.Gray);
            WriteLine("Welcome in Customer menu");
            WriteLine("1----Withdraw Cash\n2----Cash Transfer\n3----Deposit Cash" +
                "\n4----Display Balance\n5----Exit");
            return selectMenu(GetInput('1', '5'));
        }
        public override bool selectMenu(char selectedMenu)
        {
            customerDelegator d1 = null;
            d1 = selectedMenu switch
            {
                '1' => withDraw,
                '2' => cashTransfer,
                '3' => depositCash,
                '4' => displayBalance,
                '5' => Exit,
                _ => Exit
            };
            return d1.Invoke();
        }
        public bool withDraw()
        {
            customerDelegator wd = null;
            ATMInterface.changeTextColor(Color.Gray);
            WriteLine("\na) Fast Cash\nb) Normal Cash");
            wd = GetInput('a', 'b') switch
            {
                'a' => fastCash,
                'b' => normalCash,
                _   => Exit
            };
            return wd.Invoke();
        }
        public bool fastCash()
        {
            ATMInterface.changeTextColor(Color.Gray);
            WriteLine("\n1----500\n2----1000\n3----2000\n4----5000" +
                "\n5----10000\n6----15000\n7----20000");
            Write("Select one of the denominations of money: ");
            decimal selectedCash = 0;
        inputAmountAgain:
            try
            {
                selectedCash = decimal.Parse("" + GetInput('1', '5'));
            }catch(Exception ex)
            {
                errorMessage(ex.Message);
                goto inputAmountAgain;
            }
            Write($"\nAre you sure you want to withdraw Rs. {selectedCash*500} (Y / N) ?: ");
            ATMInterface.changeTextColor(Color.Green);
            ConsoleKeyInfo key = ReadKey();
            ATMInterface.changeTextColor(Color.Gray);
            if (key.KeyChar.ToString().ToLower() == "y")
            {
                CustomerBLL bll = new CustomerBLL();
                Transaction transaction = bll.withDraw(selectedCash * 500, customer.accountNo);
                if (transaction != null)
                {
                    WriteLine("Cash Successfully Withdrawn!");
                    printRecepit(transaction, customer.accountNo);
                    return true;
                }
            }
            errorMessage("Withdraw Failed");
            return true;
        }
        public bool normalCash()
        {
            Clear();
            decimal requestedAmount = 0;
        inputRequestedAmount:
            try
            {
                Write("\nEnter the withdrawal amount: ");
                ATMInterface.changeTextColor(Color.Green);
                requestedAmount = decimal.Parse(ReadLine());
                ATMInterface.changeTextColor(Color.Gray);
            }
            catch(Exception ex)
            {
                errorMessage(ex.Message);
                goto inputRequestedAmount;
            }
            CustomerBLL bll = new CustomerBLL();
            Transaction transaction = bll.withDraw(requestedAmount, customer.accountNo);
            ATMInterface.changeTextColor(Color.Gray);
            if (transaction != null)
            {
                WriteLine("Cash Successfully Withdrawn!");
                printRecepit(transaction, customer.accountNo);
                return true;
            }
            errorMessage("CashDrawn unsuccesfull!");
            return true;
        }
        public bool cashTransfer()
        {
            CustomerBLL bll = new CustomerBLL();
            int transferTo = -2;
            decimal requestedAmount = 0;
            Transaction transaction = null;
        inputRequestedAmount:
            try
            {
                ATMInterface.changeTextColor(Color.Gray);
                Write("\nEnter amount in multiples of 500: ");
                ATMInterface.changeTextColor(Color.Green);
                requestedAmount = decimal.Parse(ReadLine());
                ATMInterface.changeTextColor(Color.Gray);
                Account account = bll.getAccount(customer.accountNo);
                if (requestedAmount < 0 || requestedAmount % 500 != 0 || requestedAmount > account.Balance)
                    throw new Exception($"Amount should not be neagitive and Sould " +
                        $"be multipiles of 500 and should be less than you current account balance i.e {account.Balance}");
                Write("\nEnter the account number to which you want to transfer: ");
                ATMInterface.changeTextColor(Color.Green);
                transferTo = int.Parse(ReadLine());
                ATMInterface.changeTextColor(Color.Gray);
                Person personTo = bll.getPerson(transferTo);
                Write($"\nYou wish to deposit Rs {requestedAmount} in account held by {personTo.Name};" +
                    $" If this information is correct please re - enter the   account number: ");
                ATMInterface.changeTextColor(Color.Green);
                transferTo = int.Parse(ReadLine());
                ATMInterface.changeTextColor(Color.Gray);
                if (transferTo == personTo.accountNo)
                {
                    transaction = bll.transferAmount(requestedAmount, customer.accountNo, personTo.accountNo);
                    if (transaction != null)
                    {
                        WriteLine("Transaction confirmed.");
                        printRecepit(transaction, account.ID);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                goto inputRequestedAmount;
            }
            errorMessage("Transaction unsuccessfull!");
            return true;
        }
        public bool depositCash()
        {
            Clear();
            decimal requestedAmount=0;
        inputRequestedAmount:
            try
            {
                ATMInterface.changeTextColor(Color.Gray);
                Write("\nEnter the cash amount to Desposited : ");
                ATMInterface.changeTextColor(Color.Green);
                requestedAmount = decimal.Parse(ReadLine());
                ATMInterface.changeTextColor(Color.Gray);
                if (requestedAmount < 0)
                    throw new Exception("Enter Amount should not neagtive");
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                goto inputRequestedAmount;
            }
            CustomerBLL bll = new CustomerBLL();
            Transaction transaction = bll.depositCash(requestedAmount, this.customer.accountNo);
            if (transaction != null)
            {
                WriteLine("Cash Deposited Successfully");
                printRecepit(transaction, customer.accountNo);
                return true;
            }
            errorMessage("Cash Deposited unsuccessful");
            return true;
        }
        private void printRecepit(Transaction transaction, int accountNo)
        {
            CustomerBLL bll = new CustomerBLL();
            Write("Do you wish to print a receipt (Y/N)?");
            ATMInterface.changeTextColor(Color.Green);
            ConsoleKeyInfo key = ReadKey();
            ATMInterface.changeTextColor(Color.Gray);
            if (key.KeyChar.ToString().ToLower() == "y")
                bll.displayReciept(transaction, accountNo);
        }
        public bool displayBalance()
        {
            ATMInterface.changeTextColor(Color.Gray);
            CustomerBLL bll = new CustomerBLL();
            bll.displayReciept(null,customer.accountNo);
            return true;
        }
    }
}