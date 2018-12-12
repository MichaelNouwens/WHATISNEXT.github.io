﻿using Newtonsoft.Json;
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
        public string prefix = null;

      
        // GET: /Home/
        [HttpGet]
        public async Task<ActionResult> Index(string url)
        {
            var prefix = "movie/upcoming";
            var prefix2 = "movie/popular";

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
                var uri = prefix + "?api_key=" + apiKey + "&language=en-US";
                var uri2 = prefix2 + "?api_key=" + apiKey + "&language=en-US";
                HttpResponseMessage response = await httpClient.GetAsync(uri);
                HttpResponseMessage response2 = await httpClient.GetAsync(uri2);
                string responseData = await response.Content.ReadAsStringAsync();
                string responseData2 = await response2.Content.ReadAsStringAsync();
                var objResponse1 = JsonConvert.DeserializeObject<upcomingMovies.RootObject>(responseData);
                var objResponse2 = JsonConvert.DeserializeObject<popularMovies.RootObject>(responseData2);        
                var getTotalPages_upcomingmovies = objResponse1.total_pages;
                var getTotalPages_popularmovies = objResponse2.total_pages;
                IList<upcomingMovies.Result> lst = objResponse1.results.OfType<upcomingMovies.Result>().ToList();
                IList<popularMovies.Result> lst2 = objResponse2.results.OfType<popularMovies.Result>().ToList();

                var vm = new MainPageViewModel();
                vm.UpComingMoviesViewModel = lst;
                vm.PopularMoviesViewModel = lst2;
                vm.TotalPagesPopularmovies = getTotalPages_popularmovies;
                vm.TotalPagesUpcomingmovies = getTotalPages_upcomingmovies;
                return View(vm);
            }
        }

        //movie/{movie_id}
        public async Task<ActionResult> MoreInfo(int id)
        {         
            var prefix3 = "movie/" + id.ToString();
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                var uri3 = prefix3 + "?api_key=" + apiKey + "&language=en-US";
                HttpResponseMessage response3 = await httpClient.GetAsync(uri3);
                string responseData3 = await response3.Content.ReadAsStringAsync();
                var objResponse3 = JsonConvert.DeserializeObject<detailMovies.RootObject>(responseData3);
                return View(objResponse3);
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
