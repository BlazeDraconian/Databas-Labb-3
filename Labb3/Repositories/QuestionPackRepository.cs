using Labb3.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3.Repositories
{
    class QuestionPackRepository
    {
        private readonly MongoDBContext _context;
        public QuestionPackRepository(MongoDBContext context)
        {
            _context = context;
        }

        public async Task<List<QuestionPack>> GetAllAsync()
        {
           var result = await _context.QuestionPacks
                .Find(_ =>  true)
                .ToListAsync();
            return result;
        }

        public async Task DeletePackAsync(string packName)
        {
            await _context.QuestionPacks.DeleteOneAsync(p => p.Name == packName);       
        }

        public async Task SavePackAsync(QuestionPack pack)
        {
            await _context.QuestionPacks.ReplaceOneAsync
            (
                p => p.Name == pack.Name,
                pack,
                new ReplaceOptions { IsUpsert = true }
            );
        }

        
    }
}
