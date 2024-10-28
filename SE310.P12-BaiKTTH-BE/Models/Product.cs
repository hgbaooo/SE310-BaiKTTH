﻿namespace SE310.P12_BaiKTTH_BE.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public double Rating { get; set; }
    public ICollection<ProductCategory> ProductCategories { get; set; }
}