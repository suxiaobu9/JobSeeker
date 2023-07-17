namespace Model.Dto;

public class JobListWithPageDto : JobListDto<SimpleJobInfoDto>
{
    public int TotalPage { get; set; }
}
