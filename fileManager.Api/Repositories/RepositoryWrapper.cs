using fileManager.Api.Data;
using fileManager.Api.IRepositories;

namespace fileManager.Api.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly AppDbContext _context;
        public RepositoryWrapper(AppDbContext context)
        {
            _context = context;
        }

        private IPersonalDataRepo _personalDataRepo = default!;
        private IPersonalDocsRepo _personalDocsRepo = default!;

        public IPersonalDataRepo PersonalDataRepo => _personalDataRepo ??= new PersonalDataRepo(_context);
        public IPersonalDocsRepo PersonalDocsRepo => _personalDocsRepo ??= new PersonalDocsRepo(_context);

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
