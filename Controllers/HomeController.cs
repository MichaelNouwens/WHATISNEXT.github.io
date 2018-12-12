using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using WHATISNEXT.Models;
using WHATISNEXT.ViewModels;
using System.Net;
using System.IO;

namespace WHATISNEXT.Controllers
{

    public class HomeController : Controller
    {

        //Setting the base 
        public Uri baseAddress = new Uri("http://api.themoviedb.org/3/");
        //The api key
        private string apiKey = "de669cb60954dc927229ebd64ca39d77";
        public string responseData = null;
     

      
        // GET: /Home/
        [HttpGet]
        public async Task<ActionResult> Index(string url)
        {
            var prefix_upcomingMovies = "movie/upcoming";
            var prefix_popularMovies = "movie/popular";

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
                var uri_UpComingMovies = prefix_upcomingMovies + "?api_key=" + apiKey + "&language=en-US";
                var uri_PopularMovies = prefix_popularMovies + "?api_key=" + apiKey + "&language=en-US";

                HttpResponseMessage response = await httpClient.GetAsync(uri_UpComingMovies);
                HttpResponseMessage response_popularMovies = await httpClient.GetAsync(uri_PopularMovies);

                string responseData_UpComingMovies = await response.Content.ReadAsStringAsync();
                string responseData_PopularMovies = await response_popularMovies.Content.ReadAsStringAsync();

                var objResponse_UpComingMovies = JsonConvert.DeserializeObject<upcomingMovies.RootObject>(responseData_UpComingMovies);
                var objResponse_popularMovies = JsonConvert.DeserializeObject<popularMovies.RootObject>(responseData_PopularMovies); 
                
                var getTotalPages_upcomingmovies = objResponse_UpComingMovies.total_pages;
                var getTotalPages_popularmovies = objResponse_popularMovies.total_pages;
                IList<upcomingMovies.Result> List_UpComingMovies = objResponse_UpComingMovies.results.OfType<upcomingMovies.Result>().ToList();
                IList<popularMovies.Result> List_PopularMovies = objResponse_popularMovies.results.OfType<popularMovies.Result>().ToList();

                var vm = new MainPageViewModel();
                vm.UpComingMoviesViewModel = List_UpComingMovies;
                vm.PopularMoviesViewModel = List_PopularMovies;
                vm.TotalPagesPopularmovies = getTotalPages_popularmovies;
                vm.TotalPagesUpcomingmovies = getTotalPages_upcomingmovies;
                return View(vm);
            }
        }

        //movie/{movie_id}
        [HttpGet]
        public async Task<ActionResult> MoreInfo(int id)
        {         
            var prefix_upcomingMovies_MoreInfo = "movie/" + id.ToString();
       

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                var uri3 = prefix_upcomingMovies_MoreInfo + "?api_key=" + apiKey + "&language=en-US";
                HttpResponseMessage response_MoreInfo = await httpClient.GetAsync(uri3);
                string responseData_MoreInfo = await response_MoreInfo.Content.ReadAsStringAsync();
                var objResponse_MoreInfo = JsonConvert.DeserializeObject<detailMovies.RootObject>(responseData_MoreInfo);
                return View(objResponse_MoreInfo);
            }     
        }

        //genre/movie/list
        [HttpGet]
        public async Task<ActionResult> MovieGenre()
        {
            var prefix_upcomingMovies_MovieGenres = "genre/movie/list";
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                var uriMovieGenres = prefix_upcomingMovies_MovieGenres + "?api_key=" + apiKey + "&language=en-US";
                HttpResponseMessage response_MovieGenres = await httpClient.GetAsync(uriMovieGenres);
                string responseData_MovieGenres = await response_MovieGenres.Content.ReadAsStringAsync();
                var objResponse_MovieGenres = JsonConvert.DeserializeObject<genreMovies.RootObject>(responseData_MovieGenres);
                return View(objResponse_MovieGenres);
            }
        
        }

        //genre/tv/list
        [HttpGet]
        public async Task<ActionResult> TVGenre()
        {
            var prefix_upcomingMovies_TVGenres = "genre/tv/list";
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                var uriMovieGenres = prefix_upcomingMovies_TVGenres + "?api_key=" + apiKey + "&language=en-US";
                HttpResponseMessage response_TVGenres = await httpClient.GetAsync(uriMovieGenres);
                string responseData_TVGenres = await response_TVGenres.Content.ReadAsStringAsync();
                var objResponse_TVGenres = JsonConvert.DeserializeObject<genreTV.RootObject>(responseData_TVGenres);
                return View(objResponse_TVGenres);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
