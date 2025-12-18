using fileManager.Api.Dtos;
using fileManager.Api.Dtos.PersonalData;
using fileManager.Api.Helper;
using fileManager.Api.Interfaces;

namespace fileManager.Api.IServices
{
    public interface IPersonalDataService : IScopedService
    {
        Task<ResponseDto<PaginationDto<PersonalDataGet>>> GetAll(PaginationQuery paginationQuery, PersonalDataFilter filter);
        Task<ResponseDto<PersonalDataGet>> Add(PersonalDataAdd form);
        //Task<ResponseDto<PersonalDataGet>> GetById(int id);
        //Task<ResponseDto<PersonalDataGet>> Update(int id, PersonalDataUpdate form);
        //Task<ResponseDto<bool>> Delete(int id);
    }
}
