namespace ATM_Software_BO
{
    public class Credentials
    {
        private string password;
        public int ID { get; set; }
        public string Login {get;set;}
        public string Password { 
            get {
                return password;
            } set{
                password = value;
            }
        }
        public Credentials(string login , string password)
        {
            this.Login = login;
            this.Password = password;
        }
    }
}