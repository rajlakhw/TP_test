using System;
using System.Collections.Generic;

namespace ViewModels.flowPlusExternal.ReviewPlus
{
    public class ReviewPlusTagModel
    {
        public int Id { get; set; }
        public int JobItemId { get; set; }
        public string FileName { get; set; }
        public int Segment { get; set; }
        public string TagIdentifier { get; set; }
        public string AlternativeTagIdentifier { get; set; }
        public string TagText { get; set; }

    }

    public class ReviewSegmentModel
    {
        public int JobItemId { get; set; }
        public int Segment { get; set; }
        public string TargetText { get; set; }
        public string CommentText { get; set; }
        public bool ShowCollapsedTag { get; set; }
        public bool AutoSave { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public int ReviewTranslationId { get; set; }

        public List<int> SegmentsWithSameSourceText { get; set; }

    }



}
