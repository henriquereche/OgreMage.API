using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml.Linq;

namespace OgreMage.API.Extensions
{
    public static class SyndicationFeedExtensions
    {
        public static string GetFeedAuthor(this SyndicationFeed feed)
        {
            if (feed.ElementExtensions.Any(x => x.OuterName == "author"))
                return feed.ElementExtensions
                    .FirstOrDefault(extension => extension.OuterName.ToLower() == "author")
                    .GetObject<XElement>().Value;

            if (feed.Authors != null && feed.Authors.Any())
            {
                SyndicationPerson author = feed.Authors.FirstOrDefault();
                return author.Name ?? author.Email;
            }

            return null;
        }
    }
}
