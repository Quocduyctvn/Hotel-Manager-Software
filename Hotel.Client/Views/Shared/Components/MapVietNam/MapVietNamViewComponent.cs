using Microsoft.AspNetCore.Mvc;

namespace Hotel.Client.Views.Shared.Components.MapVietNam
{
    public class MapVietNamViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
