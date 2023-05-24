using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.flowPlusExternal.ReviewPlus
{
    public class ReviewEditModel
    {
        public int ReviewTranslationId { get; set; }
        public int JobItemId { get; set; }
        public int Segment { get; set; }
        public string SourceText { get; set; }
        public string Comments { get; set; }
        public string TranslationBeforeReview { get; set; }
        public string TranslationDuringReview { get; set; }
        public string TranslationBeforeReviewCollapsedTags { get; set; }
        public string TranslationDuringReviewCollapsedTags { get; set; }
        public string SourceLang { get; set; }
        public string TargetLang { get; set; }
        public string ContextFieldId { get; set; }
        public bool Locked { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public string ContentType { get; set; }
        public string ContextInfo { get; set; }
        public string KeyContextInfo { get; set; }
        public string MTTranslationReview { get; set; }
        public string MTTranslationReviewCollapsedTags { get; set; }
        public string FileName { get; set; }
        public string OriginalMatchPercentCSSClass { get; set; }


    }

    public class DataTablesReviewPlus
    {
        public Parameters parameters { get; set; }
        public int pageIndex { get; set; }
        public int dataObjectId { get; set; }
        public int dataTypeId { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public bool? loadAssigned { get; set; } // for review plus status tables
        public int? jobItemId { get; set; } // for review plus edit tables
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
