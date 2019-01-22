using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WHATISNEXT.ViewModels
{
    public class SearchViewModel
    {
        [Required]
        public string Query { get; set; }
    }
}