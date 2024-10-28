namespace SE310.P12_BaiKTTH_FE.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public double Rating { get; set; }
    public int CategoryId { get; set; }
}