using System;
using Microsoft.AspNetCore.Mvc;

namespace WebApiA.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class TimeController : Controller
    {
        [HttpGet]
        public string GetNow()
        {
            return DateTime.Now.ToString("hh:mm:ss");
        }
    }
}