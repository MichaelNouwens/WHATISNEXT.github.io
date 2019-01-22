using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WHATISNEXT.ViewModels;

namespace WHATISNEXT.Models
{
    public class SearchMapper
    {
        public MainPageViewModel Search(string Query)
        {
            return new MainPageViewModel
            {
                Query = Query
        };
        }
    public MainPageViewModel Search(MainPageViewModel model)
        {
            model.Query = model.Query;
            return model;
        }
}
}