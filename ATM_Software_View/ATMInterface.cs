using ATM_Software_BLL;
using ATM_Software_BO;
using System;
using static System.Console;

namespace ATM_Software_View
{
    public class ATMInterface
    {
        public enum Color
        {
            Gray,
            Green,
            Red
        }
        public static void changeTextColor(Color color)
        {
            ForegroundColor = color switch
            {
                Color.Gray => ConsoleColor.Gray,
                Color.Green => ConsoleColor.Green,
                Color.Red => ConsoleColor.Red,
                _ => ConsoleColor.Gray
            };
        }
        public int getLogInDetails()
        {
            int loginTries = 3;
            int userType = -1;
            Access access = new Access();
            string login = null;
            while (loginTries > 0)
            {
                Write("Enter login: ");
                changeTextColor(Color.Green);
                login = ReadLine();
                changeTextColor(Color.Gray);
                Write("Enter Password: ");
                changeTextColor(Color.Green);
                string password = ReadLine();
                changeTextColor(Color.Gray);
                userType = access.loginAccess(new Credentials(login, password));
                if (userType == -1 && login != "Admin")
                    loginTries--;
                else if (access.checkDisable(userType))
                {
                    errorMessage("You cannot login");
                    userType = -1;
                }else
                    return userType;
            }
            if (loginTries == 0)
                access.disableAccount(login);
            return userType;
        }
        public virtual bool menu(int usertype)
        {
            WriteLine("=============Welcome to ATM===============");
            return true;
        }
        public char GetInput(char from, char to)
        {
            bool control = true;
            
            char menu = '-';
            while (control)
            {
                try {
                    ATMInterface.changeTextColor(Color.Green);
                    ConsoleKeyInfo key = ReadKey();
                    ATMInterface.changeTextColor(Color.Gray);
                    menu = key.KeyChar;
                    if (!validateInput(key.KeyChar, from, to))
                        throw new Exception("\nInput Selection is not in range");
                    control = false;
                }
                catch(Exception ex)
                {
                    errorMessage(ex.Message);
                    control = true;
                    continue;
                }
            }
            ATMInterface.changeTextColor(Color.Gray);
            return menu;
        }
        public static void errorMessage(string Message)
        {
            ATMInterface.changeTextColor(Color.Red);
            WriteLine(Message);
            ATMInterface.changeTextColor(Color.Gray);
        }
        public static void delay()
        {
            Write("\nPress any key to resume.....");
            ReadKey();
        }
        public bool validateInput(char keyPressed,char from,char to)
        {
            return keyPressed >= from && keyPressed <= to;
        }
        public bool Exit()
        {
            changeTextColor(Color.Gray);
            return false;
        }
        public virtual bool selectMenu(char selectedMenu)
        {
            return true;
        }
    }
}