//using Microsoft.AspNetCore.Mvc;
//using URLCompactifier.Models;

//namespace APICompactifier.Controllers
//{
//    public class URLResponse
//    {
//        public string urlLink { get; set; }
//        public string statusLink { get; set; }
//        public string tokenLink { get; set; }
//    }
//    public class HomeController : Controller
//    {
//        // Our index Route
//        [HttpGet, Route("/")]
//        public IActionResult Index()
//        {
//            return View();
//        }

//        // Our URL shorten route
//        [HttpPost, Route("/")]
//        public IActionResult PostURL([FromBody] string url)
//        {
//            // Initiate variables
//            var linkArray = url.Split(new[] { '/' }, 2);

//            // If the url does not contain HTTP prefix it with it
//            if (!url.Contains("http"))
//            {
//                url = "http://" + url;
//            }

//            if (SqlLiteDataAccess.DoesTokenExist(linkArray[1]))
//            {
//                LinkDTO linkDTO = new LinkDTO();
//                linkDTO.SecondaryLink = SqlLiteDataAccess.GetSecondaryLink(linkArray[1]);
//                linkDTO.PrimaryLink = SqlLiteDataAccess.GetPrimaryLink(linkDTO.SecondaryLink.PrimaryLink_ID);

//                FindRedirect(url);
//            }
//        }

//        private string FindRedirect(string url)
//        {
//            string result = string.Empty;
//            using (var client = new HttpClient())
//            {
//                var response = client.GetAsync(url).Result;
//                if (response.IsSuccessStatusCode)
//                {
//                    result = response.Headers.Location.ToString();
//                }
//            }
//            return result;
//        }
//    }
//}
