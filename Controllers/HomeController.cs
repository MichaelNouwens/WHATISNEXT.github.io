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
        public async Task<ActionResult> Index(string url, string searchTerm)
        {
            var prefix_upcomingMovies = "movie/upcoming";
            var prefix_popularMovies = "movie/popular";
            var LanguageEnglish = "en-US";
            var page_popularMoviesStart = 1;
            // var page_popularMoviesEnd = 902;
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
                var uri_UpComingMovies = prefix_upcomingMovies + "?api_key=" + apiKey + "&language=" + LanguageEnglish;
                var uri_PopularMovies = prefix_popularMovies + "?api_key=" + apiKey + "&language=" + LanguageEnglish + "&page=" + page_popularMoviesStart.ToString();

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
                //add different pages to lists using for loops ? 
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
            var prefix_reviews_movies = "movie/" + id.ToString() + "/reviews";
            var prefix_similar_movies = "movie/" + id.ToString() + "/similar";
            var prefix_movie_videos = "movie/" + id.ToString() + "/videos";

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                var uri__upcomingMovies_MoreInfo = prefix_upcomingMovies_MoreInfo + "?api_key=" + apiKey + "&language=en-US";
                var uri__reviews_movies = prefix_reviews_movies + "?api_key=" + apiKey + "&language=en-US";
                var uri__similar_movies = prefix_similar_movies + "?api_key=" + apiKey + "&language=en-US";
                var uri__movie_videos = prefix_movie_videos + "?api_key=" + apiKey + "&language=en-US";

                HttpResponseMessage response_MoreInfo = await httpClient.GetAsync(uri__upcomingMovies_MoreInfo);
                HttpResponseMessage response_MovieReviews = await httpClient.GetAsync(uri__reviews_movies);
                HttpResponseMessage response_similarMovies = await httpClient.GetAsync(uri__similar_movies);
                HttpResponseMessage response_movieVideos = await httpClient.GetAsync(uri__movie_videos);

                string responseData_MoreInfo = await response_MoreInfo.Content.ReadAsStringAsync();
                string responseData_MovieReviews = await response_MovieReviews.Content.ReadAsStringAsync();
                string responseData_similarMovies = await response_similarMovies.Content.ReadAsStringAsync();
                string responseData_movieVideos = await response_movieVideos.Content.ReadAsStringAsync();



                //CALL THIS 
                var objResponse_MoreInfo = JsonConvert.DeserializeObject<detailMovies.RootObject>(responseData_MoreInfo);
                var objResponse_MovieReviews = JsonConvert.DeserializeObject<movieReviews.RootObject>(responseData_MovieReviews);
                var objResponse_similarMovies = JsonConvert.DeserializeObject<similarMovies.RootObject>(responseData_similarMovies);
                var objResponse_movieVideos = JsonConvert.DeserializeObject<videoMovies.RootObject>(responseData_movieVideos);


                //create a viewmodel just for "MASTER DETAIL"
                var ms = new DetailsPageViewModel();
                ms.DetailMoviesViewModel = objResponse_MoreInfo;
                ms.MovieResultsViewModel = objResponse_MovieReviews;
                ms.SimilarMoviesViewModel = objResponse_similarMovies;
                ms.VideoMoviesViewModel = objResponse_movieVideos;

                return View(ms);
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

        public async Task<ActionResult> DiscoverMovies(string genre)
        {
            var genres = genre;

            //Many
            var sortBy = "popularity.desc";
            var language = "en-US";
            bool includeAdult = false;
            bool includeVideo = false;

            var DiscoverM_Prefix = "discover/movie";
            var DiscoverM_Suffix = "&language=" + language + "&sort_by=" + sortBy + "&include_adult=" + includeAdult.ToString() + "&include_video=" + includeVideo + "&with_genres=" + genres.ToString();

            //https://api.themoviedb.org/3/discover/movie?api_key=de669cb60954dc927229ebd64ca39d77&language=en-US&sort_by=popularity.desc&include_adult=false&include_video=false&with_genres=28
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                var uri_DiscoverMovies = DiscoverM_Prefix + "?api_key=" + apiKey + DiscoverM_Suffix;
                HttpResponseMessage response_DiscoverMovies = await httpClient.GetAsync(uri_DiscoverMovies);
                string responseData_DiscoverMovies = await response_DiscoverMovies.Content.ReadAsStringAsync();
                var objResponse_DiscoverMovies = JsonConvert.DeserializeObject<DiscoverMovies.RootObject>(responseData_DiscoverMovies);
                return View(objResponse_DiscoverMovies);
            }
        }




        [HttpGet]
        public ActionResult SearchMovies()
        {
            return View(new MainPageViewModel());
        }

        //search/movie
        [HttpPost]
        public async Task<ActionResult> SearchMovies(MainPageViewModel inputModel)
        {
            SearchMapper _SearchMapper = new SearchMapper();
            var model = _SearchMapper.Search(inputModel);

            var prefix_SearchMovies = "search/movie";

            var language = "en-US";
            var page = 1;
            var includeAdult = false;
            var region = "";
            var year = "";
            var primary_release_year = "";

            var Suffix_SearchMovies = "&language=" + language + "&query=" + model.Query + "&page" + page.ToString() + "&include_adult=" + includeAdult.ToString();

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                var uri_SearchMovies = prefix_SearchMovies + "?api_key=" + apiKey + Suffix_SearchMovies;
                HttpResponseMessage response_SearchMovies = await httpClient.GetAsync(uri_SearchMovies);
                string responseData_DiscoverMovies = await response_SearchMovies.Content.ReadAsStringAsync();
                var objResponse_SearchMovies = JsonConvert.DeserializeObject<SearchMovies.RootObject>(responseData_DiscoverMovies);
                // return a View with your Data.


                ViewBag.MoviesQuery = model.Query;

                return View(objResponse_SearchMovies);
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
