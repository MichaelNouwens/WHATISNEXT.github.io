using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WHATISNEXT.Models;

namespace WHATISNEXT.ViewModels
{
    public class MainPageViewModel
    {
      
            public IList<upcomingMovies.Result> UpComingMoviesViewModel { get; set; }
            public IList<popularMovies.Result> PopularMoviesViewModel { get; set; }
            public int TotalPagesUpcomingmovies { get; set; }
            public int TotalPagesPopularmovies { get; set; }

    }
}