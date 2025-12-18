using fileManager.Api.Dtos;
using fileManager.Api.Dtos.PersonalDocuments;
using fileManager.Api.Helper;
using fileManager.Api.Interfaces;

namespace fileManager.Api.IServices
{
    public interface IPersonalDocsService : IScopedService
    {
        Task<ResponseDto<PaginationDto<PersonalDocsDTO>>> GetAll(PaginationQuery paginationQuery, FilterPersonalDocs filter);
        Task<ResponseDto<List<PersonalDocsDTO>>> Add(AddPersonalDocsWithFileDTO form);
        Task<ResponseDto<List<PersonalDocsDTO>>> GetByUser(int personalDataId);
        Task<ResponseDto<bool>> DeleteUserFile(int userId, int fileId);
    }
}
