using Microsoft.AspNetCore.Mvc;

namespace Hotel.Client.Views.Shared.Components.Introduce
{
    public class IntroduceViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
