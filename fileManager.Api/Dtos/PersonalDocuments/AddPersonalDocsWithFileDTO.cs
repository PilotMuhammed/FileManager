namespace fileManager.Api.Dtos.PersonalDocuments
{
    public class AddPersonalDocsWithFileDTO
    {
        public int PersonalDataId { get; set; }
        public List<IFormFile> Files { get; set; } 
        public string FileNames { get; set; }
    }
}
