namespace ProjectTravelin.Settings
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string TourCollectionName { get; set; }
        public string CommentCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
        public string TourProgramCollectionName { get; set; }

    }
}
