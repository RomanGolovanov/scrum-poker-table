using System;
using System.Linq;

namespace ScrumPokerTable.UI.DataAccess
{
    public class DeskNameProvider : IDeskNameProvider
    {
        public string GetNewDeskName()
        {
            var chars = DeskNameChars.ToCharArray();
            var random = new Random();
            return Enumerable
                .Range(0,8)
                .Select(x => chars[random.Next(0, chars.Length - 1)])
                .Aggregate("", (s, s1) => s + s1);
        }

        private const string DeskNameChars = "0123456789abcdefghijklmnopqrstuvwxyz";

    }
}