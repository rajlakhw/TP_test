namespace ViewModels.Contact
{
    public class ContactTableViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public short? LandlineCountryId { get; set; }
        public string LandlineNumber { get; set; }
        public short? MobileCountryId { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string JobTitle { get; set; }
        public string Department { get; set; }
    }
}
