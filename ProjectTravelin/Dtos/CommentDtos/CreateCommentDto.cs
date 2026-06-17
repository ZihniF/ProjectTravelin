namespace Project3Travelin.Dtos.CommentDtos
{
    public class CreateCommentDto
    {
        public string Headline { get; set; }
        public string CommentDetail { get; set; }
        public int Score { get; set; }
        public DateTime CommentDate { get; set; }
        public bool IsStatus { get; set; }
    }
}
