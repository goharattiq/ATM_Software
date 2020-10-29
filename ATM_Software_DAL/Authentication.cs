using ATM_Software_BO;
using System;
using System.Collections;
using System.IO;

namespace ATM_Software_DAL
{
    public class Authentication
    {
        public int LogIn(string fileName,Credentials loginDetails)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory,fileName);
            StreamReader sr = new StreamReader(filePath);
            string line = String.Empty;
            while ((line = sr.ReadLine()) != null)
                if (line.Split(",")[1] == loginDetails.Login && decryption(line.Split(",")[2]) == loginDetails.Password)
                {
                    sr.Close();
                    return Int32.Parse(line.Split(",")[0]);
                }
            sr.Close();
            return -1;
        }
        public static string decryption(string values)
        {
            char[] chars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            string cyper = String.Empty;
            foreach (char value in values)
            {
                cyper += chars[(int)(57 - value)];
            }
            return cyper;
        }
        public static string encryption(string values)
        {
            char[] chars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            string cyper = String.Empty;
            foreach (char value in values)
            {
                cyper += chars[(int)(57 - value)];
            }
            return cyper;
        }
        public int getPersonID(string loginName)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory,"credentials.csv");
            StreamReader sr = new StreamReader(filePath);
            string line = String.Empty;
            while ((line = sr.ReadLine()) != null)
                if (line.Split(",")[1] == loginName)
                {
                    sr.Close();
                    return Int32.Parse(line.Split(",")[0]);
                }
            sr.Close();
            return -1;
        }
        public void disableAccount(int PersonID)
        {
            AdminDAL dal = new AdminDAL();
            ArrayList details = dal.getPersonDetails("users.csv", -2, PersonID);
            if (details != null && details.Count > 0)
            {
                string line = (string)details[0];
                int accountNo = int.Parse(line.Split(",")[3]);
                string detail = (string)dal.getAccountDetails("account.csv",accountNo)[0];
                Account account = new Account(decimal.Parse(detail.Split(",")[1]),
                                int.Parse(detail.Split(",")[3]), int.Parse(detail.Split(",")[2]));
                account.ID = int.Parse(detail.Split(",")[0]);
                dal.deleteAccount(account.ID);
                account.Status = Account.AccountStatus.Inactive;
                dal.saveAccount(account);
            }
        }
        public bool checkDisableAccount(int userType)
        {
            AdminDAL dal = new AdminDAL();
            string detail = (string)dal.getPersonDetails("users.csv", -2, userType)?[0];
            if (detail == null)
                return true;
            string line = (string)dal.getAccountDetails("account.csv",int.Parse(detail.Split(",")[3]))?[0];
            if ((Account.AccountStatus)int.Parse(line.Split(",")[2]) == Account.AccountStatus.Inactive)
                return true;
            return false;
        }
    }
}