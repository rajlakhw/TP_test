using System;
using Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.JobItem
{
    public class JobItemCreationViewModel
    {
        public int JobOrderId { get; set; }
        public string JobOrderName { get; set; }
        public int ContactId { get; set; }
        public string ContactName { get; set; }
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public int OrgGroupId { get; set; }
        public string OrgGroupName { get; set; }
        public bool IsVisibleToClient { get; set; }
        [Required]
        public DateTime CompletionDeadline { get; set; }
        
        [Required]
        public List<LanguageService> LanguageServices { get; set; }
        [Required]
        public List<DropdownOptionViewModel> Languages { get; set; }
        public IEnumerable<DropdownOptionViewModel> LanguageServiceCategory { get; set; }


    }
}
