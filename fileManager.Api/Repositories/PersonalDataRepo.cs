using fileManager.Api.Data;
using fileManager.Api.Entities;
using fileManager.Api.IRepositories;

namespace fileManager.Api.Repositories
{
    public class PersonalDataRepo : RepositoryBase<PersonalData, int, AppDbContext>, IPersonalDataRepo
    {
        public PersonalDataRepo(AppDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
