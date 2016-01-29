using System.Collections.Generic;
using System.Linq;

namespace ScrumPokerTable.UI.Hubs
{
    public class Desk
    {
        public string Name { get;  set; }
        public List<int> Cards { get; set; } 
        public List<DeskUser> Users { get; set; }

        public DeskUser GetOrCreateUser(string name)
        {
            lock (Sync)
            {
                var desk = Users.SingleOrDefault(x => x.Name == name);
                if (desk != null) return desk;
                desk = new DeskUser
                {
                    Name = name
                };
                Users.Add(desk);
                return desk;
            }
        }

        private static readonly object Sync = new object();
    }
}