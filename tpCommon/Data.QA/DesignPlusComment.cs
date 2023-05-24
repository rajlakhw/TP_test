using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class DesignPlusComment
    {
        public int Id { get; set; }
        public int DesignPlusFileId { get; set; }
        public int TextFrameId { get; set; }
        public string TextFrameLabelName { get; set; }
        public short? PreviewVersion { get; set; }
        public byte? Status { get; set; }
        public string CommentString { get; set; }
        public string RvCommentString { get; set; }
        public short CommentIndex { get; set; }
        public DateTime CreatedDate { get; set; }
        public int AuthorExtranetUserId { get; set; }
        public string WorkingCopyNoteLabel { get; set; }
        public string DeliveryCopyNoteLabel { get; set; }
        public string ReviewWorkingCopyNoteLabel { get; set; }
        public string ReviewDeliveryCopyNoteLabel { get; set; }
        public bool? ShowInOriginalCopy { get; set; }
        public bool? ShowInReviewCopy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedByUserId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public int? DeletedByUserId { get; set; }
        public DateTime? RvModifiedDate { get; set; }
        public int? RvModifiedByUserId { get; set; }
        public DateTime? RvDeletedDateTime { get; set; }
        public int? RvDeletedByUserId { get; set; }
    }
}
