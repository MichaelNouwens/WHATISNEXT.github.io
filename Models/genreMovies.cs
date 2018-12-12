using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WHATISNEXT.Models
{
    public class genreMovies
    {
        public class Genre
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class RootObject
        {
            public List<Genre> genres { get; set; }
        }
    }
}