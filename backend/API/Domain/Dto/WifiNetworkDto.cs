namespace API.Domain.Dto;

public class WifiNetworkDto
{
    public string? WifiId { get; set; }
    public string? UserId { get; set; }
    public string? City { get; set; }
    public string? Name { get; set; }
    public string? Street { get; set; }
    public int? BuildingNumber { get; set; }
    public int? PostalCode { get; set; }
    public bool IsFree { get; set; }
    public string? Password { get; set; }
}