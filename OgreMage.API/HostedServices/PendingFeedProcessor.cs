using OgreMage.API.Contracts.Database;
using OgreMage.API.HostedServices.Base;
using OgreMage.API.Models.Database;
using OgreMage.API.Services.Feed;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace OgreMage.API.HostedServices
{
    public class PendingFeedProcessor : TimedHostedService
    {
        private readonly IMongoRepository MongoRepository;

        public PendingFeedProcessor(IMongoRepository mongoRepository) 
            : base(TimeSpan.Zero, TimeSpan.FromMinutes(3))
        {
            this.MongoRepository = mongoRepository;
        }
        
        /// <summary>
        /// Process pending inclusion podcasts.
        /// </summary>
        /// <param name="state"></param>
        public override void DoWork(object state)
        {
            IEnumerable<PendingRSSFeedProcess> pendingRSSFeeds = this.MongoRepository.Find<PendingRSSFeedProcess>(rssFeed => true);

            foreach (PendingRSSFeedProcess pendingRSSFeed in pendingRSSFeeds)
            {
                try
                {
                    RSSFeedProcessResult processResult = RSSFeedProcessor.Process(pendingRSSFeed.RSSFeed).Result;
                    bool existsPodcast = this.MongoRepository.Find<Podcast>(podcast => podcast.Title == processResult.Podcast.Title
                        && podcast.Author == processResult.Podcast.Author).Any();

                    if (!existsPodcast)
                    {
                        this.MongoRepository.Insert(processResult.Podcast);

                        foreach (Episode episode in processResult.Episodes)
                        {
                            episode.PodcastId = processResult.Podcast.Id;
                            this.MongoRepository.Insert(episode);
                        }
                    }

                    this.MongoRepository.Remove(pendingRSSFeed);
                }
                catch {
                    Debug.WriteLine($"Failed to process feed: {pendingRSSFeed.RSSFeed}");
                }
            }
        }
    }
}
