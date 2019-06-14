using MongoDB.Bson;
using OgreMage.API.Contracts.Models;

namespace OgreMage.API.Models.Database
{
    public class PendingRSSFeedProcess : IMongoModel
    {
        public ObjectId Id { get; set; }
        public string RSSFeed { get; set; }
    }
}
