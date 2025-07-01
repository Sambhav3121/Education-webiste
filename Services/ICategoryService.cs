using Education.DTO;
using Education.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Education.Services
{
    public interface ICategoryService
    {
        Task<Category> CreateCategoryAsync(CategoryDto dto);
        Task<List<CategoryDto>> GetAllCategoriesAsync();
    }
}
