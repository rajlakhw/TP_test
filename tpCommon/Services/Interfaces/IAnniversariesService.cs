using System.Threading.Tasks;
using Services.Common;
using ViewModels.HomePage;

namespace Services.Interfaces
{
    public interface IAnniversariesService : IService
    {
        Task<int> GetAllLikesCountForWorkAnniversary(int anniversaryId);
        Task<bool> InsertLikeForWorkAnniversary(int anniversaryId, short empId);
        Task<SingleCommentViewModel> InsertWorkAnniversaryComment(int anniversaryId, short empId, string content);

        Task<int> GetAllLikesCountForBirthday(int birthdayId);
        Task<bool> InsertLikeForBirthday(int birthdayId, short empId);
        Task<SingleCommentViewModel> InsertBirthdayComment(int anniversaryId, short empId, string content);
    }
}
