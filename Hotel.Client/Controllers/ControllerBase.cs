using Hotel.Data;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Client.Controllers
{
    public class ControllerBase : Controller
    {
        protected readonly ApplicationDbContext _HotelDbContext;
        public ControllerBase(ApplicationDbContext DbContext)
        {
            _HotelDbContext = DbContext;
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
