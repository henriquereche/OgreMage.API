using OgreMage.API.Extensions;
using OgreMage.API.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

namespace OgreMage.API.Services.Feed
{
    public static class RSSFeedProcessor
    {
        public static async Task<RSSFeedProcessResult> Process(string rssFeed)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage httpResponse = await httpClient.GetAsync(rssFeed);

                using (XmlReader xmlReader = XmlReader.Create(await httpResponse.Content.ReadAsStreamAsync()))
                {
                    SyndicationFeed feed = SyndicationFeed.Load(xmlReader);

                    Podcast podcast = new Podcast
                    {
                        Active = true,
                        Description = feed.Description.Text,
                        Title = feed.Title.Text,
                        Image = feed.ImageUrl,
                        LastSearch = DateTime.Now,
                        LastUpdate = feed.LastUpdatedTime.DateTime,
                        RSSFeed = new Uri(rssFeed),
                        Author = feed.GetFeedAuthor()
                    };

                    IEnumerable<Episode> episodes = feed.Items.Select(episode =>
                        new Episode
                        {
                            FeedIdentifier = episode.Id,
                            ReleaseDate = episode.PublishDate.DateTime,
                            Summary = episode.Summary.Text,
                            Title = episode.Title.Text,
                            Image = episode.GetEpisodeImageUri(),
                            Duration = episode.GetEpisodeDuration(),
                            Source = episode.GetEpisodeSource()
                        }
                    ).ToList();

                    return new RSSFeedProcessResult(podcast, episodes, podcast.LastUpdate);
                }
            }
        }
    }
}
