namespace FoosballTracker.Models
{
    public class FoosballTrackerDatabaseSettings : FoosballTrackerDatabaseSetting
    {
        public string GamesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface FoosballTrackerDatabaseSetting
    {
        string GamesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}