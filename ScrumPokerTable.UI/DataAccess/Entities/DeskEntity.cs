using System;

namespace ScrumPokerTable.UI.DataAccess.Entities
{
    public class DeskEntity
    {
        public string Name { get; set; }
        public string[] Cards { get; set; }
        public DeskState State { get; set; }
        public DateTime Timestamp { get; set; }
    }
}