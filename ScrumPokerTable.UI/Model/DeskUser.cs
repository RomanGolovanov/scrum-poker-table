using Newtonsoft.Json;

namespace ScrumPokerTable.UI.Model
{
    public class DeskUser
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("card")]
        public string Card { get; set; }
    }
}