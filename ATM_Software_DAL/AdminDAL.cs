using ATM_Software_BO;
using System;
using System.Collections;
using System.IO;

namespace ATM_Software_DAL
{
    public class AdminDAL
    {
        public void savePerson(Person person, string filename = "users.csv")
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, filename);
            StreamWriter sw = new StreamWriter(filePath, append: true);
            sw.WriteLine($"{person.id},{person.Name},{(int)person.Type},{person.accountNo}");
            sw.Close();
        }
        public void saveCredentials(Credentials credentials)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "credentials.csv");
            StreamWriter sw = new StreamWriter(filePath, append: true);
            sw.WriteLine($"{credentials.ID},{credentials.Login},{credentials.Password}");
            sw.Close();

        }
        public void saveAccount(Account account)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "account.csv");
            StreamWriter sw = new StreamWriter(filePath, append: true);
            sw.WriteLine($"{account.ID},{account.Balance},{(int)account.Status},{(int)account.Type}");
            sw.Close();
        }
        public void overWriteOnFile(ArrayList lists, string fileName)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
            File.Delete(filePath);
            StreamWriter sw = new StreamWriter(filePath);
            foreach (string line in lists)
            {
                sw.WriteLine(line);
            }
            sw.Close();
        }
        public void deletePerson(int personId)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "users.csv");
            StreamReader sr = new StreamReader(filePath);
            var lines = new ArrayList();
            string line = String.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                if (personId != int.Parse(line.Split(",")[0]))
                    lines.Add(line);
            }
            sr.Close();
            overWriteOnFile(lines, "users.csv");
        }

        public void deleteCredentials(int personId)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "credentials.csv");
            StreamReader sr = new StreamReader(filePath);
            var lines = new ArrayList();
            string line = String.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                if (personId != int.Parse(line.Split(",")[0]))
                    lines.Add(line);
            }
            sr.Close();
            overWriteOnFile(lines, "credentials.csv");
        }

        public void deleteAccount(int accountNo)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "account.csv");
            StreamReader sr = new StreamReader(filePath);
            var lines = new ArrayList();
            string line = String.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                if (accountNo != int.Parse(line.Split(",")[0]))
                    lines.Add(line);
            }
            sr.Close();
            overWriteOnFile(lines, "account.csv");
        }
        public ArrayList getPersonDetails(string fileName, int accountNo, int userID = -2, string userName = "")
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
            StreamReader sr = new StreamReader(filePath);
            ArrayList lines = new ArrayList();
            string line = String.Empty;
            while ((line = sr.ReadLine()) != null)
            {

                if (accountNo == -2 && userID == int.Parse(line.Split(",")[0]))
                    lines.Add(line);
                if (accountNo == -2 && userID == -2 && userName == line.Split(",")[1])
                    lines.Add(line);
                else
                {
                    if (accountNo == int.Parse(line.Split(",")[3]))
                    {
                        sr.Close();
                        ArrayList singleAccount = new ArrayList();
                        singleAccount.Add(line);
                        return singleAccount;
                    }
                }
            }
            sr.Close();
            return lines;
        }
        public ArrayList getCredentialDetails(string fileName, int personId)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
            StreamReader sr = new StreamReader(filePath);
            var lines = new ArrayList();
            string line = String.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                if (personId == -1)
                    lines.Add(line);
                else
                {
                    if (personId == int.Parse(line.Split(",")[0]))
                    {
                        sr.Close();
                        ArrayList singleAccount = new ArrayList();
                        singleAccount.Add(line);
                        return singleAccount;
                    }
                }
            }
            sr.Close();
            return lines;
        }
        public ArrayList getAccountDetails(string fileName, int accountNo)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
            StreamReader sr = new StreamReader(filePath);
            var lines = new ArrayList();
            string line = String.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                if (accountNo == -2)
                    lines.Add(line);
                else if (accountNo == int.Parse(line.Split(",")[0]))
                {
                    sr.Close();
                    ArrayList singleAccount = new ArrayList();
                    singleAccount.Add(line);
                    return singleAccount;
                }
            }
            sr.Close();
            return lines;
        }
        public ArrayList accountsByDate(string fileName, decimal minBalance, decimal maxBalance)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
            StreamReader sr = new StreamReader(filePath);
            ArrayList lines = new ArrayList();
            string line = String.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                decimal balance = decimal.Parse(line.Split(",")[1]);
                if (minBalance <= balance && maxBalance >= balance)
                    lines.Add(line);
            }
            sr.Close();
            return lines;
        }
        public ArrayList transactionByDates(string fileName, DateTime startDate, DateTime endDate)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
            StreamReader sr = new StreamReader(filePath);
            ArrayList lines = new ArrayList();
            string line = String.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                DateTime date = DateTime.Parse(line.Split(",")[2]);
                if (DateTime.Compare(date, startDate) >= 0 && DateTime.Compare(date, endDate) <=0)
                    lines.Add(line);
            }
            sr.Close();
            return lines;
        }
    }
}