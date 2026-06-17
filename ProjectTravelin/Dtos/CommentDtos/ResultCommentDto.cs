namespace ProjectTravelin.Dtos.CommentDtos
{
    public class ResultCommentDto
    {
        public string CommentId { get; set; }
        public string Headline { get; set; }
        public string CommentDetail { get; set; }
        public int Score { get; set; }
        public DateTime CommentDate { get; set; }
        public bool IsStatus { get; set; }
    }
}
