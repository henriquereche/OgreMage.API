using MongoDB.Bson;
using OgreMage.API.Contracts.Models;
using System;

namespace OgreMage.API.Models.Database
{
    public class Podcast : IMongoModel
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Uri Image { get; set; }
        public Uri RSSFeed { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime? LastSearch { get; set; }
        public bool Active { get; set; }
        public string Author { get; set; }
    }
}
