namespace SearchService.Domain;

public record SearchResult<T>(IEnumerable<T> Values, long TotalCount);
