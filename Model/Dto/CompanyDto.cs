using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto;

public class CompanyDto
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    /// <summary>
    /// 產品/服務
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
    /// 來源
    /// </summary>
    public string SourceFrom { get; set; } = null!;
}
