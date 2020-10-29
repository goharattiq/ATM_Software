using System;
using System.Collections;
using System.IO;

namespace ATM_Software_DAL
{
    public class ATMDAL
    {
        public static int getLastNumber(string fileName)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
            StreamReader sr = new StreamReader(filePath);
            string line = String.Empty;
            string last = String.Empty;
            while ((line = sr.ReadLine()) != null && line != String.Empty)
                last = line.Split(",")[0];
            sr.Close();
            if (last == String.Empty)
                last = "-1";
            return int.Parse(last);
        }
        public static void overWriteOnFile(ArrayList lists, string fileName)
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
    }
}