using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto;

public class JobListDto<T> where T : SimpleJobInfoDto
{
    public IEnumerable<T>? JobList { get; set; }
}

public class SimpleJobInfoDto
{
    public string CompanyId { get; set; } = null!;

    public string JobId { get; set; } = null!;

}