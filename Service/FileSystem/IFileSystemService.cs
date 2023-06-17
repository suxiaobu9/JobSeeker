namespace Service.FileSystem;

public interface IFileSystemService
{
    /// <summary>
    /// 將內容儲存到檔案系統中
    /// </summary>
    /// <param name="fileContent"></param>
    /// <param name="fileNameArgs"></param>
    /// <returns></returns>
    public Task<string> SaveFileToFileSystemAsync(string fileContent, string[] fileNameArgs);

    /// <summary>
    /// 將內容儲存到檔案系統中
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    /// <param name="fileNameArgs"></param>
    /// <returns></returns>
    public Task<string> SaveFileToFileSystemAsync<T>(T model, string[] fileNameArgs);
}
