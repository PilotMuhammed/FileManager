using fileManager.Api.Interfaces;

namespace fileManager.Api.IRepositories
{
    public interface IRepositoryWrapper : IScopedService
    {
        IPersonalDataRepo PersonalDataRepo { get; }
        IPersonalDocsRepo PersonalDocsRepo { get; }
        Task<bool> SaveAllAsync();
    }
}
