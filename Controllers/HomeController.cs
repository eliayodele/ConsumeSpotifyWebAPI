using ConsumeSpotifyWebAPI.Models;
using ConsumeSpotifyWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ConsumeSpotifyWebAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISpotifyAccountService _sportifyAccountService;

        private readonly IConfiguration _configuration;

        private readonly ISpotifyService _spotifyService;
        public HomeController(
            ISpotifyAccountService sportifyAccountService, 
            IConfiguration configuration, 
            ISpotifyService spotifyService)

        {
            _sportifyAccountService = sportifyAccountService;
            _configuration = configuration;
            _spotifyService = spotifyService;

        }

        public async Task <IActionResult> Index()
        {
            var newReleases = await GetReleases();
            return View(newReleases);
        }

        private async Task<IEnumerable<Release>> GetReleases()
        {
            try
            {
                var token = await _sportifyAccountService.GetToken(_configuration["Spotify:ClientId"], _configuration["Spotify:ClientSecret"]);

                var newRelease = await _spotifyService.GetNewReleases("FR", 20, token);
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
           return Enumerable.Empty<Release>();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}