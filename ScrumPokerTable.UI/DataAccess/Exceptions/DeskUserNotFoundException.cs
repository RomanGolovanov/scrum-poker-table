using System;

namespace ScrumPokerTable.UI.DataAccess.Exceptions
{
    public class DeskUserNotFoundException : DeskLogicException
    {
        public DeskUserNotFoundException()
        {
        }

        public DeskUserNotFoundException(string message) : base(message)
        {
        }

        public DeskUserNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}