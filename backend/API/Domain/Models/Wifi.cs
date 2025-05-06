namespace API.Domain.Models;

public class Wifi : TimeStampedEntity
{
    public int UserId { get; set; }
    
    public string Country { get; set; }
    
    public string City { get; set; }
    
    public string PlaceName { get; set; }
    
    public string Street { get; set; }
    
    public string BuildingNumber { get; set; }
    
    public int? PostalCode { get; set; }
    
    public bool IsFree { get; set; }
    
    public string? Password { get; set; }
}