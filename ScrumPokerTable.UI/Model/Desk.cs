using System;
using Newtonsoft.Json;

namespace ScrumPokerTable.UI.Model
{
    public class Desk
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cards")]
        public string[] Cards { get; set; }

        [JsonProperty("state")]
        public DeskState State { get; set; }

        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonProperty("users")]
        public DeskUser[] Users { get; set; }
    }
}