namespace fileManager.Api.Entities
{
    public class PersonalDocument : BaseEntity<int>
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int PersonalDataId { get; set; }
        public DateTime UploadedAt { get; set; }
        public PersonalData PersonalData { get; set; }
    }
}
