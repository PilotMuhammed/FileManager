namespace fileManager.Api.Dtos.PersonalDocuments
{
    public class PersonalDocsDTO
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
