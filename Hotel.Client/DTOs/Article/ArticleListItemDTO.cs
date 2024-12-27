using Hotel.Data.Entities;

namespace Hotel.Client.DTOs.Article
{
    public class ArticleListItemDTO
    {
        // Tieu de, mo ta, noi dung, hinh anh
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Summary { get; set; }
        public string Content { get; set; }
        public string Images { get; set; }

        public int IdCategory { get; set; }
        public string CateName { get; set; }
    }
}
