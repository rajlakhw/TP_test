using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class WorkFlowStep
    {
        public int Id { get; set; }
        public int WorkFlowId { get; set; }
        public int LanguageService { get; set; }
        public int DefaultTime { get; set; }
        public int Secuence { get; set; }
    }
}
