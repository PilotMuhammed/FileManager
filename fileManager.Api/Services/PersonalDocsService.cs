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

            // 🔴 تحويل أسماء الملفات القادمة من JSON إلى List
            List<string> customFileNames = new();

            if (!string.IsNullOrWhiteSpace(form.FileNames))
            {
                customFileNames = System.Text.Json.JsonSerializer
                    .Deserialize<List<string>>(form.FileNames);
            }



            string baseFolder = Path.Combine(_env.WebRootPath, "uploads");
            string userFolder = Path.Combine(baseFolder, form.PersonalDataId.ToString());
            if (!Directory.Exists(userFolder))
                Directory.CreateDirectory(userFolder);

            var resultDocs = new List<PersonalDocsDTO>();

            for (int i = 0; i < form.Files.Count; i++)
            {
                var file = form.Files[i];
                if (file == null || file.Length == 0) continue;

                var uploadedAt = DateTime.UtcNow;

                // 🔴 الاسم القادم من الفرونت (إن وجد)
                string originalName = Path.GetFileNameWithoutExtension(file.FileName);
                string extension = Path.GetExtension(file.FileName);

                string finalFileName =
                    (customFileNames != null && customFileNames.Count > i && !string.IsNullOrWhiteSpace(customFileNames[i]))
                    ? customFileNames[i]
                    : originalName;

                // تنظيف الاسم من رموز غير مسموحة
                finalFileName = string.Concat(finalFileName.Split(Path.GetInvalidFileNameChars()));

                string datePart = uploadedAt.ToString("yyyy-MM-ddTHH-mm");
                string storedFileName = $"{datePart}_{finalFileName}{extension}";

                string filePath = Path.Combine(userFolder, storedFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var personalDoc = new PersonalDocument
                {
                    PersonalDataId = form.PersonalDataId,

                    // 🔴 الاسم النهائي (المعدل أو الأصلي)
                    FileName = $"{finalFileName}{extension}",

                    FilePath = $"/uploads/{form.PersonalDataId}/{storedFileName}",
                    UploadedAt = uploadedAt
                };

                await _Wrapper.PersonalDocsRepo.Insert(personalDoc);
                resultDocs.Add(personalDoc.Adapt<PersonalDocsDTO>());
            }


            await _Wrapper.SaveAllAsync();
            return new ResponseDto<List<PersonalDocsDTO>>(resultDocs);
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
