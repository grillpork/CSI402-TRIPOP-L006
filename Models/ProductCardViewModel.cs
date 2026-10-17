namespace tripop_8021_lab006.Models;

public class ProductCardViewModel
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Price { get; set; } = string.Empty;
    public string? OldPrice { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string AccentClass { get; set; } = string.Empty;
    public bool IsNew { get; set; }
}
