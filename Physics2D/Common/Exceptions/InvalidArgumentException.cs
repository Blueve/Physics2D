namespace Physics2D.Common.Exceptions
{
    using System;

    public class InvalidArgumentException : ArgumentException
    {
        public InvalidArgumentException(string message, string paramName)
            : base(message, paramName)
        { }
    }
}
