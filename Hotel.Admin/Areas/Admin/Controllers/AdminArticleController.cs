using AutoMapper;
using Hotel.Admin.Areas.Admin.DTOs.Article;
using Hotel.Data;
using Hotel.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Hotel.Admin.Areas.Admin.Controllers
{
	public class AdminArticleController : AdminControllerBase
	{
		IConfiguration _config;
		string _clientImgFolder;
		public AdminArticleController(ApplicationDbContext DbContext, IMapper mapper, IConfiguration config) : base(DbContext, mapper)
		{
			_config = config;
			_clientImgFolder = _config["ClientImagePath"];
		}

		public IActionResult Index()
		{
			// Lấy dữ liệu từ database
			var articles = _HotelDbContext.AppArticles.Include(x => x.AppArticleCate).ToPagedList();
			return View(articles);
		}

		public IActionResult Create()
		{
			// tạo danh sách danh mục cho dropdownlist
			var categories = _HotelDbContext.AppArticlesCates.ToList();
			ViewBag.Categories = new SelectList(categories, "Id", "Name");
			return View();
		}
		[HttpPost]
		public IActionResult Create(CreateOrUpdateArticleDTO model)
		{
			if (ModelState.IsValid)
			{
				//upload file ảnh
				var adminPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "articles");
				var fileName = "";
				if (model.ImageFile != null)
				{
					IFormFile file = model.ImageFile;
					fileName = Guid.NewGuid() + file.FileName;
					var path = Path.Combine(adminPath, fileName);
					using (var stream = new FileStream(path, FileMode.Create))
					{
						file.CopyTo(stream);
					}
					model.Image = "/images/articles/" + fileName;
				}
				var article = new AppArticle
				{
					Title = model.Title,
					Summary = model.Summary,
					Content = model.Content,
					Images = model.Image,
					IdCategory = model.IdCategory,
					CreatedDate = DateTime.Now,
					UpdatedDate = DateTime.Now,
					CreatedBy = Convert.ToInt32(HttpContext.User.FindFirst("UserId").Value),
				};
				_HotelDbContext.AppArticles.Add(article);
				_HotelDbContext.SaveChanges();

				// chuyển ảnh sang client
				if (!Directory.Exists(_clientImgFolder))
				{
					Directory.CreateDirectory(_clientImgFolder);
				}
				if (model.ImageFile != null)
				{
					System.IO.File.Copy(Path.Combine(adminPath, fileName), Path.Combine(_clientImgFolder, fileName), true);
				}
				return RedirectToAction("Index");
			}
			var categories = _HotelDbContext.AppArticlesCates.ToList();
			ViewBag.Categories = new SelectList(categories, "Id", "Name", model.IdCategory);
			return View(model);
		}
		// edit
		public IActionResult Edit(int id)
		{
			var article = _HotelDbContext.AppArticles.Find(id);
			if (article == null)
			{
				return NotFound();
			}
			var model = new CreateOrUpdateArticleDTO
			{
				Title = article.Title,
				Summary = article.Summary,
				Content = article.Content,
				Image = article.Images,
				IdCategory = article.IdCategory
			};
			var categories = _HotelDbContext.AppArticlesCates.ToList();
			ViewBag.Categories = new SelectList(categories, "Id", "Name", model.IdCategory);
			return View(model);
		}
		[HttpPost]
		public IActionResult Edit(int id, CreateOrUpdateArticleDTO model)
		{
			ModelState.Remove("ImageFile");
			if (ModelState.IsValid)
			{
				var article = _HotelDbContext.AppArticles.Find(id);
				if (article == null)
				{
					SetErrorMesg("Không thể xóa bài viết");
					return NotFound();
				}
				//upload file ảnh
				var adminPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "articles");
				var fileName = "";
				if (model.ImageFile != null)
				{
					IFormFile file = model.ImageFile;
					fileName = Guid.NewGuid() + file.FileName;
					var path = Path.Combine(adminPath, fileName);
					using (var stream = new FileStream(path, FileMode.Create))
					{
						file.CopyTo(stream);
					}
					model.Image = "/images/articles/" + fileName;
					article.Images = model.Image;
				}
				article.Title = model.Title;
				article.Summary = model.Summary;
				article.Content = model.Content;
				article.IdCategory = model.IdCategory;
				article.UpdatedDate = DateTime.Now;
				article.UpdatedBy = Convert.ToInt32(HttpContext.User.FindFirst("UserId").Value);
				_HotelDbContext.SaveChanges();

				// chuyển ảnh sang client
				if (!Directory.Exists(_clientImgFolder))
				{
					Directory.CreateDirectory(_clientImgFolder);
				}
				if (model.ImageFile != null)
				{
					System.IO.File.Copy(Path.Combine(adminPath, fileName), Path.Combine(_clientImgFolder, fileName), true);
				}
				return RedirectToAction("Index");
			}
			var categories = _HotelDbContext.AppArticlesCates.ToList();
			ViewBag.Categories = new SelectList(categories, "Id", "Name", model.IdCategory);
			return View(model);
		}
		// delete
		public IActionResult Delete(int id)
		{
			var article = _HotelDbContext.AppArticles.Find(id);
			if (article == null)
			{
				return NotFound();
			}
			_HotelDbContext.AppArticles.Remove(article);
			_HotelDbContext.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}
