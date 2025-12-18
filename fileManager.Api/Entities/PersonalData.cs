namespace fileManager.Api.Entities
{
    public class PersonalData : BaseEntity<int>
    {
        public string FullName { get; set; }
        public string MotherName { get; set; }
        public DateTime BirthDate { get; set; }
        public int RetirementNumber { get; set; }
        public string DeliveredTo { get; set; }
        public string ReceivedFrom { get; set; }
        public ICollection<PersonalDocument> Documents { get; set; }
    }
}
