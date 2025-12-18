using fileManager.Api.Dtos;
using fileManager.Api.Dtos.PersonalData;
using fileManager.Api.Entities;
using fileManager.Api.Helper;
using fileManager.Api.IRepositories;
using fileManager.Api.IServices;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace fileManager.Api.Services
{
    public class PersonalDataService : IPersonalDataService
    {
        private readonly IRepositoryWrapper _Wrapper; 
        public PersonalDataService(IRepositoryWrapper wrapper)
        {
            _Wrapper = wrapper;
        }

        public async Task<ResponseDto<PaginationDto<PersonalDataGet>>> GetAll(PaginationQuery paginationQuery, PersonalDataFilter filter)
        {
            #region Apply Filter
            var query = _Wrapper.PersonalDataRepo.GetAll()
                .Where(d => string.IsNullOrEmpty(filter.FullName) || d.FullName.ToLower().Contains(filter.FullName.ToLower()))
                .Where(d => string.IsNullOrEmpty(filter.MotherName) || d.MotherName.ToLower().Contains(filter.MotherName.ToLower()))
                .Where(d => !filter.BirthDate.HasValue || d.BirthDate == filter.BirthDate)
                .Where(d => !filter.RetirementNumber.HasValue || d.RetirementNumber == filter.RetirementNumber)
                .Where(d => string.IsNullOrEmpty(filter.DeliveredTo) || d.DeliveredTo.ToLower().Contains(filter.DeliveredTo.ToLower()))
                .Where(d => string.IsNullOrEmpty(filter.ReceivedFrom) || d.ReceivedFrom.ToLower().Contains(filter.ReceivedFrom.ToLower()));
            #endregion
            var data = await query
                .OrderByDescending(d => d.CreatedAt)
                .ApplyPagging(paginationQuery)
                .ProjectToType<PersonalDataGet>()
                .ToListAsync();

            var count = await query.CountAsync();
            var metadata = new PaginationMetadata(count, paginationQuery);

            return new ResponseDto<PaginationDto<PersonalDataGet>>(
                new PaginationDto<PersonalDataGet>(data, metadata));
        }
        public async Task<ResponseDto<PersonalDataGet>> Add(PersonalDataAdd form)
        {
            var personalData = form.Adapt<PersonalData>();

            await _Wrapper.PersonalDataRepo.Insert(personalData);
            await _Wrapper.SaveAllAsync();

            return new ResponseDto<PersonalDataGet>(personalData.Adapt<PersonalDataGet>());
        }
        //public async Task<ResponseDto<PersonalDataGet>> GetById(int id)
        //{

        //}
        //public async Task<ResponseDto<PersonalDataGet>> Update(int id, PersonalDataUpdate form)
        //{

        //}
        //public async Task<ResponseDto<bool>> Delete(int id)
        //{

        //}
    }
}
