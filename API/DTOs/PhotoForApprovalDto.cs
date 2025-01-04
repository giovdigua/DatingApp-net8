namespace API.DTOs;

public class PhotoForApprovalDto
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public bool IsApproved { get; set; } = false;
    public string? UserName { get; set; }
}
