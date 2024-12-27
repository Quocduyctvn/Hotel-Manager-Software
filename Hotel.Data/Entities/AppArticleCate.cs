using Hotel.Data.Entities.Base;

namespace Hotel.Data.Entities
{
	public class AppArticleCate : AppEntityBase
	{
		public AppArticleCate()
		{
			AppArticles = new HashSet<AppArticle>();
		}
		// Ten danh muc
		public string Name { get; set; }

		public ICollection<AppArticle> AppArticles { get; set; }
	}
}
