using SE310.P12_BaiKTTH_BE.Models;

namespace SE310.P12_BaiKTTH_BE.Interfaces;

public interface IProductRepository
{
    ICollection<Product> GetProducts();
    Product GetProductById(int id);
    ICollection<Product> GetProductsByCategoryId(int categoryId);
    bool CreateProduct(Product product);
    bool UpdateProduct(Product product);
    bool DeleteProduct(int id);

    bool ProductExist(int id);
    bool Save();
}