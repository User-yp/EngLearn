
namespace FileService.WebAPI.Controllers;

public record GetFileResponse(string FileName, Uri BackupUrl, Uri RemoteUrl);