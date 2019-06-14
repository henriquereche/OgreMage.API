using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using OgreMage.API.Contracts.Database;
using OgreMage.API.Contracts.Models;
using OgreMage.API.Models.Database;

namespace OgreMage.API.Services.Database
{
    public class MongoConnection : IMongoConnection
    {
        private readonly string ConnectionString;
        private readonly string DatabaseName;

        public MongoConnection(string connectionString, string databaseName)
        {
            this.ConnectionString = connectionString;
            this.DatabaseName = databaseName;

            this.Initialize();
        }

        public virtual void Initialize()
        {
            this.Client = new MongoClient(this.ConnectionString);
            this.Database = this.Client.GetDatabase(this.DatabaseName);
        }

        public IMongoCollection<TMongoModel> GetCollection<TMongoModel>()
            where TMongoModel : class, IMongoModel
        {
            return this.Database.GetCollection<TMongoModel>(typeof(TMongoModel).Name);
        }

        public IMongoClient Client { get; private set; }
        public IMongoDatabase Database { get; private set; }
    }
}
