using System;

namespace ScrumPokerTable.UI.DataAccess.Exceptions
{
    public class DeskLogicException : Exception
    {
        public DeskLogicException()
        {
        }

        public DeskLogicException(string message) : base(message)
        {
        }

        public DeskLogicException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}