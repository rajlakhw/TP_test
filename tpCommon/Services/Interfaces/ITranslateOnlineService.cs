using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Common;
using ViewModels.translateOnline;
using Global_Settings;
using ViewModels.flowPlusExternal.ReviewPlus;

namespace Services.Interfaces
{
    public interface ITranslateOnlineService : IService
    {
        Task<List<translateOnlineStatusModel>> GetPendingAndInprogressTranslationDocuments(Enumerations.ReviewStatus reviewStatus, string extranetUserName, bool loadAssigned,
            int pageNumber = -1, int pageSize = -1, string searchTerm = "", int columnToOrderBy = -1, string orderDirection = "");
        Task<PendingAndInProgressTranslationModel> GetPendingAndInprogressTranslationJobItems(string extranetUserName);
        System.Threading.Tasks.Task ApproveTranslation(string extranetUserName, int jobItemId, string comments);
        void RevertAllTranslations(string extranetUserName, int jobItemId, string FileName);

    }
}
