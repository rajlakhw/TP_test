using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SmartAdmin.WebUI.Models
{
    public class TPConfigModel
    {
        public SelectList EndClientList { get; set; }

        public int EndClientID { get; set; }
    }
}
