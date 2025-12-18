using fileManager.Api.Data;
using fileManager.Api.Entities;
using fileManager.Api.IRepositories;

namespace fileManager.Api.Repositories
{
    public class PersonalDocsRepo : RepositoryBase<PersonalDocument, int, AppDbContext>, IPersonalDocsRepo
    {
        public PersonalDocsRepo(AppDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
