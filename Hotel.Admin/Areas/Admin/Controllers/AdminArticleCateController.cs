using AutoMapper;
using Hotel.Admin.Areas.Admin.DTOs.ArticleCate;
using Hotel.Data;
using Hotel.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Hotel.Admin.Areas.Admin.Controllers
{
	public class AdminArticleCateController : AdminControllerBase
	{
		public AdminArticleCateController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
		{

		}

		public IActionResult Index()
		{
			// Lấy dữ liệu từ database
			var articleCates = _HotelDbContext.AppArticlesCates.ToPagedList();
			return View(articleCates);
		}
		//Create
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Create(CreateOrUpdateContactDTO model)
		{
			if (ModelState.IsValid)
			{
				var articleCate = new AppArticleCate
				{
					Name = model.Name,
					CreatedDate = DateTime.Now
				};
				_HotelDbContext.AppArticlesCates.Add(articleCate);
				_HotelDbContext.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(model);
		}
		// edit 
		public IActionResult Edit(int id)
		{
			var articleCate = _HotelDbContext.AppArticlesCates.Find(id);
			if (articleCate == null)
			{
				return NotFound();
			}
			var model = new CreateOrUpdateContactDTO
			{
				Id = articleCate.Id,
				Name = articleCate.Name
			};
			return Ok(model);
		}
		[HttpPost]
		public IActionResult Edit(int id, CreateOrUpdateContactDTO model)
		{
			if (ModelState.IsValid)
			{
				var articleCate = _HotelDbContext.AppArticlesCates.Find(id);
				if (articleCate == null)
				{
					SetErrorMesg("Không tìm thấy danh mục");
					return RedirectToAction("Index"); ;
				}
				articleCate.Name = model.Name;
				_HotelDbContext.SaveChanges();
				SetSuccessMesg("Cập nhật thành công");
				return RedirectToAction("Index");
			}
			SetErrorMesg("Dữ liệu không hợp lệ");
			return RedirectToAction("Index");
		}
		// delete
		public IActionResult Delete(int id)
		{
			var articleCate = _HotelDbContext.AppArticlesCates
								.Include(c => c.AppArticles)
								.SingleOrDefault(c => c.Id == id);
			if (articleCate == null)
			{
				return NotFound();
			}
			// nếu danh mục có bài viết thì không cho xóa
			if (articleCate.AppArticles.Count > 0)
			{
				SetErrorMesg("Danh mục này đang chứa bài viết, không thể xóa");
				return RedirectToAction("Index");
			}
			_HotelDbContext.AppArticlesCates.Remove(articleCate);
			_HotelDbContext.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}
