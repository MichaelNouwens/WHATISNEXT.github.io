using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WHATISNEXT.Models
{
    public class movieReviews
    {

        public class Result
        {
            public string author { get; set; }
            public string content { get; set; }
            public string id { get; set; }
            public string url { get; set; }
        }

        public class RootObject
        {
            public int id { get; set; }
            public int page { get; set; }
            public List<Result> results { get; set; }
            public int total_pages { get; set; }
            public int total_results { get; set; }
        }
    }
}