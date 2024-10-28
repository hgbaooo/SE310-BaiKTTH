using SE310.P12_BaiKTTH_BE.Data;
using SE310.P12_BaiKTTH_BE.Interfaces;
using SE310.P12_BaiKTTH_BE.Models;

namespace SE310.P12_BaiKTTH_BE.Repository;

public class ProductRepository: IProductRepository
{
    private readonly DataContext _context;
    public ProductRepository(DataContext context)
    {
        _context = context;
    }
    
    public ICollection<Product> GetProducts()
    {
        return _context.Products.OrderBy(p => p.Id).ToList();
    }

    public Product GetProductById(int id)
    {
        return _context.Products.Where(p => p.Id == id).FirstOrDefault();
    }

    public ICollection<Product> GetProductsByCategoryId(int categoryId)
    {
        return _context.Products.Where(p => p.CategoryId == categoryId).ToList();
    }

    public bool CreateProduct(Product product)
    {
        _context.Add(product);
        return Save();
    }
    
    public bool UpdateProduct(Product product)
    {
        _context.Update(product);
        return Save();
    }

    public bool DeleteProduct(int id)
    {
        // Find the product by its ID
        var product = _context.Products.Find(id);
            
        // Check if the product exists
        if (product == null)
        {
            return false; // Product not found
        }

        // Remove the product from the context
        _context.Products.Remove(product);
            
        // Save changes to the database
        return Save();
    }
    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
    
    public bool ProductExist(int id)
    {
        return _context.Products.Any(c => c.Id == id);
    }
    public bool CategoryExist(int categoryId)
    {
        return _context.Categories.Any(c => c.Id == categoryId);
    }

}