using AutoMapper;
using Hotel.Data;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Hotel.Admin.Areas.Admin.Controllers
{
	public class AdminContactController : AdminControllerBase
	{
		public AdminContactController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
		{
		}

		public IActionResult Index(int page = 1)
		{
			var contacts = _HotelDbContext.AppContacts.ToPagedList(page, DEFAULT_PAGE_SIZE);
			return View(contacts);
		}

		// Delete contact
		public IActionResult Delete(int id)
		{
			var contact = _HotelDbContext.AppContacts.Find(id);
			if (contact != null)
			{
				_HotelDbContext.AppContacts.Remove(contact);
				_HotelDbContext.SaveChanges();
				SetSuccessMesg("Xóa thông tin liên hệ thành công");
			}
			else
			{
				SetErrorMesg("Xóa thông tin liên hệ thất bại");
			}
			return RedirectToAction("Index");
		}
	}
}
