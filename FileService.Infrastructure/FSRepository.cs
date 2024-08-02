using FileService.Domain.Entity;
using FileService.Domain;
using Microsoft.EntityFrameworkCore;

namespace FileService.Infrastructure;

class FSRepository : IFSRepository
{
    private readonly FSDbContext dbContext;

    public FSRepository(FSDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task<UploadedItem?> FindFileAsync(long fileSize, string sha256Hash)
    {
        return dbContext.UploadItems.FirstOrDefaultAsync(u => u.FileSizeInBytes == fileSize
        && u.FileSHA256Hash == sha256Hash);
    }
    public async Task<List<UploadedItem>?> GetAllFileAsync()
    {
        return await dbContext.UploadItems.ToListAsync();
    }
    public async Task<UploadedItem?> FindFileByHashAsync(string sha256Hash)
    {
        return await dbContext.UploadItems.FirstOrDefaultAsync(u => u.FileSHA256Hash == sha256Hash);
    }
    public async Task<UploadedItem?> FindFileByIdAsync(Guid Id)
    {
        return await dbContext.UploadItems.FirstOrDefaultAsync(u => u.Id == Id);
    }
    public async Task<UploadedItem?> FindFileByNameAsync(string fileName)
    {
        return await dbContext.UploadItems.FirstOrDefaultAsync(u => u.FileName == fileName);
    }
}
