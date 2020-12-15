using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StajProje.Models.Search
{
    public class SearchViewModel
    {
        public List<Ev> Evler { get; set; }
        public SelectList Tips { get; set; }
        public string type { get; set; }
        public string Keyword { get; set; }
    }
}
