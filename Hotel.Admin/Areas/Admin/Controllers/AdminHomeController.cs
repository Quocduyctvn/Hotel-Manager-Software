using AutoMapper;
using Hotel.Data;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Admin.Areas.Admin.Controllers
{
    public class AdminHomeController : AdminControllerBase
    {
        public AdminHomeController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
