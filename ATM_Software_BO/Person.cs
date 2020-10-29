namespace ATM_Software_BO
{
    public class Person
    {
        public enum Usertype
        {
            Customer,
            Admin
        }
        public int id { get; set; }
        public string Name { get; set; }
        public Usertype Type { get; set; }
        public int accountNo { get; set; }
        public Person()
        {
        }
        public Person(string name, int type)
        {
            Name = name;
            Type = (Usertype)type;
        }
    }
}