using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
namespace MvcHoang.Controllers
{
    public class HelloWorldController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }     
       
    }
}