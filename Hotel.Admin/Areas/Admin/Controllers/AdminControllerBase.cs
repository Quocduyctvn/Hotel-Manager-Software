using AutoMapper;
using Hotel.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Admin.Areas.Admin.Controllers
{
	[Authorize]
	public class AdminControllerBase : Controller
	{
		protected readonly ApplicationDbContext _HotelDbContext;  // khai báo protected quy định không cho phép truy cập ngoài lớp - trừ kế thừa 
		protected const int DEFAULT_PAGE_SIZE = 10; // Số phần tử trên 1 trang 
		protected const int DEFAULT_PAGE_NUMBER = 1;
		protected const int DEFAULT_PAGE_NUMBER_QUERY_STRING = 1;

		protected readonly IMapper _mapper;
		public AdminControllerBase(ApplicationDbContext DbContext, IMapper mapper)
		{
			_HotelDbContext = DbContext;
			_mapper = mapper;
		}


		protected void SetErrorMesg(string mesg, bool modelStateIsInvalid = false)
		{
			TempData["Err"] = mesg;
		}
		protected void SetWrnMesg(string mesg, bool modelStateIsInvalid = false)
		{
			TempData["Wrn"] = mesg;
		}

		protected void SetSuccessMesg(string mesg) => TempData["Success"] = mesg;
	}
}

