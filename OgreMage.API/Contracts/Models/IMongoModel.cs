using MongoDB.Bson;

namespace OgreMage.API.Contracts.Models
{
    public interface IMongoModel
    {
        ObjectId Id { get; }
    }
}
