using AutoMapper;
using Hotel.Client.Areas.Admin.DTOs.Hotel;
using Hotel.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hotel.Client.Areas.Admin.Controllers
{
	public class AdminHotelController : AdminControllerBase
	{
		public AdminHotelController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
		{
		}

		public IActionResult Index()
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			var hotelDTO = new HotelDTOs();
			hotelDTO.Name = hotel.Name;
			hotelDTO.Email = hotel.Email;
			hotelDTO.Phone = hotel.Phone;
			hotelDTO.Location = hotel.Location;
			hotelDTO.District = hotel.District;
			hotelDTO.City = hotel.City;
			hotelDTO.Avatar = hotel.Avatar;

			return View(hotelDTO);
		}

		[HttpPost]
		public IActionResult Update(HotelDTOs model, [FromServices] IWebHostEnvironment envi)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			if (model.FileAvatar != null)
			{
				hotel.Avatar = UploadFile(model.FileAvatar, envi.WebRootPath);
			}
			hotel.Name = model.Name;
			hotel.Email = model.Email;
			hotel.Phone = model.Phone;
			hotel.Location = model.Location;
			hotel.District = model.District;
			hotel.City = model.City;

			_HotelDbContext.Update(hotel);
			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Cập nhật thành công");
			return RedirectToAction("Index");
		}


		private string UploadFile(IFormFile file, string webRootPath)
		{
			var fName = file.FileName;
			fName = Path.GetFileNameWithoutExtension(fName)
				+ DateTime.Now.Ticks
				+ Path.GetExtension(fName);

			var directoryPath = Path.Combine(webRootPath, "images", "hotel");
			Directory.CreateDirectory(directoryPath); // Đảm bảo thư mục tồn tại

			var filePath = Path.Combine(directoryPath, fName);
			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				file.CopyTo(stream);
			}

			var relativePath = "/images/hotel/" + fName;
			return relativePath;
		}
	}
}
