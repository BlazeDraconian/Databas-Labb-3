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

        public async Task <QuestionPack> DeletePackAsync(string packId)
        {
            var filter = Builders<QuestionPack>.Filter.Eq(p => p.Id, packId);
            var result = await _context.QuestionPacks.Find(filter).FirstOrDefaultAsync();
            if (result != null)
            {
                await _context.QuestionPacks.DeleteOneAsync(filter);
            }
            return result;
        }

        public async Task <QuestionPack> SavePackAsync(QuestionPack pack)
        {
            if (pack.Id == null)
            {
                await _context.QuestionPacks.InsertOneAsync(pack);
            }

            else
            {
                var filter = Builders<QuestionPack>.Filter.Eq(p => p.Id, pack.Id);

                await _context.QuestionPacks.ReplaceOneAsync
                (
                    filter,
                    pack,
                    new ReplaceOptions { IsUpsert = true }
                );

            }
                return pack;
            
        }
    }
}
