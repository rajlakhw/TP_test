using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Services.Common;
using ViewModels.SharePlus;

namespace Services.Interfaces
{
    public interface ISharePlusService : IService
    {
        Task<List<SharePlusHomePageArticlesModel>> GetAllPinnedArticles();

        Task<List<SharePlusHomePageArticlesModel>> GetMostViewedArticles();

        Task<SharePlusArticle> GetById(int articleId);

        Task<List<SharePlusSearchModel>> GetSearchResults(string searchTerm);

        Task<int> Update<SharePlusUpdateModel>(int Id, string title, string htmlBody, string contents, short? lastModifiedByUserId);

        Task<int> Create<SharePlusCreateModel>(string title, string htmlBody, short createdByEmpId, string contents, bool isPinnedArticle);

        System.Threading.Tasks.Task Delete(int Id, int employeeId);

        Task<int> AddArticleViewedLog(int articleId, int employeeId);

        Task<int> GetNumberOfTimesArticleWasFoundHelpful(int articleId);

        Task<bool> CheckIfEmployeeHasMarkedAnArticleAsHelpful(int articleId, short employeeId);

        System.Threading.Tasks.Task UpdateCurrentArticleViewAsHelpful(int viewLogId);

        System.Threading.Tasks.Task AddSearchLog(string searchText, bool isSuccessfulSearch, int employeeId);
    }
}
