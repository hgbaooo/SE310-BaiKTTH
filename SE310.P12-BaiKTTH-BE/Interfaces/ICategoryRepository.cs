using SE310.P12_BaiKTTH_BE.Models;

namespace SE310.P12_BaiKTTH_BE.Interfaces;

public interface ICategoryRepository
{
    // Retrieve a collection of all categories
    ICollection<Category> GetCategories();

    // Retrieve a single category by ID
    Category GetCategoryById(int id);

    // Create a new category
    bool CreateCategory(Category category);

    // Update an existing category
    bool UpdateCategory(Category category);

    // Delete a category by ID
    bool DeleteCategory(int id);

    // Check if a category exists by ID
    bool CategoryExists(int id);

    // Save changes to the database
    bool Save();
}