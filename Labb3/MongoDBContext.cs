using Labb3.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3
{
    public class MongoDBContext
    {
        private readonly string _connectionString = "mongodb://localhost:27017/";
        private readonly string _databaseName = "DanielGustafsson";

        private readonly IMongoDatabase _database;


        public MongoDBContext()
        {
            var client = new MongoClient(_connectionString);
            _database = client.GetDatabase(_databaseName);
        }
        public IMongoCollection<QuestionPack> QuestionPacks =>
            _database.GetCollection<QuestionPack>("QuestionPacks");

    }
}
