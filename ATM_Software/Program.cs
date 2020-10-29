using ATM_Software_View;

namespace ATM_Software
{
    class Program
    {
        static void Main(string[] args)
        {
            ATMInterface atm = new ATMInterface();
            atm.menu(-1);
            int userType = atm.getLogInDetails();
            atm = userType switch
            {
                0  => new AdminInterface(),
                -1 => null,
                _  => new CustomerInterface()
            };
            while (userType != -1 && atm.menu(userType)) ;
        }
    }
}