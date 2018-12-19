using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WHATISNEXT.Models;

namespace WHATISNEXT.ViewModels
{
    public class DetailsPageViewModel
    {
        public detailMovies.RootObject DetailMoviesViewModel { get; set; }
        public movieReviews.RootObject MovieResultsViewModel { get; set; }
        public similarMovies.RootObject SimilarMoviesViewModel { get; set; }
        public videoMovies.RootObject VideoMoviesViewModel { get; set; }
    }
}