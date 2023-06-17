using Microsoft.Extensions.Logging;
using Model;
using System.Text.Json;

namespace Service.FileSystem;

public class LocalFileSystemService : IFileSystemService
{
    private readonly ILogger<LocalFileSystemService> logger;

    public LocalFileSystemService(ILogger<LocalFileSystemService> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// 將內容儲存到檔案系統中
    /// </summary>
    /// <param name="fileContent"></param>
    /// <param name="fileNameArgs"></param>
    /// <returns></returns>
    public async Task<string> SaveFileToFileSystemAsync(string fileContent, string[] fileNameArgs)
    {
        if(!Directory.Exists(_104Parameters.JobListDir))
            Directory.CreateDirectory(_104Parameters.JobListDir);

        var fileName = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}-{string.Join("-", fileNameArgs)}.json";

        var filePath = Path.Combine(_104Parameters.JobListDir, fileName);

        logger.LogInformation($"{nameof(LocalFileSystemService)} {nameof(SaveFileToFileSystemAsync)} write file. {{filePath}}", filePath);

        await File.WriteAllTextAsync(filePath, fileContent);

        return filePath;
    }

    /// <summary>
    /// 將內容儲存到檔案系統中
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    /// <param name="fileNameArgs"></param>
    /// <returns></returns>
    public Task<string> SaveFileToFileSystemAsync<T>(T model, string[] fileNameArgs)
    {
        return SaveFileToFileSystemAsync(JsonSerializer.Serialize(model), fileNameArgs);
    }
}
