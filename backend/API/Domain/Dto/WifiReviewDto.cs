namespace API.Domain.Dto;

public class WifiReviewDto
{
    public string? WifiId { get; set; }
    public string UserId { get; set; }
    public string? Text { get; set; }
    public int? Rating { get; set; }
}
