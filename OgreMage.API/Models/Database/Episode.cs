using MongoDB.Bson;
using OgreMage.API.Contracts.Models;
using System;

namespace OgreMage.API.Models.Database
{
    public class Episode : IMongoModel
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public Uri Image { get; set; }
        public DateTime ReleaseDate { get; set; }
        public TimeSpan? Duration { get; set; }
        public Uri Source { get; set; }
        public string FeedIdentifier { get; set; }
        public ObjectId PodcastId { get; set; }
    }
}
