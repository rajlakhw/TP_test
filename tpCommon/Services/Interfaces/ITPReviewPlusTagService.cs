using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Common;
using Data;
using ViewModels.flowPlusExternal.ReviewPlus;

namespace Services.Interfaces
{
    public interface ITPReviewPlusTagService : IService
    {
        List<string> GetNumberInBrackesTagsList(string StringToProtect);
        bool UseAlternativeTags(List<ReviewTranslation> TranslationUnitsList);
        Task<ReviewPlusTag> GetReviewPlusTag(int reviewPlusTagId);
        Task<ReviewPlusTagsTransit> GetReviewPlusInTransitTag(int reviewPlusTagId);
        Task<List<ReviewPlusTagModel>> GetReviewPlusTags(int JobItemId, string FileName, int Segment);
        string GetStringWithCollapsedTagsFromDB(string OriginalString, int JobItemId, string FileName, int Segment, bool UseAlternativeTags);
        string GetStringWithExpandedTags(string OriginalString, int JobItemId, string FileName, int Segment);
        Task<List<ReviewPlusTagModel>> GetReviewPlusTagsTransit(int JobItemId, string FileName, int Segment, bool GetFromTransit = false);
        List<TPTagModel> GetTagsListSDLXLIFF(string OriginalString);
        List<TPTagModel> GetTagsList(string OriginalString);
        List<TPTagModel> GetSingleTagsList(string OriginalString);
    }
}
