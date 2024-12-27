namespace Hotel.Admin.Areas.Admin.DTOs.Article
{
    public class CreateOrUpdateArticleDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string? Image { get; set; }
        public IFormFile ImageFile { get; set; }
        public int IdCategory { get; set; }
    }
}
