using System.Collections.Generic;
using ViewModels.EmployeeModels;

namespace ViewModels.SharePlus
{
    public class SharePlusViewModel
    {
        public IEnumerable<SharePlusHomePageArticlesModel> MostViewed { get; set; }
        public List<SharePlusHomePageArticlesModel> Pinned { get; set; }
        public List<SharePlusSearchModel> SearchResult { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public string InspirationalQuote { get; set; }
        public bool IsAllowedToEdit { get; set; }
    }
}
