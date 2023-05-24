using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using ViewModels.HomePage;

namespace Services
{
    public class AnniversariesService : IAnniversariesService
    {
        private readonly IRepository<BirthdayWish> birthdayRepository;
        private readonly IRepository<BirthdayComment> birthdayCommentRepository;
        private readonly IRepository<BirthdayLike> birthdayLikeRepository;
        private readonly IRepository<WorkAnniversariesWish> workRepository;
        private readonly IRepository<WorkAnnivarsariesComment> workCommentRepository;
        private readonly IRepository<WorkAnniversariesLike> workAnnivarsaryLikeRepository;
        private readonly IRepository<Employee> employeeRepository;
        private readonly IEmailUtilsService emailService;
        public AnniversariesService(IRepository<BirthdayWish> birthdayRepository,
            IRepository<WorkAnniversariesWish> workRepository,
            IRepository<BirthdayLike> birthdayLikeRepository,
            IRepository<WorkAnniversariesLike> workAnnivarsaryLikeRepository,
            IRepository<BirthdayComment> birthdayCommentRepository,
            IRepository<WorkAnnivarsariesComment> workCommentRepository,
            IRepository<Employee> employeeRepository,
            IEmailUtilsService emailService)
        {
            this.birthdayRepository = birthdayRepository;
            this.workRepository = workRepository;
            this.birthdayLikeRepository = birthdayLikeRepository;
            this.workAnnivarsaryLikeRepository = workAnnivarsaryLikeRepository;
            this.birthdayCommentRepository = birthdayCommentRepository;
            this.workCommentRepository = workCommentRepository;
            this.employeeRepository = employeeRepository;
            this.emailService = emailService;
        }

        public async Task<int> GetAllLikesCountForBirthday(int birthdayId)
        {
            var res = await birthdayRepository.All().Where(x => x.Id == birthdayId && x.DeletedDateTime == null).Select(x => x.BirthdayLikes.Count()).FirstOrDefaultAsync();
            return res;
        }
        public async Task<int> GetAllLikesCountForWorkAnniversary(int anniversaryId)
        {
            var res = await workRepository.All().Where(x => x.Id == anniversaryId && x.DeletedDateTime == null).Select(x => x.WorkAnniversariesLikes.Count()).FirstOrDefaultAsync();
            return res;
        }

        public async Task<bool> InsertLikeForBirthday(int birthdayId, short empId)
        {
            var res = birthdayLikeRepository.AddAsync(new BirthdayLike() { BirthdayWishId = birthdayId, EmployeeId = empId }).IsCompleted;
            await birthdayLikeRepository.SaveChangesAsync();

            await this.NotifyUser(true, true, birthdayId, empId, "");

            return res;
        }
        public async Task<bool> InsertLikeForWorkAnniversary(int anniversaryId, short empId)
        {
            var res = workAnnivarsaryLikeRepository.AddAsync(new WorkAnniversariesLike() { WorkAnniversaryId = anniversaryId, EmployeeId = empId }).IsCompleted;
            await workAnnivarsaryLikeRepository.SaveChangesAsync();

            await this.NotifyUser(true, false, anniversaryId, empId, "");

            return res;
        }

        public async Task<SingleCommentViewModel> InsertBirthdayComment(int anniversaryId, short empId, string content)
        {
            var birthday = new BirthdayComment() { BirthdayWishId = anniversaryId, EmployeeId = empId, Content = content, CreatedDateTime = GeneralUtils.GetCurrentUKTime() };

            var res = birthdayCommentRepository.AddAsync(birthday).IsCompleted;
            await birthdayCommentRepository.SaveChangesAsync();
            var comment = new SingleCommentViewModel();
            if (res)
            {
                comment = await birthdayCommentRepository.All().Where(x => x.Id == birthday.Id).Select(x => new SingleCommentViewModel()
                {
                    content = x.Content,
                    created = x.CreatedDateTime,
                    firstName = x.Employee.FirstName,
                    lastName = x.Employee.Surname,
                    Image = x.Employee.ImageBase64,
                    commentsCount = x.BirthdayWish.BirthdayComments.Count()
                }).FirstOrDefaultAsync();

                await this.NotifyUser(false, true, anniversaryId, empId, content);
            }
            return comment;
        }
        public async Task<SingleCommentViewModel> InsertWorkAnniversaryComment(int anniversaryId, short empId, string content)
        {
            // ADD SECURITY CHECKS
            var workAnniversary = new WorkAnnivarsariesComment() { WorkAnniversaryId = anniversaryId, EmployeeId = empId, Content = content, CreatedDateTime = GeneralUtils.GetCurrentUKTime() };

            var res = workCommentRepository.AddAsync(workAnniversary).IsCompleted;
            await birthdayCommentRepository.SaveChangesAsync();
            var comment = new SingleCommentViewModel();
            if (res)
            {
                comment = await workCommentRepository.All().Where(x => x.Id == workAnniversary.Id).Select(x => new SingleCommentViewModel()
                {
                    content = x.Content,
                    created = x.CreatedDateTime,
                    firstName = x.Employee.FirstName,
                    lastName = x.Employee.Surname,
                    Image = x.Employee.ImageBase64,
                    commentsCount = x.WorkAnniversary.WorkAnnivarsariesComments.Count()
                }).FirstOrDefaultAsync();

                await this.NotifyUser(false, false, anniversaryId, empId, content);
            }
            return comment;
        }

        private async Task<bool> NotifyUser(bool isLike, bool isBirthday, int anniversaryId, short empId, string content)
        {
            bool isSuccessfull = false;
            string subject = "Happy ";
            string anniversaryType = "Birthday";
            string dear = "";
            string likeOrComment = "comment";
            string employeeEmail = "";

            var wishingEmployee = await employeeRepository.All().Where(x => x.Id == empId).FirstOrDefaultAsync();
            string wishingEmployeeName = wishingEmployee.FirstName + " " + wishingEmployee.Surname;

            if (isLike)
                likeOrComment = "like";
            else
                content = "“" + content + "”</br></br>";

            if (isBirthday)
            {
                var birthday = await birthdayRepository.All().Where(x => x.Id == anniversaryId).Include(x => x.Employee).FirstOrDefaultAsync();
                dear = birthday.Employee.FirstName + " " + birthday.Employee.Surname;
                employeeEmail = birthday.Employee.EmailAddress;
            }
            else
            {
                var workAnniversary = await workRepository.All().Where(x => x.Id == anniversaryId).Include(x => x.Employee).FirstOrDefaultAsync();

                employeeEmail = workAnniversary.Employee.EmailAddress;
                dear = workAnniversary.Employee.FirstName + " " + workAnniversary.Employee.Surname;
                anniversaryType = "Anniversary";
            }
            subject += anniversaryType;

            // build email content
            var body = string.Format("<p>Dear {0}</br></br>You have just received a {1} from {2} to congratulate you on your {3}</br></br>{4}Enjoy the rest of your day!</br></br>Best wishes,</br>my plus team</p>", dear, likeOrComment, wishingEmployeeName, anniversaryType, content);

            emailService.SendMail("flow plus <flowplus@translateplus.com>", employeeEmail, subject, body);

            return isSuccessfull;
        }
    }
}
