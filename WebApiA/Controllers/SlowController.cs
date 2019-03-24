using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApiA.Controllers
{
    public class SlowController : Controller
    {
        [Produces("application/json")]
        [Route("api/[controller]/[action]")]
        public async Task<string> GetName()
        {
            await Task.Delay(6000);
            return "Jonathan";
        }
    }
}