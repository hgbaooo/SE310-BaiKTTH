﻿namespace SE310.P12_BaiKTTH_BE.Models;

public class ProductCategory
{
    public int ProductId { get; set; }
    public int CategoryId { get; set; }
    public Product Product { get; set; }
    public Category Category { get; set; }  
}