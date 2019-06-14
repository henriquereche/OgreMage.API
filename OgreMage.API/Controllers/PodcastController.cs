using LinqKit;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using OgreMage.API.Contracts.Database;
using OgreMage.API.Filters;
using OgreMage.API.Models;
using OgreMage.API.Models.Database;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace OgreMage.API.Controllers
{
    [Route("api/podcasts")]
    [ApiController]
    public class PodcastController : ControllerBase
    {
        private readonly IMongoRepository MongoRepository;

        public PodcastController(IMongoRepository mongoRepository)
        {
            this.MongoRepository = mongoRepository;
        }

        [HttpPost]
        public IActionResult Post([FromBody] ProcessRSSFeed feed)
        {
            if (!Uri.IsWellFormedUriString(feed.RSSFeed, UriKind.Absolute))
                return BadRequest("Bad formed RSSFeed URI.");

            bool existsPodcast = this.MongoRepository.Find<Podcast>(podcast => podcast.RSSFeed == new Uri(feed.RSSFeed)).Any();
            if (existsPodcast) return BadRequest("Podcast already exists.");

            PendingRSSFeedProcess pendingProcess = new PendingRSSFeedProcess { RSSFeed = feed.RSSFeed };
            this.MongoRepository.Insert(pendingProcess);

            return Created(Request.PathBase.Value, pendingProcess);
        }

        [HttpGet]
        public IActionResult Get([FromQuery] PodcastFilter filter)
        {
            Expression<Func<Podcast, bool>> podcastFilterExpression = podcast => true;

            if (!string.IsNullOrEmpty(filter.Title))
                podcastFilterExpression = podcastFilterExpression.And(
                    podcast => podcast.Title.ToLower().Contains(filter.Title.ToLower())
                );

            if (!string.IsNullOrEmpty(filter.Author))
                podcastFilterExpression = podcastFilterExpression.And(
                    podcast => podcast.Author.ToLower().Contains(filter.Author.ToLower())
                );

            if (!string.IsNullOrEmpty(filter.Description))
                podcastFilterExpression = podcastFilterExpression.And(
                    podcast => podcast.Description.ToLower().Contains(filter.Description.ToLower())
                );

            return Ok(
                this.MongoRepository.FindAndProject(
                    podcastFilterExpression,
                    podcast => new
                    {
                        podcast.Id,
                        podcast.Title,
                        podcast.Image,
                        podcast.Author,
                        podcast.LastUpdate
                    },
                    filter.Page, 
                    filter.PageSize
                )
            );
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] string id)
        {
            return Ok(
                this.MongoRepository.FindById<Podcast>(id)
            );
        }

        [HttpGet("{id}/episodes")]
        public IActionResult GetEpisodes([FromRoute] string id, [FromQuery] EpisodeFilter filter)
        {
            Expression<Func<Episode, bool>> episodeFilterExpression = episode => episode.PodcastId == new ObjectId(id);

            if (!string.IsNullOrEmpty(filter.Title))
                episodeFilterExpression = episodeFilterExpression.And(
                    episode => episode.Title.ToLower().Contains(filter.Title.ToLower())
                );

            if (!string.IsNullOrEmpty(filter.Summary))
                episodeFilterExpression = episodeFilterExpression.And(
                    episode => episode.Summary.ToLower().Contains(filter.Summary.ToLower())
                );

            return Ok(
                this.MongoRepository.FindAndProject(
                    episodeFilterExpression,
                    episode => new
                    {
                        episode.Id,
                        episode.Title,
                        episode.Image
                    },
                    filter.Page,
                    filter.PageSize
                )
            );
        }

        [HttpGet("{id}/episodes/{episodeId}")]
        public IActionResult GetEpisodeById([FromRoute] string id, [FromRoute] string episodeId)
        {
            return Ok(
                this.MongoRepository.Find<Episode>(episode => episode.PodcastId == new ObjectId(id) 
                    && episode.Id == new ObjectId(episodeId)
                ).FirstOrDefault()
            );
        }
    }
}