namespace ProjectTravelin.Dtos.BookingDtos
{
    public class GetBookingByIdDto
    {
        public string BookingId { get; set; }

        public string TourId { get; set; }

        public string NameSurname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public int PersonCount { get; set; }

        public DateTime BookingDate { get; set; }

        public string Note { get; set; }

        public string Status { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}