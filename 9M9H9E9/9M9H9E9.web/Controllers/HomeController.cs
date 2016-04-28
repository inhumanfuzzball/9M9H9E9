using System.Web.Mvc;
using _9M9H9E9.Data;
using System.Linq;
namespace _9M9H9E9.web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            PostRepository postRepo = new PostRepository();
            return View(postRepo.GetAll());
        }

        public ActionResult Recent()
        {
            PostRepository postRepo = new PostRepository();
            return View(from p in postRepo.GetAll() orderby p.Posted descending select p);
        }
    }
}