using fileManager.Api.Dtos;
using fileManager.Api.Dtos.PersonalDocuments;
using fileManager.Api.Entities;
using fileManager.Api.Helper;
using fileManager.Api.IRepositories;
using fileManager.Api.IServices;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace fileManager.Api.Services
{
    public class PersonalDocsService : IPersonalDocsService
    {
        private readonly IRepositoryWrapper _Wrapper;
        private readonly IWebHostEnvironment _env;
        public PersonalDocsService(IRepositoryWrapper wrapper, IWebHostEnvironment env)
        {
            _Wrapper = wrapper;
            _env = env;
        }

        public async Task<ResponseDto<PaginationDto<PersonalDocsDTO>>> GetAll(PaginationQuery paginationQuery, FilterPersonalDocs filter)
        {
            #region Apply Filter
            var query = _Wrapper.PersonalDocsRepo.GetAll()
                .Where(d => string.IsNullOrEmpty(filter.FileName) || d.FileName.ToLower().Contains(filter.FileName.ToLower()));
                //.Where(d => string.IsNullOrEmpty(filter.FilePath) || d.FilePath.ToLower().Contains(filter.FilePath.ToLower()))
            #endregion
            var data = await query
                .OrderByDescending(d => d.CreatedAt)
                .ApplyPagging(paginationQuery)
                .ProjectToType<PersonalDocsDTO>()
                .ToListAsync();

            var count = await query.CountAsync();
            var metadata = new PaginationMetadata(count, paginationQuery);

            return new ResponseDto<PaginationDto<PersonalDocsDTO>>(
                new PaginationDto<PersonalDocsDTO>(data, metadata));
        }
        public async Task<ResponseDto<List<PersonalDocsDTO>>> Add(AddPersonalDocsWithFileDTO form)
        {
            if (form.Files == null || !form.Files.Any())
                return new ResponseDto<List<PersonalDocsDTO>>("No file uploaded.");                             

            string baseFolder = Path.Combine(_env.WebRootPath, "uploads");
            string userFolder = Path.Combine(baseFolder, form.PersonalDataId.ToString());
            if (!Directory.Exists(userFolder))
                Directory.CreateDirectory(userFolder);

            var resultDocs = new List<PersonalDocsDTO>();

            foreach (var file in form.Files)
            {
                if (file == null || file.Length == 0) continue;

                var uploadedAt = DateTime.UtcNow;

                string datePart = uploadedAt.ToString("yyyy-MM-ddTHH-mm");
                string uniqueFileName = $"{datePart}_{file.FileName}";
                string filePath = Path.Combine(userFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var personalDoc = new PersonalDocument
                {
                    PersonalDataId = form.PersonalDataId,
                    FileName = file.FileName,
                    FilePath = $"/uploads/{form.PersonalDataId}/{uniqueFileName}",
                    UploadedAt = DateTime.UtcNow
                };

                await _Wrapper.PersonalDocsRepo.Insert(personalDoc);
                resultDocs.Add(personalDoc.Adapt<PersonalDocsDTO>());
            }

            await _Wrapper.SaveAllAsync();
            return new ResponseDto<List<PersonalDocsDTO>>(resultDocs);

            //return new ResponseDto<PersonalDocsDTO>(doc.Adapt<PersonalDocsDTO>());
        }

        public async Task<ResponseDto<List<PersonalDocsDTO>>> GetByUser(int personalDataId)
        {
             var docs = await _Wrapper.PersonalDocsRepo.GetAll()
            .Where(d => d.PersonalDataId == personalDataId)
            .ProjectToType<PersonalDocsDTO>()
            .ToListAsync();

            return new ResponseDto<List<PersonalDocsDTO>>(docs);
        }

        public async Task<ResponseDto<bool>> DeleteUserFile(int userId, int fileId)
        {
            var doc = await _Wrapper.PersonalDocsRepo.GetAll()
                .FirstOrDefaultAsync(d => d.Id == fileId && d.PersonalDataId == userId);
            if (doc == null)
                return new ResponseDto<bool>("Document not found or not authorized.");

            var filePath = Path.Combine(_env.WebRootPath, doc.FilePath.TrimStart('/'));
            if (File.Exists(filePath))
                File.Delete(filePath);

            _Wrapper.PersonalDocsRepo.Delete(doc.Id);
            await _Wrapper.SaveAllAsync();

            return new ResponseDto<bool>("Document deleted successfully.");
        }
    }
}
