using System;
using System.Collections.Generic;
using System.Linq;
//using Newtonsoft.Json.Linq;

namespace ViewModels.Organisation
{

    //    public class DataTableParameters
    //    {
    //        public Dictionary<string, DataTableColumn> Columns;
    //        public int Draw;
    //        public int Length;
    //        public Dictionary<int, DataTableOrder> Order;
    //        public bool SearchRegex;
    //        public string SearchValue;
    //        public int Start;

    //        public DataTableParameters()
    //        {
    //        }

    //        /// <summary>
    //        /// Retrieve DataTable parameters from WebMethod parameter, sanitized against parameter spoofing
    //        /// </summary>
    //        /// <param name="input"></param>
    //        /// <returns></returns>
    //        public static DataTableParameters Get(object input)
    //        {
    //            return Get(JObject.FromObject(input)["parameters"]);
    //        }

    //        /// <summary>
    //        /// Retrieve DataTable parameters from JSON, sanitized against parameter spoofing
    //        /// </summary>
    //        /// <param name="input">JToken object</param>
    //        /// <returns>parameters</returns>
    //        public static DataTableParameters Get(JToken input)
    //        {
    //            return new DataTableParameters
    //            {
    //                Columns = DataTableColumn.Get(input),
    //                Order = DataTableOrder.Get(input),
    //                Draw = (int)input["draw"],
    //                Start = (int)input["start"],
    //                Length = (int)input["length"],
    //                SearchValue =
    //                    new string(
    //                        ((string)input["search"]["value"]).Where(
    //                            c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-').ToArray()),
    //                SearchRegex = (bool)input["search"]["regex"]
    //            };
    //        }
    //    }

    //    public class DataTableColumn
    //    {
    //        public string Data;
    //        public string Name;
    //        public bool Orderable;
    //        public bool Searchable;
    //        public bool SearchRegex;
    //        public string SearchValue;

    //        public DataTableColumn()
    //        {
    //        }

    //        /// <summary>
    //        /// Retrieve the DataTables Columns dictionary from a JSON parameter list
    //        /// </summary>
    //        /// <param name="input">JToken object</param>
    //        /// <returns>Dictionary of Column elements</returns>
    //        public static Dictionary<string, DataTableColumn> Get(JToken input)
    //        {
    //            return (
    //                (JArray)input["columns"])
    //                .Select(col => new DataTableColumn
    //                {
    //                    Data = (string)col["data"],
    //                    Name =
    //                        new string(
    //                            ((string)col["name"]).Where(
    //                                c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-').ToArray()),
    //                    Searchable = (bool)col["searchable"],
    //                    Orderable = (bool)col["orderable"],
    //                    SearchValue =
    //                        new string(
    //                            ((string)col["search"]["value"]).Where(
    //                                c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-').ToArray()),
    //                    SearchRegex = (bool)col["search"]["regex"]
    //                })
    //                .ToDictionary(c => c.Data);
    //        }
    //    }

    //    public class DataTableOrder
    //    {
    //        public int Column;
    //        public string Direction;

    //        public DataTableOrder()
    //        {
    //        }

    //        /// <summary>
    //        /// Retrieve the DataTables order dictionary from a JSON parameter list
    //        /// </summary>
    //        /// <param name="input">JToken object</param>
    //        /// <returns>Dictionary of Order elements</returns>
    //        public static Dictionary<int, DataTableOrder> Get(JToken input)
    //        {
    //            return (
    //                (JArray)input["order"])
    //                .Select(col => new DataTableOrder
    //                {
    //                    Column = (int)col["column"],
    //                    Direction =
    //                        ((string)col["dir"]).StartsWith("desc", StringComparison.OrdinalIgnoreCase) ? "DESC" : "ASC"
    //                })
    //                .ToDictionary(c => c.Column);
    //        }
    //    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
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

    public class DataTables
    {
        public Parameters parameters { get; set; }
        public int dataObjectId { get; set; }
        public int dataTypeId { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }
    public class QuotesTables
    {
        public Parameters parameters { get; set; }
        public int contactId { get; set; }
        public int orgId { get; set; }
        public int orgGroupId { get; set; }
        
    }



}
