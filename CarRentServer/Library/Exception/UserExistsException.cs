namespace Library.Exception
{
    public class UserExistsException : System.Exception
    {
        public UserExistsException(string message) : base(message) { }
    }
}
