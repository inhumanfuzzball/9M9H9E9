using System.Web.Mvc;
using _9M9H9E9.Data;

namespace _9M9H9E9.web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            PostRepository postRepo = new PostRepository();
            return View(postRepo.GetAll());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}