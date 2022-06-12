namespace abcclicktrans.Exceptions
{
    public class UserLogedOutException : Exception
    {
        public UserLogedOutException()
        {
        }

        public UserLogedOutException(string message)
            : base(message)
        {
        }

        public UserLogedOutException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
