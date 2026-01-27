using Labb3.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Labb3.Repositories
{
    class CategoryRepository
    {
        private readonly MongoDBContext _context;

        public CategoryRepository(MongoDBContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories
                .Find(_=> true)
                .ToListAsync();
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.InsertOneAsync(category);
        }

        public async Task DeleteAsync(Category category)
        {
            var filter = Builders<Category>.Filter.Eq(c=> c.Id, category.Id);
            await _context.Categories.DeleteOneAsync(filter);
        }
    }
}
