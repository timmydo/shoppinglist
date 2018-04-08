using backend.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("/api/list")]
    public class ListController : Controller
    {
        public ListController()
        {
        }

        [HttpGet("my")]
        public async Task<ListResponse> GetMyLists()
        {
            await Task.Delay(10);
            return new ListResponse();
        }
    }
}