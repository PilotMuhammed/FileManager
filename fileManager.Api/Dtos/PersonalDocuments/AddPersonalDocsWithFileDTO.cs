namespace fileManager.Api.Dtos.PersonalDocuments
{
    public class AddPersonalDocsWithFileDTO
    {
        public int PersonalDataId { get; set; }
        public List<IFormFile> Files { get; set; } 
    }
}
