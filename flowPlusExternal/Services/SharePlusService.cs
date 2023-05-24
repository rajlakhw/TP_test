using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class SharePlusService : ISharePlusService
    {
        private readonly TPCoreProductionContext context;

        public SharePlusService(TPCoreProductionContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<SharePlusArticle>> GetAllArticles<SharePlusArticle>()
        {
            using (context)
            {
                return (IEnumerable<SharePlusArticle>)context.SharePlusArticles.Take(10).ToList();
            }
        }
        public async Task<Data.SharePlusArticle> GetArticleById<SharePlusArticle>(int articleId)
        {
            //var article = SharePlusArticle();
            using (var context = new TPCoreProductionContext())
            {
                return context.SharePlusArticles.Where(o => o.Id == articleId).FirstOrDefault();
            }
            //return article;
        }
    }
}
