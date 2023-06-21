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

        public virtual ICollection<Job> Jobs { get; set; }
    }
}
