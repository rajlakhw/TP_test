using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ViewModels.SearchForAnything
{
    public class SearchForAnything
    {
        public string resultType { get; set; }
        public int resultId { get; set; }
        public string resultDescription { get; set; }
  
    }

    public class SearchForAnythingResults
    {
        public List<SearchForAnything> searchResults { get; set; }
        public string searchedFor { get; set; }

        public int resultCount { get; set; }
    }
}
