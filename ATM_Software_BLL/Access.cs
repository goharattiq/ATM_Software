using ATM_Software_BO;
using ATM_Software_DAL;

namespace ATM_Software_BLL
{
    public class Access
    {
        public int loginAccess(Credentials loginDetails)
        {
            Authentication auth = new Authentication();
            return auth.LogIn("credentials.csv", loginDetails);
        }

        public void disableAccount(string loginName)
        {
            Authentication auth = new Authentication();
            auth.disableAccount(auth.getPersonID(loginName));
        }
        public bool checkDisable(int userType)
        {
            if (userType <= 1)
                return false;
            Authentication auth = new Authentication();
            return auth.checkDisableAccount(userType); ;
        }
    }
}