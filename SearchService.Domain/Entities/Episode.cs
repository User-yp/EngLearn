namespace SearchService.Domain.Entities;

public record Episode(Guid Id, string CnName, string EngName, string PlainSubtitle, Guid AlbumId);
