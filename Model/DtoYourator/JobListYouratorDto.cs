using System.Text.Json.Serialization;

namespace Model.DtoYourator;

public class JobListYouratorDto
{
    [JsonPropertyName("payload")]
    public Payload? Payload { get; set; }
}

public class Payload
{
    [JsonPropertyName("jobs")]
    public Job[]? Jobs { get; set; }
}

public class Job
{
    
    [JsonPropertyName("id")]
    public int? Id { get; set; }
    
    [JsonPropertyName("company")]
    public Company? Company { get; set; }
}

public class Company
{
    [JsonPropertyName("path")]
    public string? Path { get; set; }
    
}
