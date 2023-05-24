using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.translateOnline
{
    public class translateOnlineModel
    {

    }

    public class DataTablesTranslateOnline
    {
        public Parameters parameters { get; set; }
        public int pageIndex { get; set; }
        public int dataObjectId { get; set; }
        public int dataTypeId { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public bool? loadAssigned { get; set; }  // for translate online status tables
        public int? jobItemId { get; set; }
        public string showTagVal { get; set; } // tags (collapsed or expanded)
    }

    public class Search
    {
        public string value { get; set; }
        public bool regex { get; set; }
    }

    public class Column
    {
        public string data { get; set; }
        public string name { get; set; }
        public bool searchable { get; set; }
        public bool orderable { get; set; }
        public Search search { get; set; }
    }

    public class Order
    {
        public int column { get; set; }
        public string dir { get; set; }
    }

    public class Parameters
    {
        public int draw { get; set; }
        public List<Column> columns { get; set; }
        public List<Order> order { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public Search search { get; set; }
    }
}
