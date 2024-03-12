using MediaEncoder.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaEncoder.Domain;

public interface IMediaEncoderRepository
{
    Task<EncodingItem?> FindCompletedOneAsync(string fileHash, long fileSize);
    Task<EncodingItem[]> FindAsync(ItemStatus status);
}
