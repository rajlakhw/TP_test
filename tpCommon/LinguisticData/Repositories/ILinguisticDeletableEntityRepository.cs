using System.Linq;
using System.Threading.Tasks;

namespace LinguisticData.Repositories
{
    internal interface ILinguisticDeletableEntityRepository<TEntity> where TEntity : class, ILinguisticDeletableEntity
    {
        IQueryable<TEntity> AllWithDeleted();

        IQueryable<TEntity> AllAsNoTrackingWithDeleted();

        Task<TEntity> GetByIdWithDeletedAsync(params object[] id);

        void HardDelete(TEntity entity);

        void Undelete(TEntity entity);
    }
}
