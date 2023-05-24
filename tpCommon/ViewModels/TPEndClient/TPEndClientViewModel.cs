using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ViewModels.TPEndClient
{
    public class TPEndClientViewModel
    {
        public string Name { get; set; }
        public int Id { get; set; }

      
    }
    public class TPEndClientList
    {
        public SelectList EndClientList { get; set; }

        public List<TPEndClientViewModel> EndClientNames { get; set; }

        public int EndClientListID { get; set; }

        public string JavascriptToRun { get; set; }

    }
}
