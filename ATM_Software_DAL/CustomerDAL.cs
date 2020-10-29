using ATM_Software_BO;
using System;
using System.IO;

namespace ATM_Software_DAL
{
    public class CustomerDAL
    {
        public string getPersonDetails(string fileName, int accountNo,int userName = -2)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
            StreamReader sr = new StreamReader(filePath);
            string line = String.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                if (accountNo == -2 && userName == int.Parse(line.Split(",")[0]))
                {
                    sr.Close();
                    return line;
                }
                else if (accountNo == int.Parse(line.Split(",")[3]))
                {
                    sr.Close();
                    return line;
                }
            }
            sr.Close();
            return null;
        }
        public string getAccountDetails(string fileName, int accountNo)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
            StreamReader sr = new StreamReader(filePath);
            string line = String.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                if (accountNo == int.Parse(line.Split(",")[0]))
                {
                    sr.Close();
                    return line;
                }
            }
            sr.Close();
            return null;
        }
        public void saveTransiction(Transaction transaction)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "transactions.csv");
            StreamWriter sw = new StreamWriter(filePath, append: true);
            sw.WriteLine($"{transaction.Id},{transaction.AccountNo},{transaction.Date},{(int)transaction.type}," +
                $"{transaction.Amount}");
            sw.Close();
        }
    }
}