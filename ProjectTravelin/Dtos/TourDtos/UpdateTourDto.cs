namespace ProjectTravelin.Dtos.TourDtos
{
    public class UpdateTourDto
    {
        public string TourId { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public DateTime TourDate { get; set; }
        public string DayNight { get; set; }
        public string ImageUrl { get; set; }
        public string GeminiImageUrl { get; set; }
        public string YoutubeVideoUrl { get; set; }
        public string CategoryId { get; set; }
    }
}
