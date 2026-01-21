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

        public async Task <QuestionPack> DeletePackAsync(string packName)
        {
            var filter = Builders<QuestionPack>.Filter.Eq(p => p.Name, packName);
            var result = await _context.QuestionPacks.DeleteOneAsync(filter);
            return null;
    
        }

        public async Task <QuestionPack> SavePackAsync(QuestionPack pack)
        {
            var filter = Builders<QuestionPack>
                .Filter
                .Eq(p => p.Name, pack.Name);

            await _context.QuestionPacks.ReplaceOneAsync
            (
                filter,
                pack,
                new ReplaceOptions { IsUpsert = true}
            );
            return pack;
        }

        
    }
}
