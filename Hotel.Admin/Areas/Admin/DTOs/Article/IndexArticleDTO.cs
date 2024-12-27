namespace Hotel.Admin.Areas.Admin.DTOs.Article
{
    public class IndexArticleDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string? Image { get; set; }
        public string CategoryName { get; set; }
    }
}
