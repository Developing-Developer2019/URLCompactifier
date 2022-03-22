using Microsoft.AspNetCore.Mvc;

namespace APICompactifier.Controllers
{
	public class URLResponse
	{
		public string urlLink { get; set; }
		public string statusLink { get; set; }
		public string tokenLink { get; set; }
	}
	public class HomeController : Controller
	{
		// Our index Route
		[HttpGet, Route("/")]
		public IActionResult Index()
		{
			return View();
		}

		// Our URL shorten route
		[HttpPost, Route("/")]
		public IActionResult PostURL([FromBody] string url)
		{
			throw new NotImplementedException();
		}

		// Our Redirect route
		[HttpGet, Route("/{token}")]
		public IActionResult NixRedirect([FromRoute] string token)
		{
			throw new NotImplementedException();

		}
	}
}
