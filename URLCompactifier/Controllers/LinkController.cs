﻿using Microsoft.AspNetCore.Mvc;
using URLCompactifier.Models;

namespace URLCompactifier.Controllers
{
    public class LinkController : Controller
    {
        /// <summary>
        /// Link index
        /// </summary>
        /// <param name="link">Link details to pass to the view</param>
        /// <returns></returns>
        public IActionResult Index(LinkBO link)
        {
            // If link is empty, initialise new link
            if (link == null)
            {
                link = new LinkBO();
            }

            return View(link);
        }

        /// <summary>
        /// Input URL from view
        /// </summary>
        /// <param name="urlFullLink"></param>
        /// <returns></returns>
        public IActionResult InputURL(string urlFullLink)
        {
            // If null or empty, return to index
            if (string.IsNullOrEmpty(urlFullLink))
            {
                return RedirectToAction("Index");
            }

            LinkBO link = new LinkBO();

            var linkExists = SqlLiteDataAccess.DoesLinkExist(urlFullLink);

            if (linkExists)
            {
                // Initiate linkBO to check current values
                link = SqlLiteDataAccess.GetLinkDetailsWithTokenOrName(urlFullLink, false);
                link.LinkExists = linkExists;
            }
            // If link isn't in database then set new link properties
            else
            {
                link.Link_Name = urlFullLink;
                link.Link_Token = GenerateToken();
                link.Link_CreatedDateT = DateTime.Now.ToShortDateString();
                link.Link_ExpiryDateT = DateTime.Now.AddDays(7).ToShortDateString();
                link.LinkExists = linkExists;

                // Upload link to DB
                SqlLiteDataAccess.UploadLinks(link);
            }           

            // Return view with populated data
            return RedirectToAction("Index", link);
        }

        /// <summary>
        /// Method to generate a token
        /// </summary>
        /// <returns>Randomised token between 8-10</returns>
        public string GenerateToken()
        {
            string urlsafe = string.Empty;
            Enumerable.Range(48, 75)
              .Where(i => i < 58 || i > 64 && i < 91 || i > 96)
              .OrderBy(o => new Random().Next())
              .ToList()
              .ForEach(i => urlsafe += Convert.ToChar(i)); // Store each char into urlsafe
            var tokenValue = urlsafe.Substring(new Random().Next(0, urlsafe.Length), new Random().Next(8, 15));

            // Check if token exists, if true, run again
            if (SqlLiteDataAccess.DoesTokenExist(tokenValue))
            {
                GenerateToken();
            }

            return tokenValue;
        }
    }
}
