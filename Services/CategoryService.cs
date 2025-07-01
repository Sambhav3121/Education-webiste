using Education.DTO;
using Education.Models;
using Education.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Education.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category> CreateCategoryAsync(CategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Select(c => new CategoryDto
                {
                    Name = c.Name
                })
                .ToListAsync();
        }
    }
}
