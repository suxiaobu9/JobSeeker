using System;
using System.Collections.Generic;

namespace Model.JobSeekerDb
{
    public partial class Company
    {
        public Company()
        {
            Jobs = new HashSet<Job>();
        }

        public string Id { get; set; } = null!;
        /// <summary>
        /// 公司名稱
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// 公司網址
        /// </summary>
        public string Url { get; set; } = null!;
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
        /// 忽略理由
        /// </summary>
        public string? IgnoreReason { get; set; }
        /// <summary>
        /// 取得資料的網址
        /// </summary>
        public string GetInfoUrl { get; set; } = null!;
        /// <summary>
        /// 主要商品/服務
        /// </summary>
        public string? Product { get; set; }
        /// <summary>
        /// 公司描述
        /// </summary>
        public string? Profile { get; set; }
        /// <summary>
        /// 福利
        /// </summary>
        public string? Welfare { get; set; }
        /// <summary>
        /// 手動更新次數
        /// </summary>
        public int UpdateCount { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }
    }
}
