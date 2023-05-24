using ViewModels.HomePage;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace SmartAdmin.WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnniversariesController : ControllerBase
    {
        private readonly IAnniversariesService service;
        public AnniversariesController(IAnniversariesService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post(LikeModel likeModel)
        {
            int likesCount = 0;
            if (ModelState.IsValid)
            {
                if (likeModel.IsBirthday)
                {
                    var isSuccess = await service.InsertLikeForBirthday(likeModel.entityWishId, (short)likeModel.empId);
                    likesCount = isSuccess == true ? await service.GetAllLikesCountForBirthday(likeModel.entityWishId) : 0;
                }
                else
                {
                    var isSuccess = await service.InsertLikeForWorkAnniversary(likeModel.entityWishId, (short)likeModel.empId);
                    likesCount = isSuccess == true ? await service.GetAllLikesCountForWorkAnniversary(likeModel.entityWishId) : 0;
                }
            }
            return Ok(new { likesCount });
        }

        [HttpPost("InsertComment")]
        public async Task<IActionResult> InsertComment(CommentModel model)
        {
            var comment = new SingleCommentViewModel();
            if (ModelState.IsValid)
            {
                if (model.IsBirthday)
                {
                    comment = await service.InsertBirthdayComment(model.entityWishId, (short)model.empId, model.Content);
                }
                else
                {
                    comment = await service.InsertWorkAnniversaryComment(model.entityWishId, (short)model.empId, model.Content);
                }
            }
            return Ok(comment);
        }
    }

    public class LikeModel
    {
        public int entityWishId { get; set; }
        public int empId { get; set; }
        public bool IsBirthday { get; set; }
    }

    public class CommentModel
    {
        public int entityWishId { get; set; }
        public int empId { get; set; }
        public bool IsBirthday { get; set; }
        public string Content { get; set; }
    }
}
