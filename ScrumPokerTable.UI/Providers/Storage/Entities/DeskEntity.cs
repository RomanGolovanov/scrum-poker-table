using System;
using ScrumPokerTable.UI.Model;

namespace ScrumPokerTable.UI.Providers.Storage.Entities
{
    public class DeskEntity
    {
        public string Name { get; set; }
        public string[] Cards { get; set; }
        public DeskState State { get; set; }
        public DeskUserEntity[] Users { get; set; }
        public DateTime Timestamp { get; set; }
    }
}