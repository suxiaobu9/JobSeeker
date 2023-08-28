using System;
using System.Collections.Generic;

namespace Model.JobSeekerDb
{
    public partial class Job
    {
        public string Id { get; set; } = null!;
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// 工作內容
        /// </summary>
        public string WorkContent { get; set; } = null!;
        /// <summary>
        /// 職缺網址
        /// </summary>
        public string Url { get; set; } = null!;
        /// <summary>
        /// 薪資
        /// </summary>
        public string Salary { get; set; } = null!;
        /// <summary>
        /// 其他要求
        /// </summary>
        public string? OtherRequirement { get; set; }
        /// <summary>
        /// 已讀
        /// </summary>
        public bool HaveRead { get; set; }
        /// <summary>
        /// 已刪除
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 工作地點
        /// </summary>
        public string JobPlace { get; set; } = null!;
        public string CompanyId { get; set; } = null!;
        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateUtcAt { get; set; }
        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateUtcAt { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 忽略不看
        /// </summary>
        public bool Ignore { get; set; }
        /// <summary>
        /// 忽略不看
        /// </summary>
        public string? IgnoreReason { get; set; }
        /// <summary>
        /// 取得資訊的網址
        /// </summary>
        public string GetInfoUrl { get; set; } = null!;
        /// <summary>
        /// 手動更新次數
        /// </summary>
        public int UpdateCount { get; set; }
        /// <summary>
        /// 最後更新時間
        /// </summary>
        public string? LatestUpdateDate { get; set; }
        /// <summary>
        /// 來源
        /// </summary>
        public string CompanySourceFrom { get; set; } = null!;

        public virtual Company Company { get; set; } = null!;
    }
}
