using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Common;
using Data;
using ViewModels.flowPlusExternal.ReviewPlus;
using Global_Settings;

namespace Services.Interfaces
{
    public interface ITPClientReviewService : IService
    {
        Task<List<ReviewStatusModel>> GetPendingAndInprogressReviewDocuments(Enumerations.ReviewStatus reviewStatus, string extranetUserName, bool loadAssigned,
            int pageNumber = -1, int pageSize = -1, string searchTerm = "", int columnToOrderBy = -1, string orderDirection = "");
        Task<PendingAndInProgressReviewModel> GetPendingAndInprogressReviewJobItems(string extranetUserName);
        Task<List<Contact>> GetAllReviewersForTargetLangForOrg(int OrgId, string TargetIANACode);
        Task<List<Contact>> GetAllReviewersForTargetLangForGroup(int GroupId, string TargetIANACode);
        string GetReviewProgress(int jobItemId);
        Task<List<ReviewEditModel>> GetReviewTranslationSegments(int jobItemId, string extranetUserName,
                                                                 string showTags, string searchAll = "",
                                                                 string searchSource = "", string searchTarget = "",
                                                                 int pageNumber = -1, int pageLength = -1);
        Task<ReviewTranslation> GetAnyReviewTranslationSegment(int jobItemId);
        Task<ReviewTranslation> GetLastModifiedReviewSegment(int jobItemId);
        string UnConvertAmpersandsAndAngledBracketsForXML(string TextToProcess);
        string ConvertAmpersandsAndAngledBracketsForXML(string TextToProcess);
        Task<ReviewTranslation> GetReviewTranslation(int jobItemId, int segment);
        Task<ReviewTranslation> GetReviewTranslation(int RvTranslationId);
        Task<bool> UpdateReviewDocumentTranslationUnit(int JobItemId, string FileName, int Segment, string? TranslationDuringReview,
                                                          string? TranslationDuringReviewCollapsedTags, string SourceText, bool AutoPropagate,
                                                          string UpdatedByUserName, int ReviewDocumentTUid = 0, string Comments = "");
        Task<ReviewTranslationComment> GetReviewTranslationCommentDetails(int jobItemId, int segment);
        Task<int> InsertReviewDocumentTranslationUnitComment(int JobItemID, string FileName, int Segment, string Comments,
                                                                          string extranetUserName);
        Task<ReviewTranslationComment> UpdateReviewDocumentTranslationUnitComment(int JobItemID, int CommentID, string Comments, string extranetUserName);
        Task<bool> ValidateTargetText(bool isCollapsedTag, string textToValidate, int reviewTuId);
        Task<JobItem> UpdateJobItemReviewSegments(int jobitemid);
        int GetTotalNumberOfUpdatedSegments(int jobitemId);
        Task<bool> RevertReviewDocumentTranslationUnit(int JobItemId, string FileName, int Segment, string UpdatedByUserName);
        Task<List<int>> GetAllSegmentsWithSameText(int jobItem, int segmentId, string fileName);

    }
}
