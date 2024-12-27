using AutoMapper;
using Hotel.Client.Areas.Admin.DTOs;
using Hotel.Client.Areas.Admin.DTOs.RoomCate;
using Hotel.Data;
using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;
using X.PagedList;

namespace Hotel.Client.Areas.Admin.Controllers
{
	public class AdminRoomCateController : AdminControllerBase
	{
		public AdminRoomCateController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
		{
		}

		public IActionResult Index(string? keyword, int? status)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);


			ViewBag.Status = new List<Status> {
				new Status { Id = 0, Name = "Đang hoạt động" },
				new Status { Id = 1, Name = "Tạm ngưng hoạt động" }
			};
			var roomCate = _HotelDbContext.AppRoomCates.Include(x => x.appRooms).Include(x => x.appRoomsCateAmenity)
								.Where(x => x.IdHotel == hotel.Id && x.Status == RoomCateStatus.ACTIVE).OrderBy(x => x.Position).AsQueryable();

			if (!string.IsNullOrEmpty(keyword))
			{
				var keywordUpper = keyword.ToUpper().Trim();

				roomCate = roomCate.Where(i => i.Name.ToUpper().Contains(keywordUpper));

				TempData["searched"] = "searched";
			}
			if (status != null || status >= 0)
			{
				roomCate = roomCate.Where(i => i.Status == (RoomCateStatus)status);
				TempData["searched"] = "searched";
			}

			if (TempData["amenities"] != null)
			{
				ViewBag.d_none = 1;
			}
			ViewBag.Amenities = _HotelDbContext.AppAmenities.Where(x => x.IdHotel == hotel.Id).ToList();
			return View(roomCate.ToPagedList());
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(RoomCateDTOs model, string submitAction, [FromServices] IWebHostEnvironment envi)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			if (!ModelState.IsValid)
			{
				SetErrorMesg("Vui lòng kiểm tra lại ràng buột dữ liệu");
				return View(model);
			}

			if (model.StanderAdult > model.MaxAdult)
			{
				SetErrorMesg("Số lượng tiêu chuẩn người lớn không vượt quá số lượng tối đa");
				return View(model);
			}
			if (model.StanderChildren > model.MaxChildren)
			{
				SetErrorMesg("Số lượng tiêu chuẩn trẻ em không vượt quá số lượng tối đa");
				return View(model);
			}
			var checkExist = _HotelDbContext.AppRoomCates.Where(x => x.IdHotel == hotel.Id).FirstOrDefault(x => x.Name == model.Name);
			if (checkExist != null)
			{
				SetErrorMesg("Tên hạng phòng đã tồn tại");
				return View(model);
			}

			if (model.FileStrings == null)
			{
				model.FileStrings = new List<string>(); // Initialize if null
			}

			var formFiles = new List<IFormFile>
			{
				model.FormFile1,
				model.FormFile2,
				model.FormFile3,
				model.FormFile4
			};

			// Iterate through each file and upload if it's not null
			foreach (var formFile in formFiles)
			{
				if (formFile != null)
				{
					var file = UploadFile(formFile, envi.WebRootPath);
					model.FileStrings.Add(file);
				}
			}

			if (model.FileStrings.Count < 1)
			{
				SetErrorMesg("Vui lòng cung cấp tối thiểu 1 hình ảnh");
				return View(model);
			}



			var roomCate = new AppRoomCate()
			{
				Name = model.Name,
				EarlyCheckInFee = (double)model.EarlyCheckInFee!,
				LateCheckOutFee = (double)model.LateCheckOutFee!,
				StanderAdult = (int)model.StanderAdult!,
				StanderChildren = (int)model.StanderChildren!,
				MaxAdult = (int)model.MaxAdult!,
				MaxChildren = (int)model.MaxChildren!,
				Desc = model.Desc,
				Status = RoomCateStatus.ACTIVE,
				CreatedDate = DateTime.Now,
				IdHotel = hotel!.Id,
				Position = 10000,
				UpdatedDate = DateTime.Now
			};
			_HotelDbContext.Add(roomCate);
			_HotelDbContext.SaveChanges();


			// lưu hình ảnh
			foreach (var item in model.FileStrings)
			{
				if (item != null)
				{
					var img = new AppImage();
					img.Path = item;
					img.IdRoomCate = roomCate.Id;
					img.CreatedDate = DateTime.Now;
					_HotelDbContext.AppImages.Add(img);
					_HotelDbContext.SaveChanges();
				}
			}

			// thiết lặp giá ban đầu =0 
			var prices = new List<AppRentalPrice>()
			{
				new AppRentalPrice()
				{
					IdRoomCate = roomCate.Id, IdRentalType = 1, IdDayType =1, Price =0
				},
				new AppRentalPrice()
				{   IdRoomCate = roomCate.Id, IdRentalType = 1, IdDayType =2, Price =0
				},
				new AppRentalPrice()
				{
					IdRoomCate = roomCate.Id, IdRentalType = 1, IdDayType =3, Price =0
				},
				new AppRentalPrice()
				{
					IdRoomCate = roomCate.Id,   IdRentalType = 2,   IdDayType =1,   Price =0
				},
				new AppRentalPrice()
				{
					IdRoomCate = roomCate.Id,   IdRentalType = 2,   IdDayType =2,   Price =0
				},
				new AppRentalPrice()
				{
					IdRoomCate = roomCate.Id,   IdRentalType = 2,   IdDayType =3,   Price =0
				},
				new AppRentalPrice()
				{
					IdRoomCate = roomCate.Id,   IdRentalType = 3,   IdDayType =1,   Price =0
				},
				new AppRentalPrice()
				{
					IdRoomCate = roomCate.Id, IdRentalType = 3, IdDayType =2,   Price =0
				},
				new AppRentalPrice()
				{
					IdRoomCate = roomCate.Id, IdRentalType = 3, IdDayType =3,   Price =0
				}
			};
			_HotelDbContext.AddRange(prices);
			_HotelDbContext.SaveChanges();

			var roomCate_list = _HotelDbContext.AppRoomCates.Where(x => x.IdHotel == hotel.Id).ToList();

			if (roomCate_list.Any())
			{
				var order = roomCate_list.OrderBy(x => x.Position).ToList();
				for (int i = 0; i < order.Count; i++)
				{
					order[i].Position = i + 1;
				}
			}

			SetSuccessMesg("Thêm Hạng phòng thành công");
			// chuyển hướng theo yêu cầu 
			if (submitAction == "createAndStay")
			{
				return RedirectToAction("Create");
			}
			return RedirectToAction("Index");
		}


		public IActionResult Update(int id)
		{
			PopulateStatusDropdown();
			var roomCate = _HotelDbContext.AppRoomCates.FirstOrDefault(c => c.Id == id);

			var roomCateDTOs = _mapper.Map<RoomCateDTOs>(roomCate);
			return View(roomCateDTOs);
		}

		[HttpPost]
		public IActionResult Update(RoomCateDTOs model, int id)
		{
			if (!ModelState.IsValid)
			{
				SetErrorMesg("Vui lòng kiểm tra lại ràng buột dữ liệu");
				PopulateStatusDropdown();
				return View(model);
			}

			if (model.StanderAdult > model.MaxAdult)
			{
				PopulateStatusDropdown();
				SetErrorMesg("Số lượng tiêu chuẩn người lớn không vượt quá số lượng tối đa");
				return View(model);
			}
			if (model.StanderChildren > model.MaxChildren)
			{
				PopulateStatusDropdown();
				SetErrorMesg("Số lượng tiêu chuẩn trẻ em không vượt quá số lượng tối đa");
				return View(model);
			}


			var roomCate = _HotelDbContext.AppRoomCates.Include(c => c.appRooms).FirstOrDefault(c => c.Id == id);
			if (roomCate == null)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lý!!");
				RedirectToAction("Index");
			}

			_mapper.Map(model, roomCate);
			_HotelDbContext.Update(roomCate);
			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Cập nhật hạng phòng thành công");
			return RedirectToAction("Index");
		}

		[HttpGet]
		public IActionResult Delete(int id)
		{
			PopulateStatusDropdown();
			var roomCate = _HotelDbContext.AppRoomCates.Include(x => x.appRooms).FirstOrDefault(c => c.Id == id);

			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			TempData["RoomCate"] = _HotelDbContext.AppRoomCates.Where(x => x.IdHotel == hotel.Id && x.Status == RoomCateStatus.ACTIVE).ToList();
			return View(roomCate);
		}

		[HttpGet]
		public IActionResult DeleteRoomCate(int id)
		{
			if (id == 0 || id == null)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lý");
				return RedirectToAction("Index");
			}
			var roomCate = _HotelDbContext.AppRoomCates.FirstOrDefault(c => c.Id == id);
			if (roomCate == null)
			{

				SetErrorMesg("Hạng phòng không tồn tại");
				return RedirectToAction("Index");
			}

			roomCate.Status = RoomCateStatus.INACTIVE;


			_HotelDbContext.Update(roomCate);
			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Xóa hạng phòng thành công");
			return RedirectToAction("Index");
		}

		public IActionResult ChangesRoomCate(int id, int idRoomCateNew, AppRoomCate model)
		{
			if (id == 0 || id == null)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lý");
				return RedirectToAction("Index");
			}
			if (idRoomCateNew == 0 || idRoomCateNew == null)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lý");
				return RedirectToAction("Index");
			}

			var roomCate_old = _HotelDbContext.AppRoomCates.Include(x => x.appRooms).FirstOrDefault(c => c.Id == id);
			if (roomCate_old == null)
			{

				SetErrorMesg("Hạng phòng không tồn tại");
				return RedirectToAction("Index");
			}

			if (roomCate_old.appRooms.Any())
			{
				var rooms = roomCate_old.appRooms;
				var roomCate_new = _HotelDbContext.AppRoomCates.Include(x => x.appRooms).FirstOrDefault(c => c.Id == idRoomCateNew);
				if (roomCate_new == null)
				{
					SetErrorMesg("Hạng phòng mới không tồn tại");
					return RedirectToAction("Index");
				}

				// Chuyển các phòng từ hạng phòng cũ sang hạng phòng mới
				foreach (var room in rooms)
				{
					room.IdRoomCate = roomCate_new.Id; // Cập nhật Id của hạng phòng mới cho từng phòng
				}
				_HotelDbContext.SaveChanges();
			}

			// thay doi trang thai
			roomCate_old.Status = RoomCateStatus.INACTIVE;
			roomCate_old.appRooms.Clear();

			_HotelDbContext.SaveChanges();
			SetSuccessMesg($"Đã xóa hạng phòng và chuyển đổi thành công");
			return RedirectToAction("Index");
		}

		[HttpGet]
		public IActionResult GetAmenities(int id)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			var amenities = _HotelDbContext.AppRoomCateAmenities.Where(x => x.IdRoomCate == id).ToList();
			TempData["amenities"] = string.Join(',', amenities.Select(rp => rp.IdAmenity));
			TempData["IdAmenities"] = id;


			var rentalPrices = _HotelDbContext.AppRentalPrices.Where(x => x.IdRoomCate == id).ToList();
			var price = new RoomCatePriceDTOs();
			price.A1 = rentalPrices.FirstOrDefault(x => x.IdRentalType == 1 && x.IdDayType == 1)?.Price;
			price.B1 = rentalPrices.FirstOrDefault(x => x.IdRentalType == 2 && x.IdDayType == 1)?.Price;
			price.C1 = rentalPrices.FirstOrDefault(x => x.IdRentalType == 3 && x.IdDayType == 1)?.Price;
			price.A2 = rentalPrices.FirstOrDefault(x => x.IdRentalType == 1 && x.IdDayType == 2)?.Price;
			price.B2 = rentalPrices.FirstOrDefault(x => x.IdRentalType == 2 && x.IdDayType == 2)?.Price;
			price.C2 = rentalPrices.FirstOrDefault(x => x.IdRentalType == 3 && x.IdDayType == 2)?.Price;
			price.A3 = rentalPrices.FirstOrDefault(x => x.IdRentalType == 1 && x.IdDayType == 3)?.Price;
			price.B3 = rentalPrices.FirstOrDefault(x => x.IdRentalType == 2 && x.IdDayType == 3)?.Price;
			price.C3 = rentalPrices.FirstOrDefault(x => x.IdRentalType == 3 && x.IdDayType == 3)?.Price;

			TempData["Price"] = JsonConvert.SerializeObject(price);
			return RedirectToAction("Index");
		}

		public IActionResult CloseTable()
		{
			TempData["amenities"] = null;
			ViewBag.d_none = 0;
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult UpdateAmenities(int Id, string IdAmenities, string DeletedIdAmenities, string AddedIdAmenities)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			var amenities = _HotelDbContext.AppRoomCateAmenities.Where(x => x.IdRoomCate == Id).ToList();

			string[] deletedIdAmenities = new string[200];
			if (DeletedIdAmenities != null)
			{
				if (DeletedIdAmenities.Contains(","))  // kiểm tra xem có nhiều hơn 1 hay không
				{
					deletedIdAmenities = DeletedIdAmenities.Split(',');
				}
				else
				{
					deletedIdAmenities[0] = DeletedIdAmenities;
				}
			}

			if (deletedIdAmenities.Length > 0)
			{
				foreach (var item in deletedIdAmenities)
				{
					if (item != null)
					{
						var idAme = Convert.ToInt32(item);
						var roomCateAme = _HotelDbContext.AppRoomCateAmenities.Where(x => x.IdRoomCate == Id && x.IdAmenity == idAme)
										.FirstOrDefault();
						_HotelDbContext.AppRoomCateAmenities.Remove(roomCateAme);
						_HotelDbContext.SaveChanges();
					}
				}
			}

			// kiểm tra có thêm mới hay không 
			string[] addedIdAmenities = new string[200];
			if (AddedIdAmenities != null)
			{
				if (AddedIdAmenities.Contains(",")) // kiểm tra xem có nhiều hơn 1 hay không 
				{
					addedIdAmenities = AddedIdAmenities.Split(',');
				}
				else
				{
					addedIdAmenities[0] = AddedIdAmenities;
				}
			}

			if (addedIdAmenities.Length > 0)
			{
				amenities = new List<AppRoomCateAmenity>();
				foreach (var item in addedIdAmenities)
				{
					if (item != null)
					{
						var idAmenities = Convert.ToInt32(item);
						amenities.Add(new AppRoomCateAmenity
						{
							IdAmenity = idAmenities,
							IdRoomCate = Id
						});
					}
				}
				_HotelDbContext.AppRoomCateAmenities.AddRange(amenities);
				_HotelDbContext.SaveChanges();
				SetSuccessMesg("Cập nhập thành công Tiện nghi hạng phòng");
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult UpdatePrice(int id, RoomCatePriceDTOs model)
		{
			var rentalPrices = _HotelDbContext.AppRentalPrices.Where(x => x.IdRoomCate == id).ToList();

			// Update prices based on the model values.
			var rentalPriceA1 = rentalPrices.FirstOrDefault(x => x.IdRentalType == 1 && x.IdDayType == 1);
			if (rentalPriceA1 != null)
			{
				rentalPriceA1.Price = (double)(model.A1 ?? 0);
				_HotelDbContext.Update(rentalPriceA1);
			}

			var rentalPriceB1 = rentalPrices.FirstOrDefault(x => x.IdRentalType == 2 && x.IdDayType == 1);
			if (rentalPriceB1 != null)
			{
				rentalPriceB1.Price = (double)(model.B1 ?? 0);
				_HotelDbContext.Update(rentalPriceB1);
			}

			var rentalPriceC1 = rentalPrices.FirstOrDefault(x => x.IdRentalType == 3 && x.IdDayType == 1);
			if (rentalPriceC1 != null)
			{
				rentalPriceC1.Price = (double)(model.C1 ?? 0);
				_HotelDbContext.Update(rentalPriceC1);
			}

			// Repeat for other day types...
			var rentalPriceA2 = rentalPrices.FirstOrDefault(x => x.IdRentalType == 1 && x.IdDayType == 2);
			if (rentalPriceA2 != null)
			{
				rentalPriceA2.Price = (double)(model.A2 ?? 0);
				_HotelDbContext.Update(rentalPriceA2);
			}

			var rentalPriceB2 = rentalPrices.FirstOrDefault(x => x.IdRentalType == 2 && x.IdDayType == 2);
			if (rentalPriceB2 != null)
			{
				rentalPriceB2.Price = (double)(model.B2 ?? 0);
				_HotelDbContext.Update(rentalPriceB2);
			}

			var rentalPriceC2 = rentalPrices.FirstOrDefault(x => x.IdRentalType == 3 && x.IdDayType == 2);
			if (rentalPriceC2 != null)
			{
				rentalPriceC2.Price = (double)(model.C2 ?? 0);
				_HotelDbContext.Update(rentalPriceC2);
			}

			var rentalPriceA3 = rentalPrices.FirstOrDefault(x => x.IdRentalType == 1 && x.IdDayType == 3);
			if (rentalPriceA3 != null)
			{
				rentalPriceA3.Price = (double)(model.A3 ?? 0);
				_HotelDbContext.Update(rentalPriceA3);
			}

			var rentalPriceB3 = rentalPrices.FirstOrDefault(x => x.IdRentalType == 2 && x.IdDayType == 3);
			if (rentalPriceB3 != null)
			{
				rentalPriceB3.Price = (double)(model.B3 ?? 0);
				_HotelDbContext.Update(rentalPriceB3);
			}

			var rentalPriceC3 = rentalPrices.FirstOrDefault(x => x.IdRentalType == 3 && x.IdDayType == 3);
			if (rentalPriceC3 != null)
			{
				rentalPriceC3.Price = (double)(model.C3 ?? 0);
				_HotelDbContext.Update(rentalPriceC3);
			}

			// Save changes to the database.
			_HotelDbContext.SaveChanges();


			var tempdata = _HotelDbContext.AppRentalPrices.Where(x => x.IdRoomCate == id).ToList();
			var price = new RoomCatePriceDTOs();
			price.A1 = tempdata.FirstOrDefault(x => x.IdRentalType == 1 && x.IdDayType == 1)?.Price;
			price.B1 = tempdata.FirstOrDefault(x => x.IdRentalType == 2 && x.IdDayType == 1)?.Price;
			price.C1 = tempdata.FirstOrDefault(x => x.IdRentalType == 3 && x.IdDayType == 1)?.Price;
			price.A2 = tempdata.FirstOrDefault(x => x.IdRentalType == 1 && x.IdDayType == 2)?.Price;
			price.B2 = tempdata.FirstOrDefault(x => x.IdRentalType == 2 && x.IdDayType == 2)?.Price;
			price.C2 = tempdata.FirstOrDefault(x => x.IdRentalType == 3 && x.IdDayType == 2)?.Price;
			price.A3 = tempdata.FirstOrDefault(x => x.IdRentalType == 1 && x.IdDayType == 3)?.Price;
			price.B3 = tempdata.FirstOrDefault(x => x.IdRentalType == 2 && x.IdDayType == 3)?.Price;
			price.C3 = tempdata.FirstOrDefault(x => x.IdRentalType == 3 && x.IdDayType == 3)?.Price;

			TempData["Price"] = JsonConvert.SerializeObject(price);
			return RedirectToAction("Index");
		}

		public IActionResult Plus(int id)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);


			// Lấy danh sách các roomCate  đã sắp xếp theo Position
			var roomCate = _HotelDbContext.AppRoomCates.Where(x => x.IdHotel == hotel.Id).OrderBy(x => x.Position).ToList();

			// Tìm roomCate  hiện tại và roomCate  kế tiếp
			var currentItem = roomCate.FirstOrDefault(x => x.Id == id);

			int currentIndex = roomCate.IndexOf(currentItem);
			if (currentIndex == roomCate.Count - 1)
			{
				// Nếu là phần tử cuối cùng, giữ nguyên
				return RedirectToAction("Index");
			}

			var nextItem = roomCate[currentIndex + 1];

			// Hoán đổi Position
			(currentItem.Position, nextItem.Position) = (nextItem.Position, currentItem.Position);

			// Cập nhật và lưu thay đổi
			_HotelDbContext.SaveChanges();

			return RedirectToAction("Index");
		}

		public IActionResult Subtr(int id)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);


			// Lấy danh sách các roomCate  đã sắp xếp theo Position
			var roomCate = _HotelDbContext.AppRoomCates.Where(x => x.IdHotel == hotel.Id).OrderBy(x => x.Position).ToList();

			// Tìm roomCate  hiện tại theo id
			var currentItem = roomCate.FirstOrDefault(x => x.Id == id);

			int currentIndex = roomCate.IndexOf(currentItem);

			// Kiểm tra nếu phần tử là đầu tiên, không giảm Position
			if (currentIndex == 0)
			{
				// Giữ nguyên nếu là phần tử đầu tiên
				return RedirectToAction("Index");
			}

			// Tìm package đứng ngay trước đó
			var previousItem = roomCate[currentIndex - 1];

			// Hoán đổi Position giữa phần tử hiện tại và phần tử trước đó
			(currentItem.Position, previousItem.Position) = (previousItem.Position, currentItem.Position);

			// Lưu thay đổi vào cơ sở dữ liệu
			_HotelDbContext.SaveChanges();

			// Trả về trang danh sách
			return RedirectToAction("Index");
		}

		private void PopulateStatusDropdown()
		{
			ViewBag.Status = new List<Status>
		{
			new Status { Id = 0, Name = "Đang hoạt động" },
			new Status { Id = 1, Name = "Tạm ngưng hoạt động" }
		};
		}

		private string UploadFile(IFormFile file, string webRootPath)
		{
			var fName = file.FileName;
			fName = Path.GetFileNameWithoutExtension(fName)
				+ DateTime.Now.Ticks
				+ Path.GetExtension(fName);

			var directoryPath = Path.Combine(webRootPath, "images", "roomcate");
			Directory.CreateDirectory(directoryPath); // Đảm bảo thư mục tồn tại

			var filePath = Path.Combine(directoryPath, fName);
			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				file.CopyTo(stream);
			}

			var relativePath = "/images/roomcate/" + fName;
			return relativePath;
		}
	}
}
