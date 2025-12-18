namespace fileManager.Api.Dtos
{
    public class ListDto<T>
    {
        public T? Id { get; set; }
        public string? Title { get; set; }
    }
}
