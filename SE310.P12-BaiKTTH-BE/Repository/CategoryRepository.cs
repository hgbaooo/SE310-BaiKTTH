using SE310.P12_BaiKTTH_BE.Data;
using SE310.P12_BaiKTTH_BE.Interfaces;
using SE310.P12_BaiKTTH_BE.Models;

namespace SE310.P12_BaiKTTH_BE.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly DataContext _context;

    public CategoryRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<Category> GetCategories()
    {
        return _context.Categories.OrderBy(c => c.Id).ToList();
    }

    public Category GetCategoryById(int id)
    {
        return _context.Categories.FirstOrDefault(c => c.Id == id);
    }

    public bool CreateCategory(Category category)
    {
        _context.Categories.Add(category);
        return Save();
    }

    public bool UpdateCategory(Category category)
    {
        _context.Categories.Update(category);
        return Save();
    }

    public bool DeleteCategory(int id)
    {
        var category = _context.Categories.Find(id);
        
        if (category == null)
        {
            return false; // Category not found
        }

        _context.Categories.Remove(category);
        return Save();
    }

    public bool CategoryExists(int id)
    {
        return _context.Categories.Any(c => c.Id == id);
    }

    public bool Save()
    {
        return _context.SaveChanges() > 0;
    }
}