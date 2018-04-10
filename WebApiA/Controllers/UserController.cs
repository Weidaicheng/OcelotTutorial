using Microsoft.AspNetCore.Mvc;

namespace WebApiA.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        [HttpGet]
        public string GetSex(string name)
        {
            if (name == "Jonathan")
                return "Man";
            return null;
        }

        [HttpGet]
        public int? GetAge(string name)
        {
            if (name == "Jonathan")
                return 24;
            return null;
        }
    }
}