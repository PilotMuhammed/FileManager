namespace fileManager.Api.Dtos.PersonalData
{
    public class PersonalDataGet
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string MotherName { get; set; }
        public DateTime BirthDate { get; set; }
        public int RetirementNumber { get; set; }
        public string DeliveredTo { get; set; }
        public string ReceivedFrom { get; set; }
    }
}
