using System;
using System.Linq;
using ScrumPokerTable.UI.Providers.Exceptions;
using ScrumPokerTable.UI.Providers.Storage.Entities;

namespace ScrumPokerTable.UI.Providers.Storage
{
    public static class DeskStorageExtensions
    {
        public static DeskUserEntity GetDeskUser(this DeskEntity desk, string userName)
        {
            var user = desk.Users.SingleOrDefault(x => x.Name.Equals(userName, StringComparison.OrdinalIgnoreCase));
            if (user == null)
            {
                throw new DeskUserNotFoundException(string.Format("Desk {0} does not contains user {1}", desk.Name, userName));
            }
            return user;
        }
    }
}