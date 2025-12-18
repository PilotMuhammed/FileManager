namespace fileManager.Api.Helper
{
    public interface IPaginationQuery
    {
        int? Page { get; set; }
        int? PageSize { get; set; }
    }
}
