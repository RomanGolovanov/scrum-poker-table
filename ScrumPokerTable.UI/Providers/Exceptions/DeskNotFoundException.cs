using System;

namespace ScrumPokerTable.UI.Providers.Exceptions
{
    public class DeskNotFoundException : DeskLogicException
    {
        public DeskNotFoundException()
        {
        }

        public DeskNotFoundException(string message) : base(message)
        {
        }

        public DeskNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}