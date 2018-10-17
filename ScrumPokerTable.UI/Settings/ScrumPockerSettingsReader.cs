using System;

namespace ScrumPokerTable.UI.Settings
{
    public static class ScrumPockerSettingsReader
    {
        public static ScrumPockerSettings ReadSettings()
        {
            return new ScrumPockerSettings
            {
                UsePersistentStorage = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["UsePersistentStorage"]),
                DeskTTL = TimeSpan.Parse(System.Configuration.ConfigurationManager.AppSettings["DeskTTL"])
            };
        }
    }
}