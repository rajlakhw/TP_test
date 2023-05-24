using System.Collections.Generic;
using System.Threading.Tasks;
using Data;

namespace Services
{
    public interface ISharePlusService
    {
        Task<IEnumerable<SharePlusArticle>> GetAllArticles<SharePlusArticle>();

        Task<Data.SharePlusArticle> GetArticleById<SharePlusArticle>(int articleId);
    }
}
