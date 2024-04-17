namespace SearchService.Domain;

public  interface ISearchRepository<T>
{
    Task UpsertAsync(T entity);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(Guid Id, Dictionary<string, string> updatedFields);
    Task<SearchResult<T>> SearchAsync(string keyWord, int pageIndex, int pageSize);
}
