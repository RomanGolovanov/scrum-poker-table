using ScrumPokerTable.UI.Model;
using System;

namespace ScrumPokerTable.UI.Providers.Storage.Entities
{
    public class DeskEntity
    {
        public string Name { get; set; }
        public string[] Cards { get; set; }
        public DeskState State { get; set; }
        public DeskUserEntity[] Users { get; set; }

        public DateTime Timestamp
        {
            get
            {
                if (_timestamp.Kind != DateTimeKind.Utc)
                {
                    _timestamp = _timestamp.ToUniversalTime();
                }
                return _timestamp;
            }
            set
            {
                _timestamp = value;
            }
        }

        public DateTime _timestamp;
    }
}