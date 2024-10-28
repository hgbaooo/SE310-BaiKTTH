namespace SE310.P12_BaiKTTH_BE.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; } // Corrected to Products
    }
}