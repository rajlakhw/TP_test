using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.HomePage
{
    public class flowPlusExtClientHomePage
    {
        public int NumberOfPendingQuotes { get; set; }
        public decimal ValueOfPendingQuotes { get; set; }
        public int NumberOfOpenProjects { get; set; }
        public decimal ValueOfOpenProjects { get; set; }
        public int NumberOfServiceInProgressProjects { get; set; }
        public decimal ValueOfServiceInProgressProjects { get; set; }
        public int NumberOfInReviewProjects { get; set; }
        public decimal ValueOfInReviewProjects { get; set; }
        public int NumberOfFinalChecksProjects { get; set; }
        public decimal ValueOfFinalChecksProjects { get; set; }
        public int NumberOfReadyToCollectProjects { get; set; }
        public decimal ValueOfReadyToCollectProjects { get; set; }
        public int TotalNumberOfJobItemsInOpenProjects { get; set; }
        public int TotalNumberOfCompletedJobItems { get; set; }
        public int TotalWordCountInOpenProjects { get; set; }
        public int TotalWordCountOfCompletedJobItems { get; set; }
        public decimal NumberOfJobItemPercentage
        {
            get
            {
                decimal Percentage = 0;
                if (TotalNumberOfJobItemsInOpenProjects != 0 && TotalNumberOfCompletedJobItems != 0)
                {
                    Percentage = (decimal)TotalNumberOfCompletedJobItems / (decimal)TotalNumberOfJobItemsInOpenProjects * 100;
                }

                return Percentage;
            }
            set { }
        }
        public decimal NumberOfWordCountPercentage
        {
            get
            {
                decimal Percentage = 0;
                if (TotalWordCountInOpenProjects != 0 && TotalWordCountOfCompletedJobItems != 0)
                {
                    Percentage = (decimal)TotalWordCountOfCompletedJobItems / (decimal)TotalWordCountInOpenProjects * 100;
                }

                return Percentage;
            }
            set { }
        }

    }
}
