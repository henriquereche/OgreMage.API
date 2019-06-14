using OgreMage.API.Models.Database;
using System;
using System.Collections.Generic;

namespace OgreMage.API.Services.Feed
{
    public class RSSFeedProcessResult
    {
        public RSSFeedProcessResult(
            Podcast podcast, 
            IEnumerable<Episode> episodes, 
            DateTime lastUpdateDate
            )
        {
            this.LastUpdateDate = lastUpdateDate;
            this.Podcast = podcast;
            this.Episodes = episodes;
        }

        public DateTime LastUpdateDate { get; private set; }
        public Podcast Podcast { get; private set; }
        public IEnumerable<Episode> Episodes { get; private set; }
    }
}
