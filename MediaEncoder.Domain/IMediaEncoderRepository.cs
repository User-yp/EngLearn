using MediaEncoder.Domain.Entity;

namespace MediaEncoder.Domain;

public interface IMediaEncoderRepository
{
    Task<EncodingItem?> FindCompletedOneAsync(string fileHash, long fileSize);
    Task<EncodingItem[]> FindAsync(ItemStatus status);
}
