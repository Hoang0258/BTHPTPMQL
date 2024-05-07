using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
namespace MvcHoang.Controllers
{
    public class HelloWorldController : Controller
    { 
        // GET: /HelloWorld/
        public IActionResult Index()
        {
            return View();
        } 
        // GET: /HelloWorld/Welcome/ 

        public string Welcome()
        {
            return "Thieu Huy Hoang";
        }
    }
}
