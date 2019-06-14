using MongoDB.Driver;
using OgreMage.API.Contracts.Models;

namespace OgreMage.API.Contracts.Database
{
    public interface IMongoConnection
    {
        IMongoClient Client { get; }
        IMongoDatabase Database { get; }
        IMongoCollection<TMongoModel> GetCollection<TMongoModel>()
            where TMongoModel : class, IMongoModel;
    }
}
