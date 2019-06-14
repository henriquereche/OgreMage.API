using System;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml.Linq;

namespace OgreMage.API.Extensions
{
    public static class SyndicationItemExtensions
    {
        public static Uri GetEpisodeImageUri(this SyndicationItem episode)
        {
            if (episode.ElementExtensions.Any(extension => extension.OuterName.ToLower() == "image"))
            {
                string uri = episode.ElementExtensions
                    .FirstOrDefault(extension => extension.OuterName.ToLower() == "image")
                    .GetObject<XElement>()
                    .FirstAttribute.Value;

                return Uri.IsWellFormedUriString(uri, UriKind.Absolute)
                    ? new Uri(uri, UriKind.Absolute)
                    : null;
            }

            return null;
        }

        public static TimeSpan? GetEpisodeDuration(this SyndicationItem episode)
        {
            if (episode.ElementExtensions.Any(extension => extension.OuterName.ToLower() == "duration"))
            {
                string duration = episode.ElementExtensions
                    .FirstOrDefault(extension => extension.OuterName.ToLower() == "duration")
                    .GetObject<XElement>()
                    .Value;

                if (!string.IsNullOrEmpty(duration) && duration.Split(":").Count() == 2)
                    duration = $"00:{duration}";

                return TimeSpan.TryParse(duration, out TimeSpan result)
                    ? (TimeSpan?)result
                    : null;
            }

            return null;
        }

        public static Uri GetEpisodeSource(this SyndicationItem episode)
        {
            return episode.Links.FirstOrDefault(link => link.MediaType?.ToLower().Contains("audio") == true).Uri;
        }
    }
}
