using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Interfaces;
using Data;
using System.Text.RegularExpressions;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;
using ViewModels.flowPlusExternal.ReviewPlus;

namespace Services
{
    public class TPReviewPlusTagService : ITPReviewPlusTagService
    {

        private readonly IRepository<ReviewPlusTag> reviewPlusTagRepo;
        private readonly IRepository<ReviewPlusTagsTransit> reviewPlusTagTransitRepo;

        public TPReviewPlusTagService(IRepository<ReviewPlusTag> _reviewPlusTagRepo,
                                      IRepository<ReviewPlusTagsTransit> _reviewPlusTagTransitRepo)
        {
            this.reviewPlusTagRepo = _reviewPlusTagRepo;
            this.reviewPlusTagTransitRepo = _reviewPlusTagTransitRepo;
        }
        public bool UseAlternativeTags(List<ReviewTranslation> TranslationUnitsList)
        {
            int NumberInBracketsCount = 0;

            foreach (ReviewTranslation tu in TranslationUnitsList)
            {
                NumberInBracketsCount += GetNumberInBrackesTagsList(tu.TranslationDuringReview).Count;
            }

            return NumberInBracketsCount != 0;
        }

        public List<string> GetNumberInBrackesTagsList(string StringToProtect)
        {
            var NumberBetweenBrackesList = new List<string>();

            string Regex_NumberInBrackets = "¦(\\d+)¦|¦TP(\\d+)TP¦";

            try
            {
                var rxl = new Regex(Regex_NumberInBrackets);
                var TagsCollection = rxl.Matches(StringToProtect);

                foreach (Match match in TagsCollection)
                {
                    NumberBetweenBrackesList.Add(match.Value);
                }
                return NumberBetweenBrackesList;
            }
            catch (Exception ex)
            {
                throw new Exception("GetNumberInBrackesTagsList. An error occurred while parsing the document" + ex.Message);
            }

        }

        public async Task<List<ReviewPlusTagModel>> GetReviewPlusTags(int JobItemId, string FileName, int Segment)
        {
            var result = await reviewPlusTagRepo.All().Where(r => r.JobItemId == JobItemId && r.FileName == FileName && r.Segment == Segment).
                                Select(x => new ReviewPlusTagModel()
                                {
                                    Id = x.Id,
                                    JobItemId = x.JobItemId,
                                    FileName = x.FileName,
                                    Segment = x.Segment,
                                    TagIdentifier = x.TagIdentifier,
                                    AlternativeTagIdentifier = x.AlternativeTagIdentifier,
                                    TagText = x.TagText
                                }).ToListAsync();
            return result;
        }

        public string GetStringWithCollapsedTagsFromDB(string OriginalString, int JobItemId, string FileName, int Segment, bool UseAlternativeTags)
        {
            var StringTagsList = GetReviewPlusTags(JobItemId, FileName, Segment).Result;
            Regex rxl2;

            foreach (ReviewPlusTagModel tag in StringTagsList)
            {
                rxl2 = new Regex(tag.TagText.Replace("[", "\\[").Replace("]", "\\]"));
                MatchCollection TagsCollection = rxl2.Matches(OriginalString);

                if (UseAlternativeTags == true)
                {
                    OriginalString = rxl2.Replace(OriginalString, tag.AlternativeTagIdentifier, 1);
                }
                else
                {
                    OriginalString = rxl2.Replace(OriginalString, tag.TagIdentifier, 1);
                }
            }
            return OriginalString;
        }


        public async Task<ReviewPlusTag> GetReviewPlusTag(int reviewPlusTagId)
        {
            var result = await reviewPlusTagRepo.All().Where(r => r.Id == reviewPlusTagId).FirstOrDefaultAsync();
            return result;
        }

        public async Task<ReviewPlusTagsTransit> GetReviewPlusInTransitTag(int reviewPlusTagId)
        {
            var result = await reviewPlusTagTransitRepo.All().Where(r => r.Id == reviewPlusTagId).FirstOrDefaultAsync();
            return result;
        }

        public string GetStringWithExpandedTags(string OriginalString, int JobItemId, string FileName, int Segment)
        {
            var StringTagsList = GetReviewPlusTags(JobItemId, FileName, Segment).Result;

            if (StringTagsList.Count == 0)
            {
                StringTagsList = GetReviewPlusTagsTransit(JobItemId, FileName, Segment, true).Result;
            }

            foreach (ReviewPlusTagModel tag in StringTagsList)
            {
                OriginalString = OriginalString.Replace(tag.TagIdentifier, tag.TagText).Replace(tag.AlternativeTagIdentifier, tag.TagText);
            }

            return OriginalString;
        }

        public async Task<List<ReviewPlusTagModel>> GetReviewPlusTagsTransit(int JobItemId, string FileName, int Segment, bool GetFromTransit = false)
        {
            var result = await reviewPlusTagTransitRepo.All().Where(r => r.JobItemId == JobItemId && r.FileName == FileName && r.Segment == Segment).Select(x => x.Id).ToListAsync();

            var ReviewPlusTagsList = new List<ReviewPlusTagModel>();

            foreach (int trId in result)
            {
                if (GetFromTransit == true)
                {
                    var reviewPlustag = await GetReviewPlusTag(trId);

                    var reviewPlusTagModel = new ReviewPlusTagModel
                    {
                        Id = reviewPlustag.Id,
                        JobItemId = reviewPlustag.JobItemId,
                        FileName = reviewPlustag.FileName,
                        Segment = reviewPlustag.Segment,
                        TagIdentifier = reviewPlustag.TagIdentifier,
                        AlternativeTagIdentifier = reviewPlustag.AlternativeTagIdentifier,
                        TagText = reviewPlustag.TagText
                    };
                    ReviewPlusTagsList.Add(reviewPlusTagModel);
                }
                else
                {

                    var reviewPlustag = await GetReviewPlusInTransitTag(trId);

                    var reviewPlusTagModel = new ReviewPlusTagModel
                    {
                        Id = reviewPlustag.Id,
                        JobItemId = reviewPlustag.JobItemId,
                        FileName = reviewPlustag.FileName,
                        Segment = reviewPlustag.Segment,
                        TagIdentifier = reviewPlustag.TagIdentifier,
                        AlternativeTagIdentifier = reviewPlustag.AlternativeTagIdentifier,
                        TagText = reviewPlustag.TagText
                    };
                    ReviewPlusTagsList.Add(reviewPlusTagModel);
                }

            }

            return ReviewPlusTagsList;
        }

        public List<TPTagModel> GetTagsListSDLXLIFF(string OriginalString)
        {
            string Regex_Total_Tags = "<([^>]+)>";

            try
            {
                Regex rxl = new Regex(Regex_Total_Tags);
                MatchCollection TagsCollection = rxl.Matches(OriginalString);

                string tagText;
                int tagStart;
                int tagEnd;
                int tagLength;
                List<TPTagModel> TagsList = new List<TPTagModel>();

                foreach (Match tag in TagsCollection)
                {
                    tagText = tag.Value;
                    tagStart = tag.Index;
                    tagEnd = tagStart + tag.Length - 1;
                    tagLength = tag.Length;
                    TPTagModel CompleteTag = new TPTagModel()
                    {
                        Text = tagText,
                        Start = tagStart,
                        End = tagEnd,
                        Length = tagLength
                    };
                    TagsList.Add(CompleteTag);
                }

                return TagsList;

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while parsing the document" + ex.Message);
            }
        }

        /// <summary>
        /// Returns a list with the tags in the specified OriginalString
        /// </summary>
        /// <param name="OriginalString"></param>
        /// <returns></returns>
        public List<TPTagModel> GetTagsList(string OriginalString)
        {
            string Regex_Total_Tags = "<([^>]+)>((.|\n)*?)</([^>]+)>";


            try
            {
                Regex rgx = new Regex(Regex_Total_Tags);
                MatchCollection TagsCollection = rgx.Matches(OriginalString);

                string tagText;
                int tagStart;
                int tagEnd;
                int tagLength;

                List<TPTagModel> TagsList = new List<TPTagModel>();

                foreach (Match tag in TagsCollection)
                {
                    tagText = tag.Value;
                    tagStart = tag.Index;
                    tagEnd = tagStart + tag.Length - 1;
                    tagLength = tag.Length;
                    TPTagModel CompleteTag = new TPTagModel()
                    {
                        Text = tagText,
                        Start = tagStart,
                        End = tagEnd,
                        Length = tagLength
                    };
                    TagsList.Add(CompleteTag);
                }

                return TagsList;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while parsing the document" + ex.Message);
            }
        }

        /// <summary>
        /// Returns a list with the single tags (for example /br, p, etc...) in the specified OriginalString
        /// </summary>
        /// <param name="OriginalString"></param>
        /// <returns></returns>
        public List<TPTagModel> GetSingleTagsList(string OriginalString) {
            string Regex_Total_Tags = "<([^>]+)>";

            try
            {
                Regex rxl = new Regex(Regex_Total_Tags);
                MatchCollection TagsCollection = rxl.Matches(OriginalString);

                string tagText;
                int tagStart;
                int tagEnd;
                int tagLength;

                List<TPTagModel> TagsList = new List<TPTagModel>();

                foreach(Match tag in TagsCollection)
                {
                    tagText = tag.Value;
                    tagStart = tag.Index;
                    tagEnd = tagStart + tag.Length - 1;
                    tagLength = tag.Length;
                    TPTagModel CompleteTag = new TPTagModel()
                    {
                        Text = tagText,
                        Start = tagStart,
                        End = tagEnd,
                        Length = tagLength
                    };
                    TagsList.Add(CompleteTag);
                }
                return TagsList;
            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while parsing the document" + ex.Message);
            }
        }

       

    }
}
