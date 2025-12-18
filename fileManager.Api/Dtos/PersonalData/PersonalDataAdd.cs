namespace fileManager.Api.Dtos.PersonalData
{
    public class PersonalDataAdd
    {
        public string FullName { get; set; }
        public string MotherName { get; set; }
        public DateTime BirthDate { get; set; }
        public int RetirementNumber { get; set; }
        public string DeliveredTo { get; set; }
        public string ReceivedFrom { get; set; }
    }
}
