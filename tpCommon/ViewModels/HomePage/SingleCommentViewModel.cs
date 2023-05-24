using System;

namespace ViewModels.HomePage
{
    public class SingleCommentViewModel
    {
        //data.Image, data.firstName, data.lastName, data.created, data.content
        public string Image { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string content { get; set; }
        public DateTime created { get; set; }
        public string formattedDate { get => created.ToString("dddd dd MMMM"); set { } }
        public int commentsCount { get; set; }
    }
}
