using Hotel.Client.DTOs;
using Hotel.Client.DTOs.Article;
using Hotel.Data;
using Hotel.Share.Const;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using X.PagedList;

namespace Hotel.Client.Controllers
{
    public class ArticleController : ControllerBase
    {
        public ArticleController(ApplicationDbContext DbContext) : base(DbContext)
        {
        }

        public IActionResult Index(int page=1)
        {
            var perPage = 50;
            var articles = _HotelDbContext.AppArticles
                .Where(x => x.DeletedDate == null)
                .Select(x => new ArticleListItemDTO
            {
                Id = x.Id,
                Title = x.Title,
                Summary = x.Summary,
                Content = x.Content,
                Images = x.Images,
                IdCategory = x.IdCategory,
                CateName = x.AppArticleCate.Name
            }).ToPagedList(page, perPage);
            return View(articles);
        }

        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var article = _HotelDbContext.AppArticles
                .Include(x => x.AppArticleCate)
                .FirstOrDefault(x => x.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            ViewBag.RelatedArticles = _HotelDbContext.AppArticles
                .Where(x => x.IdCategory == article.IdCategory && x.Id != article.Id)
                .Select(x => new ArticleListItemDTO
                {
                    Id = x.Id,
                    Title = x.Title,
                    Summary = x.Summary,
                    Content = x.Content,
                    Images = x.Images,
                    IdCategory = x.IdCategory,
                    CateName = x.AppArticleCate.Name
                }).ToList();

            return View(article);
        }
    }
}
