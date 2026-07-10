namespace ProjectTravelin.Dtos.CommentDtos
{
    public class CreateCommentDto
    {
        public string Headline { get; set; }

        public string CommentDetail { get; set; }

        public int Score { get; set; }

        public string TourId { get; set; }
    }
}