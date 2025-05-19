using Microsoft.AspNetCore.Mvc;

namespace LocalHistoryWebsite.Data.Repositories
{
    public class HistoryPostRepository : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
