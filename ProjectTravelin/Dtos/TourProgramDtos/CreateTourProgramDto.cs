namespace ProjectTravelin.Dtos.TourProgramDtos
{
    public class CreateTourProgramDto
    {
        public string TourId { get; set; }

        public int DayNumber { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }
    }
}
