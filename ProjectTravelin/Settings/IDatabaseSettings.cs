namespace Project3Travelin.Settings
{
    public interface IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string TourCollectionName { get; set; }
        public string CommentCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
    }
}
