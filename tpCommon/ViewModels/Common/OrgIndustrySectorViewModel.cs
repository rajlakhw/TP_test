namespace ViewModels.Common
{
    public class OrgIndustrySectorViewModel
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public short MainIndustryId { get; set; }
        public short AltairIndustryId { get; set; }
    }
}
