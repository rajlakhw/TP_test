using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Repositories;
using Data;
using Microsoft.EntityFrameworkCore;
using ViewModels.SharePlus;
using Services.Interfaces;

namespace Services
{
    public class SharePlusService : ISharePlusService
    {
        private readonly IRepository<SharePlusArticle> repository;
        private readonly IRepository<SharePlusArticleViewLog> SharePlusArticleViewLogrepository;
        private readonly IRepository<SharePlusSearchLog> SharePlusSearchLogrepository;

        public SharePlusService(IRepository<SharePlusArticle> repository,
            IRepository<SharePlusArticleViewLog> SharePlusArticleViewLogrepository,
            IRepository<SharePlusSearchLog> SharePlusSearchLogrepository)
        {
            this.repository = repository;
            this.SharePlusArticleViewLogrepository = SharePlusArticleViewLogrepository;
            this.SharePlusSearchLogrepository = SharePlusSearchLogrepository;
        }

        public async Task<List<SharePlusHomePageArticlesModel>> GetAllPinnedArticles()
        {
            var result = await repository.All().Where(s => s.DeletedDateTime == null && s.IsPinnedArticle == true).Select(a=> new SharePlusHomePageArticlesModel(){ Id=a.Id, Title=a.Title, NumberOfTimesViewed=a.NumberOfTimesViewed }).OrderByDescending(a => a.NumberOfTimesViewed).ToListAsync();
            return result;

            //using (var context = new Data.TPCoreProductionContext())
            //{
            //    return await context.SharePlusArticles.Where(s => s.DeletedDateTime != null).OrderByDescending(a => a.NumberOfTimesViewed).ToListAsync();
            //}
        }

        public async Task<int> Update<SharePlusUpdateModel>(int Id, string title, string htmlBody, string contents, short? lastModifiedByUserId)
        {
            var art = await GetById(Id);

            // maybe add security check later here and autoMapper
            art.Title = title;
            art.Htmlbody = htmlBody;
            art.Contents = contents;
            art.LastModifiedDateTime = GeneralUtils.GetCurrentUKTime();
            art.LastModifiedByEmpId = lastModifiedByUserId;

            repository.Update(art);
            await repository.SaveChangesAsync();

            return Id;
        }

        public async System.Threading.Tasks.Task Delete(int Id, int employeeId)
        {
            var article = await repository.All().Where(a => a.Id == Id).FirstOrDefaultAsync();
            article.DeletedByEmployeeId = (short)employeeId;
            article.DeletedDateTime = GeneralUtils.GetCurrentUKTime();
            repository.Update(article);
            await repository.SaveChangesAsync();
        }

        public async Task<List<SharePlusHomePageArticlesModel>> GetMostViewedArticles()
        {
            var result = await repository.All().Where(s => s.DeletedDateTime == null && s.IsPinnedArticle == false).Select(a => new SharePlusHomePageArticlesModel() { Id = a.Id, Title = a.Title, NumberOfTimesViewed = a.NumberOfTimesViewed }).OrderByDescending(a => a.NumberOfTimesViewed).Take(10).ToListAsync();
            return result;
        }

        public async Task<int> Create<SharePlusCreateModel>(string title, string htmlBody, short createdByEmpId, string contents, bool isPinnedArticle)
        {
            var article = new SharePlusArticle()
            {
                Title = title,
                Htmlbody = htmlBody,
                Contents = contents,
                IsPinnedArticle = isPinnedArticle,
                CreatedDateTime = GeneralUtils.GetCurrentUKTime(),
                CreatedByEmpId = createdByEmpId
            };

            await repository.AddAsync(article);
            await repository.SaveChangesAsync();

            return article.Id;
        }

        public async Task<int> AddArticleViewedLog(int articleId, int employeeId)
        {
            var newDate = GeneralUtils.GetCurrentUKTime();

            var log = new SharePlusArticleViewLog()
            {
                ArticleId = articleId,
                ViewedByEmployeeId = (short)employeeId,
                ViewedDateTime = newDate
            };

            await SharePlusArticleViewLogrepository.AddAsync(log);
            // if the log id is nothing add it to a var here and then return it 
            await SharePlusArticleViewLogrepository.SaveChangesAsync();

            var article = await repository.All().Where(a => a.Id == articleId).FirstOrDefaultAsync();
            article.LastViewedDateTime = newDate;
            article.LastViewedByEmpId = (short)employeeId;
            repository.Update(article);
            await repository.SaveChangesAsync();

            return log.Id;
        }

        public async System.Threading.Tasks.Task UpdateCurrentArticleViewAsHelpful(int viewLogId)
        {
            var logToUpdate = await SharePlusArticleViewLogrepository.All().Where(l => l.Id == viewLogId).FirstOrDefaultAsync();

            logToUpdate.ArtilcleMarkedAsHelpful = true;

            SharePlusArticleViewLogrepository.Update(logToUpdate);
            await SharePlusArticleViewLogrepository.SaveChangesAsync();
        }

        public async Task<SharePlusArticle> GetById(int articleId)
        {
            var result = await repository.All().Where(a => a.Id == articleId && a.DeletedDateTime == null).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<SharePlusSearchModel>> GetSearchResults(string searchTerm)
        {
            var results = await repository.All().Where(a => a.DeletedDateTime == null && (a.Title.Contains(searchTerm) || a.Contents.Contains(searchTerm))).Select(a=>new SharePlusSearchModel() { Id = a.Id, Value = a.Title }).ToListAsync();
            return results;
        }

        public async System.Threading.Tasks.Task AddSearchLog(string searchText, bool isSuccessfulSearch, int employeeId)
        {
            var log = new SharePlusSearchLog()
            {
                IsSuccessfulSearch = isSuccessfulSearch,
                SearchText = searchText,
                SearchedByEmpId = (short)employeeId,
                SearchedDateTime = GeneralUtils.GetCurrentUKTime()
            };

            await SharePlusSearchLogrepository.AddAsync(log);
            await SharePlusSearchLogrepository.SaveChangesAsync();
        }

        public async Task<int> GetNumberOfTimesArticleWasFoundHelpful(int articleId)
        {
            var res = await SharePlusArticleViewLogrepository.All().Where(x => x.ArticleId == articleId && x.ArtilcleMarkedAsHelpful == true).CountAsync();
            return res;
        }

        public async Task<bool> CheckIfEmployeeHasMarkedAnArticleAsHelpful(int articleId, short employeeId)
        {
            var res = await SharePlusArticleViewLogrepository.All().Where(x => x.ArticleId == articleId && x.ArtilcleMarkedAsHelpful == true && x.ViewedByEmployeeId == employeeId).CountAsync();
            return res > 0 ? true : false;
        }
    }
}
