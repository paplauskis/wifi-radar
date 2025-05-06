namespace API.Domain.Models;

public class WifiReview : TimeStampedEntity
{
    public int WifiId { get; set; }
    
    public int UserId { get; set; }
    
    public string? Text { get; set; }
    
    public int Rating { get; set;}
}