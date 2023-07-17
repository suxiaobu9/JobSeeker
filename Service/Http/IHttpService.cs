using Model.Dto;

namespace Service.Http;

public interface IHttpService
{
    /// <summary>
    /// 取得職缺清單
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Task<IEnumerable<T>> GetJobList<T>(string url) where T : JobListDto;

    /// <summary>
    /// 取得職缺資訊
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <returns></returns>
    public Task<T> GetJobInfo<T>(string url) where T : JobDto;

    /// <summary>
    /// 取得公司資訊
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <returns></returns>
    public Task<T> GetCompanyInfo<T>(string url) where T : CompanyDto;

}
