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
        public async Task<ActionResult> Index()
        {


            prefix = "movie/upcoming";
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
         
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
                string uri = prefix + "?api_key=" + apiKey + "&language=en-US";


                using (var response = await httpClient.GetAsync(uri))
                {

                    string responseData = await response.Content.ReadAsStringAsync();
                    var objResponse1 = JsonConvert.DeserializeObject<upcomingMovies.RootObject>(responseData);
                    // var objResponse1 = JsonConvert.DeserializeObject<List<TheTMDB.RootObject>>(responseData);


                    IList<upcomingMovies.Result> lst = objResponse1.results.OfType<upcomingMovies.Result>().ToList();
                    //List<upcomingMovies.Result> lst = objResponse1.results.OfType<upcomingMovies.Result>().ToList();



                    var vm = new MainPageViewModel();
                    vm.UpComingMoviesViewModel = lst;

                    //var me = vm.UpComingMoviesViewModel;
                    //me.Select(x => x.backdrop_path).ToList();


                    return View(vm);


                    //var CountAllTheUpcomignMovies = objResponse1.total_results;
                    //ViewBag.Count = CountAllTheUpcomignMovies;




                }
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
