namespace API.Domain;

public class WifiReviewDto
{
    public string? WifiId { get; set; }
    public string UserId { get; set; }
    public int? Rating { get; set; }
    public string? Text { get; set; }
}