using System.Collections.Generic;
using System.Linq;

namespace ScrumPokerTable.UI.Hubs
{
    public class DeskRepository
    {
        public Desk GetOrCreateDesk(string name)
        {
            lock (Sync)
            {
                var desk = _desks.SingleOrDefault(x => x.Name == name);
                if (desk != null) return desk;
                desk = new Desk
                {
                    Name = name,
                    Cards = new List<int> { 1, 2, 3, 4, 5, 6, 7 },
                    Users = new List<DeskUser>()
                };
                _desks.Add(desk);
                return desk;
            }
        }

        private static readonly object Sync = new object();

        private readonly List<Desk> _desks = new List<Desk>();
    }
}